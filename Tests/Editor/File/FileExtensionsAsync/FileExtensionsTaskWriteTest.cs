using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Creator_Hian.Unity.Common;
using NUnit.Framework;
using Random = System.Random;

namespace FileExtensions.Async
{
    /// <summary>
    /// FileExtensions의 비동기 파일 쓰기(Task) 관련 기능을 테스트합니다.
    /// </summary>
    /// <remarks>
    /// 테스트 범위:
    /// - 기본 파일 쓰기 동작
    /// - 예외 처리 (잘못된 경로, null 경로)
    /// - 취소 동작
    /// - 중첩 디렉토리 생성
    /// - 대용량 파일 처리
    /// </remarks>
    public class FileExtensionsTaskWriteTest : FileExtensionsTaskTestBase
    {
        /// <summary>
        /// 유효한 경로에 파일을 비동기적으로 쓰는 기본 동작을 테스트합니다.
        /// </summary>
        /// <remarks>
        /// 검증 항목:
        /// 1. 파일이 생성되는지 확인
        /// 2. 쓰여진 데이터가 원본과 일치하는지 확인
        /// 3. 파일 핸들이 적절히 해제되는지 확인
        /// </remarks>
        [Test]
        public async Task WriteFileToPathAsync_ValidPath_CreatesFile()
        {
            try
            {
                // Act
                await Creator_Hian.Unity.Common.FileExtensions.WriteFileToPathAsync(
                    _testFile1,
                    _testData
                );
                await Task.Delay(100); // 파일 핸들이 해제될 때까지 대기

                // Assert
                Assert.That(File.Exists(_testFile1), Is.True);
                using FileStream fs = new FileStream(
                    _testFile1,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite
                );
                byte[] readData = new byte[_testData.Length];
                _ = await fs.ReadAsync(readData, 0, readData.Length);
                Assert.That(readData, Is.EqualTo(_testData));
            }
            finally
            {
                await Task.Delay(100); // 파일 핸들이 해제될 때까지 대기
            }
        }

        /// <summary>
        /// 잘못된 경로로 파일 쓰기를 시도할 때의 예외 처리를 테스트합니다.
        /// </summary>
        [Test]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task WriteFileToPathAsync_InvalidPath_ThrowsException()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            // Arrange
            string invalidPath = Path.Combine(_testDirectory, new string('*', 300));

            // Act & Assert
            FilePathException ex = Assert.ThrowsAsync<FilePathException>(
                async () =>
                    await Creator_Hian.Unity.Common.FileExtensions.WriteFileToPathAsync(
                        invalidPath,
                        _testData
                    )
            );

            StringAssert.Contains("유효하지 않은 파일 경로입니다", ex.Message);
        }

        /// <summary>
        /// null 경로로 파일 쓰기를 시도할 때 FilePathException이 발생하는지 테스트합니다.
        /// </summary>
        [Test]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task WriteFileToPathAsync_NullPath_ThrowsException()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            // Act & Assert
            FilePathException ex = Assert.ThrowsAsync<FilePathException>(
                async () =>
                    await Creator_Hian.Unity.Common.FileExtensions.WriteFileToPathAsync(
                        null,
                        _testData
                    )
            );

            StringAssert.Contains("경로가 null이거나 비어있습니다", ex.Message);
        }

        /// <summary>
        /// 비동기 파일 쓰기 중 취소 요청이 발생했을 때의 동작을 테스트합니다.
        /// </summary>
        [Test]
        public async Task WriteFileToPathAsync_CancellationRequested_ThrowsException()
        {
            // Arrange
            string filePath = Path.Combine(_testDirectory, "testCancelled.txt");
            using CancellationTokenSource cts = new CancellationTokenSource();
            cts.Cancel(); // 즉시 취소

            try
            {
                // Act & Assert
                FileWriteException ex = Assert.ThrowsAsync<FileWriteException>(
                    async () =>
                        await Creator_Hian.Unity.Common.FileExtensions.WriteFileToPathAsync(
                            filePath,
                            _testData,
                            cancellationToken: cts.Token
                        )
                );

                StringAssert.Contains("파일 쓰기가 취소되었습니다", ex.Message);
                Assert.That(File.Exists(filePath), Is.False, "취소된 파일이 남아있습니다.");
            }
            finally
            {
                await Task.Delay(100);
            }
        }

        /// <summary>
        /// 중첩된 디렉토리 구조에서 파일 쓰기가 정상 동작하는지 테스트합니다.
        /// </summary>
        [Test]
        public async Task WriteFileToPathAsync_CreateNestedDirectories_Success()
        {
            try
            {
                // Arrange
                string nestedPath = Path.Combine(_testDirectory, "nested", "folders", "test.txt");

                // Act
                await Creator_Hian.Unity.Common.FileExtensions.WriteFileToPathAsync(
                    nestedPath,
                    _testData
                );
                await Task.Delay(100);

                // Assert
                Assert.That(File.Exists(nestedPath), Is.True);
                using FileStream fs = new FileStream(
                    nestedPath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite
                );
                byte[] readData = new byte[_testData.Length];
                _ = await fs.ReadAsync(readData, 0, readData.Length);
                Assert.That(readData, Is.EqualTo(_testData));
            }
            finally
            {
                await Task.Delay(100);
            }
        }

        /// <summary>
        /// 대용량 파일의 비동기식 쓰기가 정상 동작하는지 테스트합니다.
        /// </summary>
        [Test]
        public async Task WriteFileToPathAsync_LargeFile_Success()
        {
            try
            {
                // Arrange
                string filePath = Path.Combine(_testDirectory, "largeFile.txt");
                byte[] largeData = new byte[1024 * 1024]; // 1MB
                new Random().NextBytes(largeData);

                // Act
                await Creator_Hian.Unity.Common.FileExtensions.WriteFileToPathAsync(
                    filePath,
                    largeData,
                    bufferSize: 8192
                );
                await Task.Delay(100);

                // Assert
                Assert.That(File.Exists(filePath), Is.True);
                using (
                    FileStream fs = new FileStream(
                        filePath,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.ReadWrite
                    )
                )
                {
                    byte[] readData = new byte[largeData.Length];
                    _ = await fs.ReadAsync(readData, 0, readData.Length);
                    Assert.That(readData, Is.EqualTo(largeData));
                }

                // 파일 크기 확인
                FileInfo fileInfo = new FileInfo(filePath);
                Assert.That(fileInfo.Length, Is.EqualTo(largeData.Length));
            }
            finally
            {
                await Task.Delay(100);
            }
        }
    }
}
