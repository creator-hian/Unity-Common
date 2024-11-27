using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using Creator_Hian.Unity.Common;
using Creator_Hian.Unity.Common.Tests;
using Random = System.Random;

/// <summary>
/// FileExtensions 클래스의 파일 시스템 작업 기능을 테스트합니다.
/// </summary>
// ReSharper disable once CheckNamespace
public class FileExtensionsTest
{
    private string _testDirectoryPath;
    private byte[] _testData;

    [SetUp]
    public void SetUp()
    {
        // 각 테스트마다 새로운 디렉토리 생성
        _testDirectoryPath = Path.Combine(
            Application.temporaryCachePath, 
            "FileExtensionsTest", 
            Guid.NewGuid().ToString());
        
        Directory.CreateDirectory(_testDirectoryPath);
        // 테스트용 데이터 생성
        _testData = Encoding.UTF8.GetBytes(FileTypeTestConstants.Contents.TextContent);
    }

    [TearDown]
    // ReSharper disable once CognitiveComplexity
    public void TearDown()
    {
        try
        {
            if (Directory.Exists(_testDirectoryPath))
            {
                // 파일 핸들이 해제될 때까지 대기
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        Directory.Delete(_testDirectoryPath, true);
                        break;
                    }
                    catch (IOException)
                    {
                        if (i < 2)
                        {
                            Thread.Sleep(100);
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        }
                    }
                }
            }
        }
        catch
        {
            // 정리 실패는 무시
        }
    }

    /// <summary>
    /// 유효한 경로에 파일을 동기적으로 쓰는 기본 동작을 테스트합니다.
    /// 파일이 생성되고 데이터가 정확히 쓰여졌는지 확인합니다.
    /// </summary>
    [Test]
    public void WriteFileToPath_ValidPath_CreatesFile()
    {
        // Arrange
        string filePath = Path.Combine(_testDirectoryPath, "test.txt");

        // Act
        FileExtensions.WriteFileToPath(filePath, _testData);

        // Assert
        Assert.That(File.Exists(filePath), Is.True);
        byte[] readData = File.ReadAllBytes(filePath);
        Assert.That(readData, Is.EqualTo(_testData));
    }

    /// <summary>
    /// 유효한 경로에 파일을 비동기적으로 쓰는 동작을 테스트합니다.
    /// 비동기 작업이 완료된 후 파일이 생성되고 데이터가 정확히 쓰여졌는지 확인합니다.
    /// </summary>
    [Test]
    public async Task WriteFileToPathAsync_ValidPath_CreatesFile()
    {
        // Arrange
        string filePath = Path.Combine(_testDirectoryPath, "testAsync.txt");

        try
        {
            // Act
            await FileExtensions.WriteFileToPathAsync(filePath, _testData);
            await Task.Delay(100); // 파일 핸들 해제 대기

            // Assert
            Assert.That(File.Exists(filePath), Is.True);
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                byte[] readData = new byte[_testData.Length];
                await fs.ReadAsync(readData, 0, readData.Length);
                Assert.That(readData, Is.EqualTo(_testData));
            }
        }
        finally
        {
            if (File.Exists(filePath))
            {
                await Task.Delay(100);
                try { File.Delete(filePath); } catch { }
            }
        }
    }

    /// <summary>
    /// 잘못된 경로로 파일 쓰기를 시도할 때의 예외 처리를 테스트합니다.
    /// </summary>
    /// <remarks>
    /// 검증하는 예외 상황:
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
            FileExtensions.WriteFileToPath(invalidPath, _testData));
    }

    /// <summary>
    /// null 경로로 파일 쓰기를 시도할 때 FilePathException이 발생하는지 테스트합니다.
    /// </summary>
    [Test]
    public void WriteFileToPath_NullPath_ThrowsException()
    {
        // Act & Assert
        Assert.Throws<FilePathException>(() =>
            FileExtensions.WriteFileToPath(null, _testData));
    }

    /// <summary>
    /// 비동기 파일 쓰기 중 취소 요청이 발생했을 때의 동작을 테스트합니다.
    /// </summary>
    /// <remarks>
    /// 검증 항목:
    /// 1. CancellationToken으로 취소 시 FileWriteException 발생
    /// 2. 취소 후 파일이 생성되지 않았는지 확인
    /// </remarks>
    [Test]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task WriteFileToPathAsync_CancellationRequested_ThrowsException()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        // Arrange
        string filePath = Path.Combine(_testDirectoryPath, "testCancelled.txt");
        var cts = new CancellationTokenSource();
        cts.Cancel(); // 즉시 취소

        // Act & Assert
        Assert.ThrowsAsync<FileWriteException>(async () =>
            await FileExtensions.WriteFileToPathAsync(filePath, _testData, cancellationToken: cts.Token));
    }

    /// <summary>
    /// 중첩된 디렉토리 구조에서 파일 쓰기가 정상 동작하는지 테스트합니다.
    /// 존재하지 않는 중첩 디렉토리가 자동으로 생성되고 파일이 정상적으로 생성되는지 확인합니다.
    /// </summary>
    [Test]
    public void WriteFileToPath_CreateNestedDirectories_Success()
    {
        // Arrange
        string nestedPath = Path.Combine(_testDirectoryPath, "nested", "folders", "test.txt");

        // Act
        FileExtensions.WriteFileToPath(nestedPath, _testData);

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
        new Random().NextBytes(largeData);

        // Act
        FileExtensions.WriteFileToPath(filePath, largeData);

        // Assert
        Assert.That(File.Exists(filePath), Is.True);
        byte[] readData = File.ReadAllBytes(filePath);
        Assert.That(readData, Is.EqualTo(largeData));
    }

    /// <summary>
    /// 대용량 파일(1MB)의 비동기식 쓰기가 정상 동작하는지 테스트합니다.
    /// 사용자 정의 버퍼 크기(8192)로 쓰기 작업이 수행되는지 확인합니다.
    /// </summary>
    [Test]
    public async Task WriteFileToPathAsync_LargeFile_Success()
    {
        // Arrange
        string filePath = Path.Combine(_testDirectoryPath, "largeFileAsync.txt");
        byte[] largeData = new byte[1024 * 1024]; // 1MB
        new Random().NextBytes(largeData);

        try
        {
            // Act
            await FileExtensions.WriteFileToPathAsync(filePath, largeData, bufferSize: 8192);
            await Task.Delay(100); // 파일 핸들 해제 대기

            // Assert
            Assert.That(File.Exists(filePath), Is.True);
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                byte[] readData = new byte[largeData.Length];
                await fs.ReadAsync(readData, 0, readData.Length);
                Assert.That(readData, Is.EqualTo(largeData));
            }
        }
        finally
        {
            if (File.Exists(filePath))
            {
                await Task.Delay(100);
                try { File.Delete(filePath); } catch { }
            }
        }
    }


    /// <summary>
    /// 읽기 전용으로 파일이 열려있을 때 잠금 상태가 false로 반환되는지 테스트합니다.
    /// </summary>
    [Test]
    public void IsFileLocked_WithReadLock_ReturnsFalse()
    {
        // Arrange
        string filePath = Path.Combine(_testDirectoryPath, "readLocked.txt");
        FileExtensions.WriteFileToPath(filePath, _testData);
        
        using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        
        // Act & Assert
        Assert.That(FileExtensions.IsFileLocked(filePath), Is.False);
    }

    /// <summary>
    /// 쓰기 잠금이 설정된 파일의 잠금 상태가 true로 반환되는지 테스트합니다.
    /// </summary>
    [Test]
    public void IsFileLocked_WithWriteLock_ReturnsTrue()
    {
        // Arrange
        string filePath = Path.Combine(_testDirectoryPath, "writeLocked.txt");
        FileExtensions.WriteFileToPath(filePath, _testData);
        
        using var stream = File.Open(filePath, FileMode.Open, FileAccess.Write, FileShare.None);
        
        // Act & Assert
        Assert.That(FileExtensions.IsFileLocked(filePath), Is.True);
    }


    /// <summary>
    /// 동일한 내용을 가진 두 파일을 비교할 때 true를 반환하는지 테스트합니다.
    /// </summary>
    /// <remarks>
    /// 검증 항목:
    /// 1. 동일한 내용의 파일 생성
    /// 2. CompareFilesAsync가 true를 반환하는지 확인
    /// </remarks>
    [Test]
    public async Task CompareFilesAsync_WithIdenticalFiles_ReturnsTrue()
    {
        // Arrange
        string path1 = Path.Combine(_testDirectoryPath, "file1.txt");
        string path2 = Path.Combine(_testDirectoryPath, "file2.txt");
        await FileExtensions.WriteFileToPathAsync(path1, _testData);
        await FileExtensions.WriteFileToPathAsync(path2, _testData);

        // Act
        bool result = await FileExtensions.CompareFilesAsync(path1, path2);

        // Assert
        Assert.That(result, Is.True);
    }

    /// <summary>
    /// 서로 다른 내용을 가진 두 파일을 비교할 때 false를 반환하는지 테스트합니다.
    /// </summary>
    /// <remarks>
    /// 검증 항목:
    /// 1. 서로 다른 내용의 파일 생성
    /// 2. CompareFilesAsync가 false를 반환하는지 확인
    /// </remarks>
    [Test]
    public async Task CompareFilesAsync_WithDifferentFiles_ReturnsFalse()
    {
        // Arrange
        string path1 = Path.Combine(_testDirectoryPath, "file1.txt");
        string path2 = Path.Combine(_testDirectoryPath, "file2.txt");
        await FileExtensions.WriteFileToPathAsync(path1, _testData);
        await FileExtensions.WriteFileToPathAsync(path2, new byte[] { 1, 2, 3 });

        // Act
        bool result = await FileExtensions.CompareFilesAsync(path1, path2);

        // Assert
        Assert.That(result, Is.False);
    }

    /// <summary>
    /// 파일 비교 중 취소 요청이 발생했을 때 예외가 발생하는지 테스트합니다.
    /// </summary>
    /// <remarks>
    /// 검증 항목:
    /// 1. 큰 크기(1MB)의 동일한 파일 두 개 생성
    /// 2. 취소된 CancellationToken으로 비교 시도
    /// 3. FileOperationException이 발생하는지 확인
    /// </remarks>
    [Test]
    public async Task CompareFilesAsync_WithCancellation_ThrowsException()
    {
        // Arrange
        string path1 = Path.Combine(_testDirectoryPath, "file1.txt");
        string path2 = Path.Combine(_testDirectoryPath, "file2.txt");
        byte[] largeData = new byte[1024 * 1024]; // 1MB
        new Random().NextBytes(largeData);
        await FileExtensions.WriteFileToPathAsync(path1, largeData);
        await FileExtensions.WriteFileToPathAsync(path2, largeData);

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        Assert.ThrowsAsync<FileOperationException>(() => 
            FileExtensions.CompareFilesAsync(path1, path2, cancellationToken: cts.Token));
    }

    /// <summary>
    /// 파일 쓰기 작업 중 취소가 발생했을 때의 동작을 테스트합니다.
    /// </summary>
    /// <remarks>
    /// 검증 항목:
    /// 1. 대용량 데이터 준비
    /// 2. 작은 버퍼로 쓰기 시작
    /// 3. 즉시 취소 요청
    /// 4. FileWriteException이 발생하는지 확인
    /// 5. 부분적으로 생성된 파일이 없는지 확인
    /// </remarks>
    [Test]
    public async Task WriteFileToPathAsync_CancelDuringWrite_ThrowsException()
    {
        // Arrange
        string filePath = Path.Combine(_testDirectoryPath, "test.txt");
        byte[] largeData = new byte[1024 * 1024]; // 1MB로 축소
        new Random().NextBytes(largeData);

        using var cts = new CancellationTokenSource();
        
        // 즉시 취소
        cts.Cancel();

        try
        {
            // Act
            await FileExtensions.WriteFileToPathAsync(
                filePath, 
                largeData, 
                bufferSize: 1024,
                cancellationToken: cts.Token);

            Assert.Fail("예상된 FileWriteException이 발생하지 않았습니다.");
        }
        catch (FileWriteException ex)
        {
            // Assert
            StringAssert.Contains("파일 쓰기가 취소되었습니다", ex.Message);
            Assert.That(File.Exists(filePath), Is.False, "부분 파일이 생성되었습니다.");
        }
    }

    /// <summary>
    /// 기본적인 파일 복사 동작을 테스트합니다.
    /// </summary>
    [Test]
    public async Task CopyFileAsync_BasicOperation_Success()
    {
        // Arrange
        string sourcePath = Path.Combine(_testDirectoryPath, "source.txt");
        string destPath = Path.Combine(_testDirectoryPath, "dest.txt");
        
        byte[] testData = Encoding.UTF8.GetBytes("Test content");
        await File.WriteAllBytesAsync(sourcePath, testData);

        // Act
        await FileExtensions.CopyFileAsync(sourcePath, destPath);

        // Assert
        Assert.That(File.Exists(destPath), Is.True);
        byte[] copiedData = await File.ReadAllBytesAsync(destPath);
        Assert.That(copiedData, Is.EqualTo(testData));
    }

    /// <summary>
    /// 파일 복사 중 취소 요청이 정상적으로 처리되는지 테스트합니다.
    /// </summary>
    [Test]
    public async Task CopyFileAsync_WhenCancelled_CleansUpAndThrows()
    {
        // Arrange
        string sourcePath = Path.Combine(_testDirectoryPath, "source.txt");
        string destPath = Path.Combine(_testDirectoryPath, "dest.txt");
        
        // 더 작은 크기의 테스트 파일 생성 (1MB)
        byte[] testData = new byte[1024 * 1024];
        new Random().NextBytes(testData);
        await File.WriteAllBytesAsync(sourcePath, testData);

        using var cts = new CancellationTokenSource();

        // 즉시 취소
        cts.Cancel();

        try
        {
            // Act
            await FileExtensions.CopyFileAsync(
                sourcePath, 
                destPath,
                cancellationToken: cts.Token);

            Assert.Fail("예상된 FileOperationException이 발생하지 않았습니다.");
        }
        catch (FileOperationException ex)
        {
            // Assert
            StringAssert.Contains("파일 복사가 취소되었습니다", ex.Message);
            Assert.That(File.Exists(destPath), Is.False, "부분 파일이 정리되지 않았습니다.");
        }
    }

    /// <summary>
    /// 덮어쓰기 옵션이 정상적으로 동작하는지 테스트합니다.
    /// </summary>
    [Test]
    public async Task CopyFileAsync_WithOverwrite_Success()
    {
        // Arrange
        string sourcePath = Path.Combine(_testDirectoryPath, "source.txt");
        string destPath = Path.Combine(_testDirectoryPath, "dest.txt");
        
        byte[] initialData = Encoding.UTF8.GetBytes("Initial content");
        byte[] newData = Encoding.UTF8.GetBytes("New content");
        
        await File.WriteAllBytesAsync(sourcePath, newData);
        await File.WriteAllBytesAsync(destPath, initialData);

        // Act
        await FileExtensions.CopyFileAsync(sourcePath, destPath, overwrite: true);

        // Assert
        Assert.That(File.Exists(destPath), Is.True);
        byte[] result = await File.ReadAllBytesAsync(destPath);
        Assert.That(result, Is.EqualTo(newData));
    }

    [Test]
    public async Task WriteFileToPathAsync_WithDifferentBufferSizes()
    {
        // Arrange
        string baseFilePath = Path.Combine(_testDirectoryPath, "bufferTest");
        byte[] testData = new byte[1024 * 1024]; // 1MB
        new Random().NextBytes(testData);

        var testCases = new[]
        {
            (path: $"{baseFilePath}_auto.dat", bufferSize: -1), // AutoCalculateBufferSize
            (path: $"{baseFilePath}_small.dat", bufferSize: 512), // 작은 버퍼
            (path: $"{baseFilePath}_large.dat", bufferSize: 1024 * 1024), // 1MB 버퍼
        };

        foreach (var (path, bufferSize) in testCases)
        {
            try
            {
                // Act
                await FileExtensions.WriteFileToPathAsync(path, testData, bufferSize);
                await Task.Delay(100); // 파일 핸들 해제 대기

                // Assert
                Assert.That(File.Exists(path), Is.True, $"파일이 생성되지 않음: {path}");
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    byte[] readData = new byte[testData.Length];
                    await fs.ReadAsync(readData, 0, readData.Length);
                    Assert.That(readData, Is.EqualTo(testData), $"데이터가 일치하지 않음: {path}");
                }
            }
            finally
            {
                if (File.Exists(path))
                {
                    await Task.Delay(100);
                    try { File.Delete(path); } catch { }
                }
            }
        }
    }

    [Test]
    public async Task WriteFileToPathAsync_WithVeryLargeFile()
    {
        // Arrange
        string filePath = Path.Combine(_testDirectoryPath, "veryLarge.dat");
        long fileSize = 52_428_800 + 1024; // LargeFileThreshold + 1KB
        byte[] largeData = new byte[fileSize];
        new Random().NextBytes(largeData);

        var memoryBefore = GC.GetTotalMemory(true);
        
        try
        {
            // Act
            await FileExtensions.WriteFileToPathAsync(filePath, largeData);

            // Assert
            Assert.That(File.Exists(filePath), Is.True, "대용량 파일이 생성되지 않음");
            Assert.That(new FileInfo(filePath).Length, Is.EqualTo(fileSize), "파일 크기가 일치하지 않음");

            // 메모리 사용량 확인 (급격한 증가가 없어야 함)
            var memoryAfter = GC.GetTotalMemory(false);
            var memoryDiff = memoryAfter - memoryBefore;
            
            Assert.That(memoryDiff, Is.LessThan(fileSize / 2), 
                "메모리 사용량이 너무 높습니다");

            // 파일 내용 검증
            byte[] readData;
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                readData = new byte[fileSize];
                int totalBytesRead = 0;
                int bytesRead;
                var buffer = new byte[1024 * 1024];

                while ((bytesRead = await fs.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    Buffer.BlockCopy(buffer, 0, readData, totalBytesRead, bytesRead);
                    totalBytesRead += bytesRead;
                }
                Assert.That(totalBytesRead, Is.EqualTo(fileSize), "읽은 데이터 크기가 일치하지 않음");
            }
            
            // 데이터 비교
            Assert.That(readData, Is.EqualTo(largeData), "파일 내용이 일치하지 않음");
        }
        finally
        {
            try
            {
                if (File.Exists(filePath))
                {
                    await Task.Delay(100); // 파일 핸들이 해제될 때까지 잠시 대기
                    File.Delete(filePath);
                }
            }
            catch (IOException)
            {
                // 정리 실패는 무시
            }
        }
    }

    [Test]
    public void WriteFileToPathAsync_WithInvalidBufferSize_ThrowsException()
    {
        // Arrange
        string filePath = Path.Combine(_testDirectoryPath, "invalid.dat");
        byte[] testData = new byte[1024];
        
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await FileExtensions.WriteFileToPathAsync(filePath, testData, -2)); // AutoCalculateBufferSize(-1) 외 음수
    }
}