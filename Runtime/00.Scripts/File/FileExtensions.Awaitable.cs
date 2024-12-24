#if UNITY_2023_2_OR_NEWER
using System.Threading;
using UnityEngine;
using System;
using System.IO;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Creator_Hian.Unity.Common
{
    public static partial class FileExtensions
    {
        /// <summary>
        /// 비동기적으로 파일을 쓰는 메서드 (Awaitable)
        /// </summary>
        public static async Awaitable WriteFileToPathAwaitable(
            string path,
            byte[] dataBytes,
            int bufferSize = AutoCalculateBufferSize,
            CancellationToken cancellationToken = default
        )
        {
            if (bufferSize < -1) // AutoCalculateBufferSize(-1) 허용
            {
                throw new ArgumentException(
                    "버퍼 크기는 -1(자동) 또는 양수여야 합니다.",
                    nameof(bufferSize)
                );
            }

            string fullPath = null;
            FileStream fileStream = null;

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                ValidatePath(path);
                fullPath = Path.GetFullPath(path);

                // 파일 크기에 따른 버퍼 크기 최적화
                int actualBufferSize =
                    bufferSize == AutoCalculateBufferSize
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
                    FileOptions.Asynchronous | FileOptions.WriteThrough
                );

                // 대용량 파일의 경우 청크 단위로 쓰기
                if (dataBytes.Length > LargeFileThreshold)
                {
                    await WriteInChunksAsync(
                        fileStream,
                        dataBytes,
                        actualBufferSize,
                        cancellationToken
                    );
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
                await CleanupAwaitable(fullPath);
                throw new FileWriteException($"파일 쓰기가 취소되었습니다: {path}");
            }
            catch (Exception ex)
                when (ex is not FilePathException and not DirectoryCreationException)
            {
                if (fileStream != null)
                {
                    await fileStream.DisposeAsync();
                }
                await CleanupAwaitable(fullPath);
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

        /// <summary>
        /// 두 파일이 동일한지 비교합니다. (Awaitable)
        /// </summary>
        public static async Awaitable<bool> CompareFilesAwaitable(
            string path1,
            string path2,
            bool compareContent = true,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                ValidatePath(path1);
                ValidatePath(path2);

                string fullPath1 = Path.GetFullPath(path1);
                string fullPath2 = Path.GetFullPath(path2);

                if (!File.Exists(fullPath1) || !File.Exists(fullPath2))
                {
                    throw new FileNotFoundException("하나 이상의 파일을 찾을 수 없습니다.");
                }

                // 파일 크기 비교
                FileInfo file1 = new FileInfo(fullPath1);
                FileInfo file2 = new FileInfo(fullPath2);

                if (file1.Length != file2.Length)
                {
                    return false;
                }

                // 내용 비교가 필요없으면 여기서 종료
                if (!compareContent)
                {
                    return true;
                }

                // 파일 크기에 따른 버퍼 크기 최적화
                int bufferSize = CalculateOptimalBufferSize(file1.Length);

                await using FileStream fs1 = new FileStream(
                    fullPath1,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite,
                    bufferSize,
                    FileOptions.Asynchronous
                );
                await using FileStream fs2 = new FileStream(
                    fullPath2,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite,
                    bufferSize,
                    FileOptions.Asynchronous
                );

                return await CompareStreamContentsAwaitable(
                    fs1,
                    fs2,
                    bufferSize,
                    cancellationToken
                );
            }
            catch (Exception ex) when (ex is not (FilePathException or FileNotFoundException))
            {
                throw new FileOperationException($"파일 비교 실패: {path1}, {path2}", ex);
            }
        }

        private static async Awaitable<bool> CompareStreamContentsAwaitable(
            Stream stream1,
            Stream stream2,
            int bufferSize,
            CancellationToken cancellationToken
        )
        {
            byte[] buffer1 = new byte[bufferSize];
            byte[] buffer2 = new byte[bufferSize];

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                int count1 = await stream1.ReadAsync(buffer1, 0, bufferSize, cancellationToken);
                int count2 = await stream2.ReadAsync(buffer2, 0, bufferSize, cancellationToken);

                if (count1 != count2)
                {
                    return false;
                }

                if (count1 == 0)
                {
                    return true;
                }

                for (int i = 0; i < count1; i++)
                {
                    if (buffer1[i] != buffer2[i])
                    {
                        return false;
                    }
                }

                // 주기적으로 다른 작업이 실행될 수 있도록 양보
                if (count1 == bufferSize)
                {
                    await Task.Yield();
                }
            }
        }

        /// <summary>
        /// 파일을 비동기적으로 복사합니다. (Awaitable)
        /// </summary>
        public static async Awaitable CopyFileAwaitable(
            string sourcePath,
            string destinationPath,
            bool overwrite = false,
            int bufferSize = 4096,
            CancellationToken cancellationToken = default
        )
        {
            if (sourcePath == null)
            {
                throw new ArgumentNullException(nameof(sourcePath));
            }

            if (destinationPath == null)
            {
                throw new ArgumentNullException(nameof(destinationPath));
            }

            string fullDestPath = null;

            try
            {
                // 경로 검증
                ValidatePath(sourcePath);
                ValidatePath(destinationPath);

                string fullSourcePath = Path.GetFullPath(sourcePath);
                fullDestPath = Path.GetFullPath(destinationPath);

                // 파일 존재 여부 확인
                if (!File.Exists(fullSourcePath))
                {
                    throw new FileNotFoundException($"원본 파일을 찾을 수 없습니다: {sourcePath}");
                }

                if (File.Exists(fullDestPath) && !overwrite)
                {
                    throw new FileOperationException(
                        $"대상 파일이 이미 존재합니다: {destinationPath}"
                    );
                }

                // 디렉토리 생성을 별도 Task로 실행
                await Task.Run(() => EnsureDirectoryExists(fullDestPath), cancellationToken);

                // 실제 복사 작업 수행
                await DoCopyAwaitable(fullSourcePath, fullDestPath, bufferSize, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                await CleanupAwaitable(fullDestPath);
                throw new FileOperationException(
                    $"파일 복사가 취소되었습니다: {sourcePath} -> {destinationPath}"
                );
            }
            catch (Exception ex) when (ex is not (FilePathException or FileNotFoundException))
            {
                throw new FileOperationException(
                    $"파일 복사 실패: {sourcePath} -> {destinationPath}",
                    ex
                );
            }
        }

        private static async Awaitable DoCopyAwaitable(
            string sourcePath,
            string destPath,
            int bufferSize,
            CancellationToken cancellationToken
        )
        {
            // 스트림 작업을 Task.Run으로 래핑
            await Task.Run(
                async () =>
                {
                    using FileStream sourceStream = new FileStream(
                        sourcePath,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.Read,
                        bufferSize
                    );

                    using FileStream destStream = new FileStream(
                        destPath,
                        FileMode.Create,
                        FileAccess.Write,
                        FileShare.None,
                        bufferSize
                    );

                    byte[] buffer = new byte[bufferSize];
                    int bytesRead;

                    while (
                        (
                            bytesRead = await sourceStream.ReadAsync(
                                buffer,
                                0,
                                buffer.Length,
                                cancellationToken
                            )
                        ) > 0
                    )
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        await destStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);

                        // 주기적으로 컨텍스트 전환 허용
                        if (bytesRead == bufferSize)
                        {
                            await Task.Yield();
                        }
                    }
                },
                cancellationToken
            );
        }

        /// <summary>
        /// 파일을 비동기적으로 정리합니다. (Awaitable)
        /// </summary>
        public static async Awaitable CleanupAwaitable(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            try
            {
                await Task.Run(() =>
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                });
            }
            catch
            {
                // 정리 실패는 무시
            }
        }
    }
}
#endif
