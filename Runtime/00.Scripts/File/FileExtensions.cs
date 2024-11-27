using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Creator_Hian.Unity.Common
{
    public static class FileExtensions
    {
        private const int MaxPathLength = 260; // Windows 경로 길이 제한
        private const string ErrorMsgNullPath = "경로가 null이거나 비어있습니다: {0}";
        private const string ErrorMsgInvalidPath = "유효하지 않은 파일 경로입니다: {0}";
        private const int DefaultBufferSize = 81920; // 80KB
        private const int LargeFileThreshold = 52428800; // 50MB
        private const int AutoCalculateBufferSize = -1; // 버퍼 크기 자동 계산 플래그

        /// <summary>
        /// 파일 경로 유효성 검사를 수행합니다.
        /// </summary>
        private static void ValidatePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new FilePathException(string.Format(ErrorMsgNullPath, path));
            }

            var fullPath = Path.GetFullPath(path);
            if (!IsValidPath(fullPath))
            {
                throw new FilePathException(string.Format(ErrorMsgInvalidPath, fullPath));
            }
        }

        public static void WriteFileToPath(string path, byte[] dataBytes)
        {
            try
            {
                ValidatePath(path);
                var fullPath = Path.GetFullPath(path);
                EnsureDirectoryExists(fullPath);

                using var fileStream = new FileStream(
                    fullPath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None);
                fileStream.Write(dataBytes, 0, dataBytes.Length);
            }
            catch (FilePathException)
            {
                throw;
            }
            catch (DirectoryCreationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FileWriteException($"파일 쓰기 실패: {path}", ex);
            }
        }

        /// <summary>
        /// 비동기적으로 파일을 쓰는 메서드
        /// </summary>
        public static async Task WriteFileToPathAsync(
            string path,
            byte[] dataBytes,
            int bufferSize = AutoCalculateBufferSize,
            CancellationToken cancellationToken = default)
        {
            if (bufferSize < -1) // AutoCalculateBufferSize(-1) 허용
            {
                throw new ArgumentException("버퍼 크기는 -1(자동) 또는 양수여야 합니다.", nameof(bufferSize));
            }

            string fullPath = null;
            FileStream fileStream = null;

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                ValidatePath(path);
                fullPath = Path.GetFullPath(path);

                // 파일 크기에 따른 버퍼 크기 최적화
                int actualBufferSize = bufferSize == AutoCalculateBufferSize 
                    ? CalculateOptimalBufferSize(dataBytes.Length)
                    : bufferSize;

                // 디렉토리 생성을 비동기로 처리
                await Task.Run(() => EnsureDirectoryExists(fullPath), cancellationToken);

                // 파일 스트림 생성을 비동기로 처리
                fileStream = new FileStream(
                    fullPath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.Read,
                    actualBufferSize,
                    FileOptions.Asynchronous | FileOptions.WriteThrough);

                // 대용량 파일의 경우 청크 단위로 쓰기
                if (dataBytes.Length > LargeFileThreshold)
                {
                    await WriteInChunksAsync(fileStream, dataBytes, actualBufferSize, cancellationToken);
                }
                else
                {
                    await fileStream.WriteAsync(dataBytes, 0, dataBytes.Length, cancellationToken);
                }
                
                await fileStream.FlushAsync(cancellationToken);
                fileStream = null;
            }
            catch (OperationCanceledException)
            {
                if (fileStream != null)
                {
                    await fileStream.DisposeAsync();
                    fileStream = null;
                }

                // 취소 시 부분 파일 정리
                await CleanupFileAsync(fullPath);
                throw new FileWriteException($"파일 쓰기가 취소되었습니다: {path}");
            }
            catch (Exception ex) when (ex is not FilePathException && ex is not DirectoryCreationException)
            {
                if (fileStream != null)
                {
                    await fileStream.DisposeAsync();
                }
                await CleanupFileAsync(fullPath);
                throw new FileWriteException($"비동기 파일 쓰기 실패: {path}", ex);
            }
            finally
            {
                if (fileStream != null)
                {
                    await fileStream.DisposeAsync();
                }
            }
        }

        private static int CalculateOptimalBufferSize(long fileSize)
        {
            if (fileSize <= 0) return DefaultBufferSize;
            
            // 파일 크기에 따른 버퍼 크기 조정
            if (fileSize > LargeFileThreshold) // 50MB 이상
            {
                return Math.Min(262144, Math.Max(DefaultBufferSize, (int)(fileSize / 100))); // 최대 256KB
            }
            
            return DefaultBufferSize;
        }

        private static async Task WriteInChunksAsync(
            FileStream fileStream,
            byte[] data,
            int bufferSize,
            CancellationToken cancellationToken)
        {
            int offset = 0;
            int remainingBytes = data.Length;

            while (remainingBytes > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();

                int chunkSize = Math.Min(bufferSize, remainingBytes);
                await fileStream.WriteAsync(data, offset, chunkSize, cancellationToken);
                
                offset += chunkSize;
                remainingBytes -= chunkSize;

                // 주기적으로 다른 작업이 실행될 수 있도록 양보
                if (remainingBytes > 0)
                {
                    await Task.Yield();
                }
            }
        }

        private static async Task CleanupFileAsync(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            try
            {
                await Task.Run(() =>
                {
                    if (File.Exists(path))
                        File.Delete(path);
                });
            }
            catch
            {
                // 정리 실패는 무시
            }
        }

        /// <summary>
        /// 지정된 경로의 디렉토리가 존재하는지 확인하고, 없다면 생성합니다.
        /// </summary>
        /// <param name="path">확인할 파일의 전체 경로</param>
        /// <exception cref="DirectoryCreationException">
        /// 디렉토리 생성에 실패하거나 권한이 없는 경우 발생합니다.
        /// </exception>
        /// <remarks>
        /// 이 메서드는 재귀적으로 모든 상위 디렉토리도 생성합니다.
        /// UAC(User Account Control) 권한이 필요할 수 있습니다.
        /// </remarks>
        private static void EnsureDirectoryExists(string path)
        {
            try
            {
                string directoryPath = Path.GetDirectoryName(path);
                if (string.IsNullOrEmpty(directoryPath))
                {
                    throw new DirectoryCreationException($"Directory path is null or empty for file path: {path}");
                }

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
            }
            catch (Exception ex)
            {
                throw new DirectoryCreationException($"Failed to ensure directory exists: {path}. Exception: {ex}");
            }
        }

        private static bool IsValidPath(string path)
        {
            try
            {
                // 이미 ValidatePath에서 null 체크를 하므로 중복 검사 제거
                var fullPath = Path.GetFullPath(path);

                // 추가적인 보안 검사
                if (Path.GetInvalidPathChars().Any(path.Contains))
                    return false;

                // 절대 경로 길이 검사 (Windows의 경우 260자 제한)
                if (fullPath.Length >= MaxPathLength)
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 파일의 크기를 바이트 단위로 반환합니다.
        /// </summary>
        /// <param name="path">파일 경로</param>
        /// <returns>파일 크기(바이트)</returns>
        /// <exception cref="FilePathException">파일 경로가 유효하지 않은 경우</exception>
        /// <exception cref="FileNotFoundException">파일이 존재하지 않는 경우</exception>
        public static long GetFileSize(string path)
        {
            try
            {
                ValidatePath(path);
                var fullPath = Path.GetFullPath(path);
                
                if (!File.Exists(fullPath))
                {
                    throw new FileNotFoundException($"파일을 찾을 수 없습니다: {path}");
                }

                return new FileInfo(fullPath).Length;
            }
            catch (Exception ex) when (!(ex is FilePathException || ex is FileNotFoundException))
            {
                throw new FileOperationException($"파일 크기 확인 실패: {path}", ex);
            }
        }

        /// <summary>
        /// 두 파일이 동일한지 비교합니다.
        /// </summary>
        /// <param name="path1">첫 번째 파일 경로</param>
        /// <param name="path2">두 번째 파일 경로</param>
        /// <param name="compareContent">내용까지 비교할지 여부 (기본값: true)</param>
        /// <returns>파일이 동일하면 true, 다르면 false</returns>
        /// <exception cref="FilePathException">파일 경로가 유효하지 않은 경우</exception>
        /// <exception cref="FileNotFoundException">파일이 존재하지 않는 경우</exception>
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once CognitiveComplexity
        public static async Task<bool> CompareFilesAsync(
            string path1, 
            string path2, 
            bool compareContent = true,
            CancellationToken cancellationToken = default)
        {
            try
            {
                ValidatePath(path1);
                ValidatePath(path2);
                
                var fullPath1 = Path.GetFullPath(path1);
                var fullPath2 = Path.GetFullPath(path2);

                if (!File.Exists(fullPath1) || !File.Exists(fullPath2))
                {
                    throw new FileNotFoundException("하나 이상의 파일을 찾을 수 없습니다.");
                }

                // 파일 크기 비교
                var file1 = new FileInfo(fullPath1);
                var file2 = new FileInfo(fullPath2);

                if (file1.Length != file2.Length)
                    return false;

                // 내용 비교가 필요없으면 여기서 종료
                if (!compareContent)
                    return true;

                // 파일 크기에 따른 버퍼 크기 최적화
                int bufferSize = CalculateOptimalBufferSize(file1.Length);
                
                await using var fs1 = new FileStream(
                    fullPath1, 
                    FileMode.Open, 
                    FileAccess.Read, 
                    FileShare.ReadWrite,
                    bufferSize, 
                    FileOptions.Asynchronous);
                await using var fs2 = new FileStream(
                    fullPath2, 
                    FileMode.Open, 
                    FileAccess.Read, 
                    FileShare.ReadWrite,
                    bufferSize, 
                    FileOptions.Asynchronous);
                
                return await CompareStreamContentsAsync(fs1, fs2, bufferSize, cancellationToken);
            }
            catch (Exception ex) when (!(ex is FilePathException || ex is FileNotFoundException))
            {
                throw new FileOperationException($"파일 비교 실패: {path1}, {path2}", ex);
            }
        }

        private static async Task<bool> CompareStreamContentsAsync(
            Stream stream1,
            Stream stream2,
            int bufferSize,
            CancellationToken cancellationToken)
        {
            var buffer1 = new byte[bufferSize];
            var buffer2 = new byte[bufferSize];

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                var count1 = await stream1.ReadAsync(buffer1, 0, bufferSize, cancellationToken);
                var count2 = await stream2.ReadAsync(buffer2, 0, bufferSize, cancellationToken);

                if (count1 != count2)
                    return false;

                if (count1 == 0)
                    return true;

                for (int i = 0; i < count1; i++)
                {
                    if (buffer1[i] != buffer2[i])
                        return false;
                }

                // 주기적으로 다른 작업이 실행될 수 있도록 양보
                if (count1 == bufferSize)
                {
                    await Task.Yield();
                }
            }
        }

        /// <summary>
        /// 파일을 비동기적으로 복사합니다.
        /// </summary>
        /// <param name="sourcePath">원본 파일 경로</param>
        /// <param name="destinationPath">대상 파일 경로</param>
        /// <param name="overwrite">기존 파일 덮어쓰기 여부</param>
        /// <param name="bufferSize">복사 시 사용할 버퍼 크기</param>
        /// <param name="cancellationToken">작업 취소 토큰</param>
        /// <returns>복사 작업 완료를 나타내는 Task</returns>
        /// <exception cref="ArgumentNullException">sourcePath 또는 destinationPath가 null인 경우</exception>
        /// <exception cref="FileNotFoundException">원본 파일이 존재하지 않는 경우</exception>
        /// <exception cref="FileOperationException">파일 복사 중 오류가 발생한 경우</exception>
        public static async Task CopyFileAsync(
            string sourcePath, 
            string destinationPath, 
            bool overwrite = false,
            int bufferSize = 4096,
            CancellationToken cancellationToken = default)
        {
            if (sourcePath == null)
                throw new ArgumentNullException(nameof(sourcePath));
            if (destinationPath == null)
                throw new ArgumentNullException(nameof(destinationPath));

            string fullDestPath = null;

            try
            {
                // 경로 검증
                ValidatePath(sourcePath);
                ValidatePath(destinationPath);
                
                var fullSourcePath = Path.GetFullPath(sourcePath);
                fullDestPath = Path.GetFullPath(destinationPath);

                // 파일 존재 여부 확인
                if (!File.Exists(fullSourcePath))
                    throw new FileNotFoundException($"원본 파일을 찾을 수 없습니다: {sourcePath}");

                if (File.Exists(fullDestPath) && !overwrite)
                    throw new FileOperationException($"대상 파일이 이미 존재합니다: {destinationPath}");

                // 디렉토리 생성을 별도 Task로 실행
                await Task.Run(() => EnsureDirectoryExists(fullDestPath), cancellationToken);

                // 실제 복사 작업 수행
                await DoCopyAsync(fullSourcePath, fullDestPath, bufferSize, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                await CleanupAsync(fullDestPath);
                throw new FileOperationException($"파일 복사가 취소되었습니다: {sourcePath} -> {destinationPath}");
            }
            catch (Exception ex) when (!(ex is FilePathException || ex is FileNotFoundException))
            {
                throw new FileOperationException($"파일 복사 실패: {sourcePath} -> {destinationPath}", ex);
            }
        }

        private static async Task DoCopyAsync(
            string sourcePath,
            string destPath,
            int bufferSize,
            CancellationToken cancellationToken)
        {
            // 스트림 작업을 Task.Run으로 래핑
            await Task.Run(async () =>
            {
                using var sourceStream = new FileStream(
                    sourcePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    bufferSize);

                using var destStream = new FileStream(
                    destPath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None,
                    bufferSize);

                var buffer = new byte[bufferSize];
                int bytesRead;

                while ((bytesRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await destStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);
                    
                    // 주기적으로 컨텍스트 전환 허용
                    if (bytesRead == bufferSize)
                        await Task.Yield();
                }
            }, cancellationToken);
        }

        private static async Task CleanupAsync(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            try
            {
                await Task.Run(() =>
                {
                    if (File.Exists(path))
                        File.Delete(path);
                });
            }
            catch
            {
                // 정리 실패는 무시
            }
        }

        /// <summary>
        /// 파일이 다른 프로세스에 의해 잠겨있는지 확인합니다.
        /// </summary>
        public static bool IsFileLocked(string path)
        {
            try
            {
                ValidatePath(path);
                var fullPath = Path.GetFullPath(path);

                using var stream = File.Open(fullPath, FileMode.Open, FileAccess.Write, FileShare.Read);
                return false;
            }
            catch (IOException)
            {
                return true;
            }
            catch (Exception ex)
            {
                throw new FileOperationException($"파일 잠금 상태 확인 실패: {path}", ex);
            }
        }
    }
}