#if UNITY_2023_2_OR_NEWER
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace FileExtensions.Awaitable
{
    /// <summary>
    /// FileExtensions의 Awaitable 파일 쓰기 기능을 테스트합니다.
    /// </summary>
    public class FileExtensionsAwaitableWriteTest : FileExtensionsAwaitableTestBase
    {
        /// <summary>
        /// 유효한 경로에 파일을 Awaitable로 쓰는 기본 동작을 테스트합니다.
        /// </summary>
        [Test]
        public async Task WriteFileToPathAwaitable_ValidPath_CreatesFile()
        {
            // Act
            await Creator_Hian.Unity.Common.FileExtensions.WriteFileToPathAwaitable(
                _testFile1,
                _testData
            );

            // 파일 핸들이 해제될 때까지 잠시 대기
            await Task.Delay(100);

            // Assert
            Assert.That(File.Exists(_testFile1), Is.True);
            byte[] readData;
            using (
                FileStream fs = new FileStream(
                    _testFile1,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite
                )
            )
            {
                readData = new byte[fs.Length];
                _ = await fs.ReadAsync(readData, 0, readData.Length);
            }
            Assert.That(readData, Is.EqualTo(_testData));
        }
    }
}
#endif
