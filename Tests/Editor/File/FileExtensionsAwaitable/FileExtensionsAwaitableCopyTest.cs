#if UNITY_2023_2_OR_NEWER
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace FileExtensions.Awaitable
{
    /// <summary>
    /// FileExtensions의 Awaitable 파일 복사 기능을 테스트합니다.
    /// </summary>
    public class FileExtensionsAwaitableCopyTest : FileExtensionsAwaitableTestBase
    {
        /// <summary>
        /// 파일 복사의 기본 동작을 테스트합니다.
        /// </summary>
        [Test]
        public async Task CopyFileAwaitable_ValidPaths_CopiesFile()
        {
            // Arrange
            await Creator_Hian.Unity.Common.FileExtensions.WriteFileToPathAwaitable(
                _testFile1,
                _testData
            );
            await Task.Delay(100); // 파일 핸들이 해제될 때까지 대기

            // Act
            await Creator_Hian.Unity.Common.FileExtensions.CopyFileAwaitable(
                _testFile1,
                _testFile2
            );
            await Task.Delay(100); // 파일 핸들이 해제될 때까지 대기

            // Assert
            Assert.That(File.Exists(_testFile2), Is.True);

            // 파일 내용 비교를 위해 안전한 방식으로 읽기
            byte[] sourceData;
            byte[] destData;

            using (
                FileStream fs1 = new FileStream(
                    _testFile1,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite
                )
            )
            {
                sourceData = new byte[fs1.Length];
                _ = await fs1.ReadAsync(sourceData, 0, sourceData.Length);
            }

            using (
                FileStream fs2 = new FileStream(
                    _testFile2,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite
                )
            )
            {
                destData = new byte[fs2.Length];
                _ = await fs2.ReadAsync(destData, 0, destData.Length);
            }

            Assert.That(destData, Is.EqualTo(sourceData));
        }
    }
}
#endif
