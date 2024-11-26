using System;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using System.Threading.Tasks;

namespace Creator_Hian.Unity.Common
{
    public static class FileExtensions
    {
        private const int MAX_PATH_LENGTH = 260; // Windows 경로 길이 제한
        private const string ERROR_MSG_NULL_PATH = "경로가 null이거나 비어있습니다: {0}";
        private const string ERROR_MSG_INVALID_PATH = "유효하지 않은 파일 경로입니다: {0}";

        /// <summary>
        /// 파일 경로 유효성 검사를 수행합니다.
        /// </summary>
        private static void ValidatePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new FilePathException(string.Format(ERROR_MSG_NULL_PATH, path));
            }

            var fullPath = Path.GetFullPath(path);
            if (!IsValidPath(fullPath))
            {
                throw new FilePathException(string.Format(ERROR_MSG_INVALID_PATH, fullPath));
            }
        }

        public static void WriteFileToPath(string path, byte[] dataBytes)
        {
            try
            {
                ValidatePath(path);
                var fullPath = Path.GetFullPath(path);
                EnsureDirectoryExists(fullPath);

                using (var fileStream = new FileStream(
                           fullPath,
                           FileMode.Create,
                           FileAccess.Write,
                           FileShare.None))
                {
                    fileStream.Write(dataBytes, 0, dataBytes.Length);
                }
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
            int bufferSize = 4096,
            CancellationToken cancellationToken = default)
        {
            try
            {
                ValidatePath(path);
                var fullPath = Path.GetFullPath(path);
                EnsureDirectoryExists(fullPath);

                using (var fileStream = new FileStream(
                           fullPath,
                           FileMode.Create,
                           FileAccess.Write,
                           FileShare.None,
                           bufferSize))
                {
                    await fileStream.WriteAsync(dataBytes, 0, dataBytes.Length, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                throw new FileWriteException($"파일 쓰기가 취소되었습니다: {path}");
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
                throw new FileWriteException($"비동기 파일 쓰기 실패: {path}", ex);
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
                if (fullPath.Length >= MAX_PATH_LENGTH)
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
        public static bool CompareFiles(string path1, string path2, bool compareContent = true)
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

                // 내용 비교
                const int BUFFER_SIZE = 4096;
                using (var fs1 = File.OpenRead(fullPath1))
                using (var fs2 = File.OpenRead(fullPath2))
                {
                    var buffer1 = new byte[BUFFER_SIZE];
                    var buffer2 = new byte[BUFFER_SIZE];

                    while (true)
                    {
                        int count1 = fs1.Read(buffer1, 0, BUFFER_SIZE);
                        int count2 = fs2.Read(buffer2, 0, BUFFER_SIZE);

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
            }
            catch (Exception ex) when (!(ex is FilePathException || ex is FileNotFoundException))
            {
                throw new FileOperationException($"파일 비교 실패: {path1}, {path2}", ex);
            }
        }

    }
}