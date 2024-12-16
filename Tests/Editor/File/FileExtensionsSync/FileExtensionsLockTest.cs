using System.IO;
using NUnit.Framework;
using UnityEngine;
using Creator_Hian.Unity.Common;

namespace FileExtensions.Sync
{
    /// <summary>
    /// FileExtensions 클래스의 파일 잠금 기능을 테스트합니다.
    /// </summary>
    public class FileExtensionsLockTest : FileExtensionsTestBase
    {
        /// <summary>
        /// 읽기 전용으로 파일이 열려있을 때 잠금 상태가 false로 반환되는지 테스트합니다.
        /// </summary>
        [Test]
        public void IsFileLocked_WithReadLock_ReturnsFalse()
        {
            // Arrange
            string filePath = Path.Combine(_testDirectoryPath, "readLocked.txt");
            Creator_Hian.Unity.Common.FileExtensions.WriteFileToPath(filePath, _testData);

            using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            // Act & Assert
            Assert.That(Creator_Hian.Unity.Common.FileExtensions.IsFileLocked(filePath), Is.False);
        }

        /// <summary>
        /// 쓰기 잠금이 설정된 파일의 잠금 상태가 true로 반환되는지 테스트합니다.
        /// </summary>
        [Test]
        public void IsFileLocked_WithWriteLock_ReturnsTrue()
        {
            // Arrange
            string filePath = Path.Combine(_testDirectoryPath, "writeLocked.txt");
            Creator_Hian.Unity.Common.FileExtensions.WriteFileToPath(filePath, _testData);

            using var stream = File.Open(filePath, FileMode.Open, FileAccess.Write, FileShare.None);

            // Act & Assert
            Assert.That(Creator_Hian.Unity.Common.FileExtensions.IsFileLocked(filePath), Is.True);
        }
    }
} 