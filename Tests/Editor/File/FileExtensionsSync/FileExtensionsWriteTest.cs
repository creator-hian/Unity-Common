using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using Creator_Hian.Unity.Common;
using System.Threading;

namespace FileExtensions.Sync
{
    /// <summary>
    /// FileExtensions 클래스의 동기 파일 쓰기 기능을 테스트합니다.
    /// </summary>
    public class FileExtensionsWriteTest : FileExtensionsTestBase
    {
        /// <summary>
        /// 유효한 경로에 파일을 동기적으로 쓰는 기본 동작을 테스트합니다.
        /// </summary>
        /// <remarks>
        /// 검증 항목:
        /// 1. 파일이 생성되는지 확인
        /// 2. 데이터가 정확히 쓰여졌는지 확인
        /// </remarks>
        [Test]
        public void WriteFileToPath_ValidPath_CreatesFile()
        {
            // Arrange
            string filePath = Path.Combine(_testDirectoryPath, "test.txt");

            // Act
            Creator_Hian.Unity.Common.FileExtensions.WriteFileToPath(filePath, _testData);

            // Assert
            Assert.That(File.Exists(filePath), Is.True);
            byte[] readData = File.ReadAllBytes(filePath);
            Assert.That(readData, Is.EqualTo(_testData));
        }

        /// <summary>
        /// 잘못된 경로로 파일 쓰기를 시도할 때의 예외 처리를 테스트합니다.
        /// </summary>
        /// <remarks>
        /// 검증 항목:
        /// 1. 너무 긴 경로명(300자)에 대한 FilePathException 발생
        /// 2. 잘못된 문자('*')가 포함된 경로에 대한 처리
        /// </remarks>
        [Test]
        public void WriteFileToPath_InvalidPath_ThrowsException()
        {
            // Arrange
            string invalidPath = Path.Combine(_testDirectoryPath, new string('*', 300));

            // Act & Assert
            Assert.Throws<FilePathException>(() =>
                Creator_Hian.Unity.Common.FileExtensions.WriteFileToPath(invalidPath, _testData));
        }

        /// <summary>
        /// null 경로로 파일 쓰기를 시도할 때 FilePathException이 발생하는지 테스트합니다.
        /// </summary>
        [Test]
        public void WriteFileToPath_NullPath_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<FilePathException>(() =>
                Creator_Hian.Unity.Common.FileExtensions.WriteFileToPath(null, _testData));
        }

        /// <summary>
        /// 중첩된 디렉토리 구조에서 파일 쓰기가 정상 동작하는�� 테스트합니다.
        /// </summary>
        /// <remarks>
        /// 검증 항목:
        /// 1. 존재하지 않는 중첩 디렉토리 자동 생성
        /// 2. 파일이 정상적으로 생성되는지 확인
        /// </remarks>
        [Test]
        public void WriteFileToPath_CreateNestedDirectories_Success()
        {
            // Arrange
            string nestedPath = Path.Combine(_testDirectoryPath, "nested", "folders", "test.txt");

            // Act
            Creator_Hian.Unity.Common.FileExtensions.WriteFileToPath(nestedPath, _testData);

            // Assert
            Assert.That(File.Exists(nestedPath), Is.True);
        }

        /// <summary>
        /// 대용량 파일(1MB)의 동기식 쓰기가 정상 동작하는지 테스트합니다.
        /// </summary>
        [Test]
        public void WriteFileToPath_LargeFile_Success()
        {
            // Arrange
            string filePath = Path.Combine(_testDirectoryPath, "largeFile.txt");
            byte[] largeData = new byte[1024 * 1024]; // 1MB
            new System.Random().NextBytes(largeData);

            // Act
            Creator_Hian.Unity.Common.FileExtensions.WriteFileToPath(filePath, largeData);

            // Assert
            Assert.That(File.Exists(filePath), Is.True);
            byte[] readData = File.ReadAllBytes(filePath);
            Assert.That(readData, Is.EqualTo(largeData));
        }

    }
}