using System.IO;
using NUnit.Framework;
using UnityEngine;
using Creator_Hian.Unity.Common;

namespace FileExtensions.Sync
{
    /// <summary>
    /// FileExtensions 클래스의 동기 파일 복사 기능을 테스트합니다.
    /// </summary>
    public class FileExtensionsCopyTest : FileExtensionsTestBase
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
        /// </remarks>
        [Test]
        public void CopyFile_BasicOperation_Success()
        {
            // Arrange
            string sourcePath = Path.Combine(_testDirectoryPath, "source.txt");
            string destPath = Path.Combine(_testDirectoryPath, "dest.txt");
            Creator_Hian.Unity.Common.FileExtensions.WriteFileToPath(sourcePath, _testData);

            // Act
            Creator_Hian.Unity.Common.FileExtensions.CopyFile(sourcePath, destPath);

            // Assert
            Assert.That(File.Exists(destPath), Is.True);
            byte[] copiedData = File.ReadAllBytes(destPath);
            Assert.That(copiedData, Is.EqualTo(_testData));
        }

        /// <summary>
        /// 덮어쓰기 옵션이 정상적으로 동작하는지 테스트합니다.
        /// </summary>
        [Test]
        public void CopyFile_WithOverwrite_Success()
        {
            // Arrange
            string sourcePath = Path.Combine(_testDirectoryPath, "source.txt");
            string destPath = Path.Combine(_testDirectoryPath, "dest.txt");
            Creator_Hian.Unity.Common.FileExtensions.WriteFileToPath(sourcePath, _testData);
            Creator_Hian.Unity.Common.FileExtensions.WriteFileToPath(destPath, new byte[] { 5, 6, 7 });

            // Act
            Creator_Hian.Unity.Common.FileExtensions.CopyFile(sourcePath, destPath, overwrite: true);

            // Assert
            Assert.That(File.Exists(destPath), Is.True);
            byte[] result = File.ReadAllBytes(destPath);
            Assert.That(result, Is.EqualTo(_testData));
        }

        /// <summary>
        /// 존재하지 않는 원본 파일 경로로 CopyFile을 호출했을 때 예외가 발생하는지 테스트합니다.
        /// </summary>
        [Test]
        public void CopyFile_WithNonExistentSource_ThrowsException()
        {
            // Arrange
            string sourcePath = Path.Combine(_testDirectoryPath, "nonexistent.txt");
            string destPath = Path.Combine(_testDirectoryPath, "dest.txt");

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() =>
                Creator_Hian.Unity.Common.FileExtensions.CopyFile(sourcePath, destPath));
        }

        /// <summary>
        /// 대상 파일이 이미 존재하고 덮어쓰기 옵션이 false일 때 예외가 발생하는지 테스트합니다.
        /// </summary>
        [Test]
        public void CopyFile_WithExistingDestinationAndNoOverwrite_ThrowsException()
        {
            // Arrange
            string sourcePath = Path.Combine(_testDirectoryPath, "source.txt");
            string destPath = Path.Combine(_testDirectoryPath, "dest.txt");
            Creator_Hian.Unity.Common.FileExtensions.WriteFileToPath(sourcePath, _testData);
            Creator_Hian.Unity.Common.FileExtensions.WriteFileToPath(destPath, _testData);

            // Act & Assert
            Assert.Throws<FileOperationException>(() =>
                Creator_Hian.Unity.Common.FileExtensions.CopyFile(sourcePath, destPath, overwrite: false));
        }
    }
} 