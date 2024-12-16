#if UNITY_2023_2_OR_NEWER
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using Creator_Hian.Unity.Common;

namespace FileExtensions.Awaitable
{
    /// <summary>
    /// FileExtensions의 Awaitable 파일 비교 기능을 테스트합니다.
    /// </summary>
    public class FileExtensionsAwaitableCompareTest : FileExtensionsAwaitableTestBase
    {
        /// <summary>
        /// 동일한 내용을 가진 두 파일을 비교할 때 true를 반환하는지 테스트합니다.
        /// </summary>
        [Test]
        public async Task CompareFilesAwaitable_SameContent_ReturnsTrue()
        {
            // Arrange
            await Creator_Hian.Unity.Common.FileExtensions.WriteFileToPathAwaitable(_testFile1, _testData);
            await Creator_Hian.Unity.Common.FileExtensions.WriteFileToPathAwaitable(_testFile2, _testData);

            // Act
            bool result = await Creator_Hian.Unity.Common.FileExtensions.CompareFilesAwaitable(_testFile1, _testFile2);

            // Assert
            Assert.That(result, Is.True);
        }
    }
}
#endif 