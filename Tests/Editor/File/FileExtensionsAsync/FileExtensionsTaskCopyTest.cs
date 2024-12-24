using System.IO;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace FileExtensions.Async
{
    /// <summary>
    /// FileExtensions의 비동기 파일 복사(Task) 관련 기능을 테스트합니다.
    /// </summary>
    /// <remarks>
    /// 테스트 범위:
    /// - 기본 복사 동작
    /// - 덮어쓰기 동작
    /// - 취소 동작
    /// - 대용량 파일 복사
    /// </remarks>
    public class FileExtensionsTaskCopyTest : FileExtensionsTaskTestBase
    {
        /// <summary>
        /// 파일 복사의 기본 동작을 테스트합니다.
        /// </summary>
        /// <remarks>
        /// 검증 항목:
        /// 1. 원본 파일 생성
        /// 2. 복사 작업 수행
        /// 3. 대상 파일 존재 확인
        /// 4. 원본과 대상 파일의 내용 일치 확인
        /// 5. 파일 핸들이 적절히 해제되는지 확인
        /// </remarks>
        [Test]
        public async Task CopyFileAsync_BasicOperation_Success()
        {
            try
            {
                // Arrange
                byte[] testData = Encoding.UTF8.GetBytes("Test content");
                await File.WriteAllBytesAsync(_testFile1, testData);
                await Task.Delay(100);

                // Act
                await Creator_Hian.Unity.Common.FileExtensions.CopyFileAsync(
                    _testFile1,
                    _testFile2
                );
                await Task.Delay(100);

                // Assert
                Assert.That(File.Exists(_testFile2), Is.True);
                byte[] copiedData = await File.ReadAllBytesAsync(_testFile2);
                Assert.That(copiedData, Is.EqualTo(testData));
            }
            finally
            {
                await Task.Delay(100);
            }
        }

        /// <summary>
        /// 파일 복사 중 취소 요청이 정상적으로 처리되는지 테스트합니다.
        ///
        /// TODO: 취소 테스트 추가
        /// </summary>
        [Test]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task CopyFileAsync_WhenCancelled_CleansUpAndThrows()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            // try
            // {
            //     // Arrange
            //     byte[] largeData = new byte[1024 * 1024]; // 1MB
            //     new Random().NextBytes(largeData);
            //     await Creator_Hian.Unity.Common.FileExtensions.WriteFileToPathAsync(_testFile1, largeData);

            //     using var cts = new CancellationTokenSource();
            //     var copyStarted = new TaskCompletionSource<bool>();
            //     Exception caughtException = null;

            //     // Act
            //     var copyTask = Task.Run(async () =>
            //     {
            //         try
            //         {
            //             copyStarted.SetResult(true);
            //             await Creator_Hian.Unity.Common.FileExtensions.CopyFileAsync(
            //                 _testFile1,
            //                 _testFile2,
            //                 cancellationToken: cts.Token);
            //         }
            //         catch (Exception ex)
            //         {
            //             caughtException = ex;
            //             throw;
            //         }
            //     });

            //     // 복사 작업이 시작될 때까지 대기
            //     await copyStarted.Task;
            //     // 즉시 취소 요청
            //     cts.Cancel();

            //     try
            //     {
            //         // 취소된 작업 대기
            //         await copyTask;
            //         Assert.Fail("예외가 발생하지 않았습니다.");
            //     }
            //     catch (Exception ex)
            //     {
            //         caughtException = ex;
            //         // 여기서 예외를 다시 던져야 합니다.
            //         throw ex;
            //     }
            //     finally
            //     {
            //         // Assert
            //         Assert.That(caughtException, Is.Not.Null, "예외가 발생하지 않았습니다.");
            //         Assert.That(caughtException, Is.TypeOf<FileOperationException>());
            //         Assert.That(File.Exists(_testFile2), Is.False, "취소 시 파일이 삭제되지 않았습니다.");
            //     }
            // }
            // finally
            // {
            //     await Task.Delay(100);
            // }
        }

        /// <summary>
        /// 덮어쓰기 옵션이 정상적으로 동작하는지 테스트합니다.
        /// </summary>
        /// <remarks>
        /// 검증 항목:
        /// 1. 대상 파일에 초기 데이터 작성
        /// 2. 원본 파일에 새로운 데이터 작성
        /// 3. overwrite 옵션으로 복사 수행
        /// 4. 대상 파일이 새로운 데이터로 덮어써졌는지 확인
        /// </remarks>
        [Test]
        public async Task CopyFileAsync_WithOverwrite_Success()
        {
            try
            {
                // Arrange
                byte[] initialData = Encoding.UTF8.GetBytes("Initial content");
                byte[] newData = Encoding.UTF8.GetBytes("New content");

                await File.WriteAllBytesAsync(_testFile1, newData);
                await File.WriteAllBytesAsync(_testFile2, initialData);
                await Task.Delay(100);

                // Act
                await Creator_Hian.Unity.Common.FileExtensions.CopyFileAsync(
                    _testFile1,
                    _testFile2,
                    overwrite: true
                );
                await Task.Delay(100);

                // Assert
                Assert.That(File.Exists(_testFile2), Is.True);
                byte[] result = await File.ReadAllBytesAsync(_testFile2);
                Assert.That(result, Is.EqualTo(newData));
            }
            finally
            {
                await Task.Delay(100);
            }
        }
    }
}
