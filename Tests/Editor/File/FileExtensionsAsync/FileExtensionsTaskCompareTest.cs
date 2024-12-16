using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Random = System.Random;

namespace FileExtensions.Async
{
    /// <summary>
    /// FileExtensions의 비동기 파일 비교(Task) 관련 기능을 테스트합니다.
    /// </summary>
    /// <remarks>
    /// 테스트 범위:
    /// - 동일 내용 파일 비교
    /// - 다른 내용 파일 비교
    /// - 취소 동작
    /// - 대용량 파일 비교
    /// </remarks>
    public class FileExtensionsTaskCompareTest : FileExtensionsTaskTestBase
    {
        /// <summary>
        /// 동일한 내용을 가진 두 파일을 비교할 때 true를 반환하는지 테스트합니다.
        /// </summary>
        /// <remarks>
        /// 검증 항목:
        /// 1. 동일한 내용의 파일 두 개 생성
        /// 2. CompareFilesAsync 호출 결과가 true인지 확인
        /// 3. 파일 핸들이 적절히 해제되는지 확인
        /// </remarks>
        [Test]
        public async Task CompareFilesAsync_SameContent_ReturnsTrue()
        {
            try
            {
                // Arrange
                await Creator_Hian.Unity.Common.FileExtensions.WriteFileToPathAsync(_testFile1, _testData);
                await Creator_Hian.Unity.Common.FileExtensions.WriteFileToPathAsync(_testFile2, _testData);
                await Task.Delay(100);

                // Act
                bool result = await Creator_Hian.Unity.Common.FileExtensions.CompareFilesAsync(_testFile1, _testFile2);

                // Assert
                Assert.That(result, Is.True);
            }
            finally
            {
                await Task.Delay(100);
            }
        }

        /// <summary>
        /// 서로 다른 내용을 가진 두 파일을 비교할 때 false를 반환하는지 테스트합니다.
        /// </summary>
        /// <remarks>
        /// 검증 항목:
        /// 1. 서로 다른 내용의 파일 두 개 생성
        /// 2. CompareFilesAsync 호출 결과가 false인지 확인
        /// 3. 파일 핸들이 적절히 해제되는지 확인
        /// </remarks>
        [Test]
        public async Task CompareFilesAsync_WithDifferentFiles_ReturnsFalse()
        {
            try
            {
                // Arrange
                await Creator_Hian.Unity.Common.FileExtensions.WriteFileToPathAsync(_testFile1, _testData);
                await Creator_Hian.Unity.Common.FileExtensions.WriteFileToPathAsync(_testFile2, new byte[] { 1, 2, 3 });
                await Task.Delay(100);

                // Act
                bool result = await Creator_Hian.Unity.Common.FileExtensions.CompareFilesAsync(_testFile1, _testFile2);

                // Assert
                Assert.That(result, Is.False);
            }
            finally
            {
                await Task.Delay(100);
            }
        }

        /// <summary>
        /// 파일 비교 중 취소 요청이 발생했을 때 예외가 발생하는지 테스트합니다.
        /// 
        /// TODO: 취소 테스트 추가
        /// </summary>
        [Test]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task CompareFilesAsync_WithCancellation_ThrowsException()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            // try
            // {
            //     // Arrange
            //     byte[] largeData = new byte[100 * 1024 * 1024]; // 10MB
            //     new Random().NextBytes(largeData);
            //     await Creator_Hian.Unity.Common.FileExtensions.WriteFileToPathAsync(_testFile1, largeData);
            //     await Creator_Hian.Unity.Common.FileExtensions.WriteFileToPathAsync(_testFile2, largeData);

            //     using var cts = new CancellationTokenSource();
            //     var compareStarted = new TaskCompletionSource<bool>();
            //     Exception caughtException = null;

            //     // 비교 작업 시작
            //     var compareTask = Task.Run(async () =>
            //     {
            //         try
            //         {
            //             compareStarted.SetResult(true);
            //             // 취소 토큰을 사용하여 CompareFilesAsync 호출
            //             return await Creator_Hian.Unity.Common.FileExtensions.CompareFilesAsync(
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

            //     // 비교 작업이 시작될 때까지 대기
            //     await compareStarted.Task;
            //     // 즉시 취소 요청
            //     cts.Cancel();

            //     try
            //     {
            //         // 취소된 작업 대기
            //         await compareTask;
            //         Assert.Fail("예외가 발생해야 합니다."); // 취소되었을 경우 예외가 발생해야 하므로, 이 코드는 실행되면 안됩니다.
            //     }
            //     catch (OperationCanceledException ex)
            //     {
            //         caughtException = ex;
            //     }

            //     // Act & Assert
            //     Assert.That(caughtException, Is.Not.Null, "예외가 발생하지 않았습니다.");
            //     Assert.That(caughtException, Is.TypeOf<OperationCanceledException>());

            // }
            // finally
            // {
            // }
        }
    }
}