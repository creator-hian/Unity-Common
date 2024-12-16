using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using Creator_Hian.Unity.Common;

namespace FileExtensions.Sync
{
    /// <summary>
    /// FileExtensions 클래스의 동기 파일 비교 기능을 테스트합니다.
    /// </summary>
    public class FileExtensionsCompareTest : FileExtensionsTestBase
    {
        /// <summary>
        /// 동일한 내용을 가진 두 파일을 비교할 때 true를 반환하는지 테스트합니다.
        /// </summary>
        /// <remarks>
        /// 검증 항목:
        /// 1. 동일한 내용의 파일 생성
        /// 2. CompareFiles가 true를 반환하는지 확인
        /// </remarks>
        [Test]
        public void CompareFiles_WithIdenticalFiles_ReturnsTrue()
        {
            // Arrange
            string path1 = Path.Combine(_testDirectoryPath, "file1.txt");
            string path2 = Path.Combine(_testDirectoryPath, "file2.txt");
            Creator_Hian.Unity.Common.FileExtensions.WriteFileToPath(path1, _testData);
            Creator_Hian.Unity.Common.FileExtensions.WriteFileToPath(path2, _testData);

            // Act
            bool result = Creator_Hian.Unity.Common.FileExtensions.CompareFiles(path1, path2);

            // Assert
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// 서로 다른 내용을 가진 두 파일을 비교할 때 false를 반환하는지 테스트합니다.
        /// </summary>
        [Test]
        public void CompareFiles_WithDifferentFiles_ReturnsFalse()
        {
            // Arrange
            string path1 = Path.Combine(_testDirectoryPath, "file1.txt");
            string path2 = Path.Combine(_testDirectoryPath, "file2.txt");
            Creator_Hian.Unity.Common.FileExtensions.WriteFileToPath(path1, _testData);
            Creator_Hian.Unity.Common.FileExtensions.WriteFileToPath(path2, new byte[] { 1, 2, 3 });

            // Act
            bool result = Creator_Hian.Unity.Common.FileExtensions.CompareFiles(path1, path2);

            // Assert
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// 파일 비교 시 내용 비교 옵션을 끌 경우, 내용이 다르더라도 true를 반환하는지 테스트합니다.
        /// </summary>
        [Test]
        public void CompareFiles_WithoutContentComparison_ReturnsTrueForDifferentFiles()
        {
            // Arrange
            string path1 = Path.Combine(_testDirectoryPath, "file1.txt");
            string path2 = Path.Combine(_testDirectoryPath, "file2.txt");
            Creator_Hian.Unity.Common.FileExtensions.WriteFileToPath(path1, _testData);
            Creator_Hian.Unity.Common.FileExtensions.WriteFileToPath(path2, new byte[] { 1, 2, 3 });

            // Act
            bool result = Creator_Hian.Unity.Common.FileExtensions.CompareFiles(path1, path2, compareContent: false);

            // Assert
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// 존재하지 않는 파일 경로로 CompareFiles를 호출했을 때 예외가 발생하는지 테스트합니다.
        /// </summary>
        [Test]
        public void CompareFiles_WithNonExistentFile_ThrowsException()
        {
            // Arrange
            string path1 = Path.Combine(_testDirectoryPath, "file1.txt");
            string path2 = Path.Combine(_testDirectoryPath, "nonexistent.txt");
            Creator_Hian.Unity.Common.FileExtensions.WriteFileToPath(path1, _testData);

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() =>
                Creator_Hian.Unity.Common.FileExtensions.CompareFiles(path1, path2));
        }
    }
} 