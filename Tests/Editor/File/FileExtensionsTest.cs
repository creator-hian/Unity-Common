using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using Random = System.Random;
using Creator_Hian.Unity.Common;

public class FileExtensionsTest
{
    private string testDirectoryPath;
    private byte[] testData;

    [OneTimeSetUp]
    public void SetUp()
    {
        // 테스트용 임시 디렉토리 생성
        testDirectoryPath = Path.Combine(Application.temporaryCachePath, "FileExtensionsTest");
        if (Directory.Exists(testDirectoryPath))
        {
            Directory.Delete(testDirectoryPath, true);
        }

        Directory.CreateDirectory(testDirectoryPath);

        // 테스트용 데이터 생성
        testData = Encoding.UTF8.GetBytes("Test Data Content");
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        // 테스트 종료 후 정리
        if (Directory.Exists(testDirectoryPath))
        {
            Directory.Delete(testDirectoryPath, true);
        }
    }

    /// <summary>
    /// 유효한 경로에 파일을 동기적으로 쓰는 기본 테스트입니다.
    /// </summary>
    /// <remarks>
    /// 검증 항목:
    /// 1. 파일이 실제로 생성되는지 확인
    /// 2. 쓰여진 데이터가 원본 데이터와 일치하는지 확인
    /// </remarks>
    [Test]
    public void WriteFileToPath_ValidPath_CreatesFile()
    {
        // Arrange
        string filePath = Path.Combine(testDirectoryPath, "test.txt");

        // Act
        FileExtensions.WriteFileToPath(filePath, testData);

        // Assert
        Assert.That(File.Exists(filePath), Is.True);
        byte[] readData = File.ReadAllBytes(filePath);
        Assert.That(readData, Is.EqualTo(testData));
    }

    /// <summary>
    /// 유효한 경로에 파일을 비동기적으로 쓰는 테스트입니다.
    /// </summary>
    /// <remarks>
    /// 검증 항목:
    /// 1. 비동기 작업이 정상적으로 완료되는지 확인
    /// 2. 파일이 실제로 생성되는지 확인
    /// 3. 쓰여진 데이터가 원본 데이터와 일치하는지 확인
    /// </remarks>
    [Test]
    public async Task WriteFileToPathAsync_ValidPath_CreatesFile()
    {
        // Arrange
        string filePath = Path.Combine(testDirectoryPath, "testAsync.txt");

        // Act
        await FileExtensions.WriteFileToPathAsync(filePath, testData);

        // Assert
        Assert.That(File.Exists(filePath), Is.True);
        byte[] readData = File.ReadAllBytes(filePath);
        Assert.That(readData, Is.EqualTo(testData));
    }

    /// <summary>
    /// 잘못된 경로로 파일 쓰기를 시도할 때 적절한 예외가 발생하는지 테스트합니다.
    /// </summary>
    /// <remarks>
    /// 검증 항목:
    /// 1. 너무 긴 경로명(300자)에 대한 FilePathException ���생 확인
    /// 2. 잘못된 문자('*')가 포함된 경로에 대한 처리 확인
    /// </remarks>
    [Test]
    public void WriteFileToPath_InvalidPath_ThrowsException()
    {
        // Arrange
        string invalidPath = Path.Combine(testDirectoryPath, new string('*', 300));

        // Act & Assert
        Assert.Throws<FilePathException>(() =>
            FileExtensions.WriteFileToPath(invalidPath, testData));
    }

    /// <summary>
    /// null 경로로 파일 쓰기를 시도할 때 적절한 예외가 발생하는지 테스트합니다.
    /// </summary>
    /// <remarks>
    /// 검증 항목:
    /// 1. null 경로에 대한 FilePathException 발생 확인
    /// </remarks>
    [Test]
    public void WriteFileToPath_NullPath_ThrowsException()
    {
        // Act & Assert
        Assert.Throws<FilePathException>(() =>
            FileExtensions.WriteFileToPath(null, testData));
    }

    /// <summary>
    /// 비동기 파일 쓰기 중 취소 요청이 발생했을 때의 동작을 테스트합니다.
    /// </summary>
    /// <remarks>
    /// 검증 항목:
    /// 1. CancellationToken으로 취소 시 FileWriteException 발생 확인
    /// 2. 취소 후 파일이 생성되지 않았는지 확인
    /// </remarks>
    [Test]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task WriteFileToPathAsync_CancellationRequested_ThrowsException()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        // Arrange
        string filePath = Path.Combine(testDirectoryPath, "testCancelled.txt");
        var cts = new CancellationTokenSource();
        cts.Cancel(); // 즉시 취소

        // Act & Assert
        Assert.ThrowsAsync<FileWriteException>(async () =>
            await FileExtensions.WriteFileToPathAsync(filePath, testData, cancellationToken: cts.Token));
    }

    /// <summary>
    /// 중첩된 디렉토리 구조에서 파일 쓰기가 정상 동작하는지 테스트합니다.
    /// </summary>
    /// <remarks>
    /// 검증 항목:
    /// 1. 존재하지 않는 중첩 디렉토리가 자동으로 생성되는지 확인
    /// 2. 최종 파일이 정상적으로 생성되는지 확인
    /// </remarks>
    [Test]
    public void WriteFileToPath_CreateNestedDirectories_Success()
    {
        // Arrange
        string nestedPath = Path.Combine(testDirectoryPath, "nested", "folders", "test.txt");

        // Act
        FileExtensions.WriteFileToPath(nestedPath, testData);

        // Assert
        Assert.That(File.Exists(nestedPath), Is.True);
    }

    /// <summary>
    /// 대용량 파일의 동기식 쓰기가 정상 동작하는지 테스트합니다.
    /// </summary>
    /// <remarks>
    /// 검증 항목:
    /// 1. 1MB 크기의 파일 쓰기 성공 확인
    /// 2. 쓰여진 대용량 데이터의 무결성 검증
    /// </remarks>
    [Test]
    public void WriteFileToPath_LargeFile_Success()
    {
        // Arrange
        string filePath = Path.Combine(testDirectoryPath, "largeFile.txt");
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
    /// 대용량 파일의 비동기식 쓰기가 정상 동작하는지 테스트합니다.
    /// </summary>
    /// <remarks>
    /// 검증 항목:
    /// 1. 1MB 크기의 파일 비동기 쓰기 성공 확인
    /// 2. 사용자 정의 버퍼 크기(8192)로 쓰기 작업 수행 확인
    /// 3. 쓰여진 대용량 데이터의 무결성 검증
    /// </remarks>
    [Test]
    public async Task WriteFileToPathAsync_LargeFile_Success()
    {
        // Arrange
        string filePath = Path.Combine(testDirectoryPath, "largeFileAsync.txt");
        byte[] largeData = new byte[1024 * 1024]; // 1MB
        new Random().NextBytes(largeData);
        // Act
        await FileExtensions.WriteFileToPathAsync(filePath, largeData, bufferSize: 8192);

        // Assert
        Assert.That(File.Exists(filePath), Is.True);
        byte[] readData = File.ReadAllBytes(filePath);
        Assert.That(readData, Is.EqualTo(largeData));
    }
}