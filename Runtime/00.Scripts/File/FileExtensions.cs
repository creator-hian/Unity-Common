using System;
using System.IO;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Creator_Hian.Unity.Common
{
    public static partial class FileExtensions
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

        /// <summary>
        /// 두 파일이 동일한지 비교합니다.
        /// </summary>
        public static bool CompareFiles(
            string path1,
            string path2,
            bool compareContent = true)
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

                // 내용 비교가 필요없으면 파일 존재 여부만으로 판단
                if (!compareContent)
                    return true;

                // 파일 크기 비교
                var file1 = new FileInfo(fullPath1);
                var file2 = new FileInfo(fullPath2);

                if (file1.Length != file2.Length)
                    return false;

                // 파일 크기에 따른 버퍼 크기 최적화
                int bufferSize = CalculateOptimalBufferSize(file1.Length);

                using var fs1 = new FileStream(
                    fullPath1,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite,
                    bufferSize);
                using var fs2 = new FileStream(
                    fullPath2,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite,
                    bufferSize);

                return CompareStreamContents(fs1, fs2, bufferSize);
            }
            catch (Exception ex) when (!(ex is FilePathException || ex is FileNotFoundException))
            {
                throw new FileOperationException($"파일 비교 실패: {path1}, {path2}", ex);
            }
        }

        private static bool CompareStreamContents(
            Stream stream1,
            Stream stream2,
            int bufferSize)
        {
            var buffer1 = new byte[bufferSize];
            var buffer2 = new byte[bufferSize];

            while (true)
            {
                var count1 = stream1.Read(buffer1, 0, bufferSize);
                var count2 = stream2.Read(buffer2, 0, bufferSize);

                if (count1 != count2)
                    return false;

                if (count1 == 0)
                    return true;

                for (int i = 0; i < count1; i++)
                {
                    if (buffer1[i] != buffer2[i])
                        return false;
                }
            }
        }

        /// <summary>
        /// 파일을 동기적으로 복사합니다.
        /// </summary>
        public static void CopyFile(
            string sourcePath,
            string destinationPath,
            bool overwrite = false,
            int bufferSize = 4096)
        {
            if (sourcePath == null)
                throw new ArgumentNullException(nameof(sourcePath));
            if (destinationPath == null)
                throw new ArgumentNullException(nameof(destinationPath));

            try
            {
                // 경로 검증
                ValidatePath(sourcePath);
                ValidatePath(destinationPath);

                var fullSourcePath = Path.GetFullPath(sourcePath);
                var fullDestPath = Path.GetFullPath(destinationPath);

                // 파일 존재 여부 확인
                if (!File.Exists(fullSourcePath))
                    throw new FileNotFoundException($"원본 파일을 찾을 수 없습니다: {sourcePath}");

                if (File.Exists(fullDestPath) && !overwrite)
                    throw new FileOperationException($"대상 파일이 이미 존재합니다: {destinationPath}");

                // 디렉토리 생성
                EnsureDirectoryExists(fullDestPath);

                // 실제 복사 작업 수행
                DoCopy(fullSourcePath, fullDestPath, bufferSize);
            }
            catch (Exception ex) when (!(ex is FilePathException || ex is FileNotFoundException))
            {
                throw new FileOperationException($"파일 복사 실패: {sourcePath} -> {destinationPath}", ex);
            }
        }

        private static void DoCopy(
            string sourcePath,
            string destPath,
            int bufferSize)
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

            while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                destStream.Write(buffer, 0, bytesRead);
            }
        }

        /// <summary>
        /// 파일을 동기적으로 정리합니다.
        /// </summary>
        private static void Cleanup(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            try
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch
            {
                // 정리 실패는 무시
            }
        }
    }
}