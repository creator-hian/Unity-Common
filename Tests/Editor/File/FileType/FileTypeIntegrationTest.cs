using System.IO;
using NUnit.Framework;
using UnityEngine;
using Creator_Hian.Unity.Common;
using Creator_Hian.Unity.Common.Tests;

/// <summary>
/// FileTypeResolver의 실제 파일 시스템과의 통합 동작을 테스트합니다.
/// </summary>
// ReSharper disable once CheckNamespace
namespace FileExtensions.FileType
{
    public class FileTypeIntegrationTest
    {
        private IFileTypeResolver _resolver;
        private string _testDirectory;

        /// <summary>
        /// 테스트에 필요한 임시 디렉토리를 생성하고 리졸버를 초기화합니다.
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            _resolver = FileTypeResolver.Instance;
            _testDirectory = Path.Combine(Application.temporaryCachePath, "FileTypeTests");
            Directory.CreateDirectory(_testDirectory);
        }

        /// <summary>
        /// 테스트 완료 후 임시 디렉토리를 정리합니다.
        /// </summary>
        [OneTimeTearDown]
        public void TearDown()
        {
            if (Directory.Exists(_testDirectory))
            {
                Directory.Delete(_testDirectory, true);
            }
        }

        /// <summary>
        /// 실제 파일 생성과 타입 확인이 정상적으로 동작하는지 테스트합니다.
        /// 텍스트 파일과 JSON 파일을 생성하고, 파일 존재 여부, 카테고리 매칭, IsTypeOf 메서드 동작을 검증합니다.
        /// </summary>
        [Test]
        public void FileOperations_WithTypeChecking_Success()
        {
            // Arrange
            var testFiles = new[]
            {
                ($"test{FileTypeTestConstants.Extensions.Text}", 
                    FileTypeTestConstants.Contents.TextContent, 
                    FileCategory.Common.Text),
                ($"data{FileTypeTestConstants.Extensions.Json}", 
                    FileTypeTestConstants.Contents.JsonContent, 
                    FileCategory.Common.Data)
            };

            foreach (var (fileName, content, expectedCategory) in testFiles)
            {
                string filePath = Path.Combine(_testDirectory, fileName);
                
                // Act
                File.WriteAllText(filePath, content);
                var fileType = _resolver.GetFileType(filePath);
                bool isCorrectType = _resolver.IsTypeOf(filePath, expectedCategory);

                // Assert
                Assert.That(File.Exists(filePath), "파일이 생성되지 않았습니다");
                Assert.That(fileType.Category, Is.EqualTo(expectedCategory), 
                    "파일 카테고리가 일치하지 않습니다");
                Assert.That(isCorrectType, Is.True, 
                    "IsTypeOf 검사 결과가 올바르지 않습니다");
            }
        }

        /// <summary>
        /// 다양한 경로 형식에 대한 파일 타입 해석을 테스트합니다.
        /// 중첩 경로, 상대 경로, 절대 경로에서 Unity 관련 파일들의 확장자가 올바르게 추출되는지 확인합니다.
        /// </summary>
        [Test]
        public void FileTypeResolution_WithMixedPaths_HandlesCorrectly()
        {
            // Arrange
            var testPaths = new[]
            {
                Path.Combine(_testDirectory, FileTypeTestConstants.Paths.NestedPath + FileTypeTestConstants.Extensions.Scene),
                Path.Combine(_testDirectory, "test" + FileTypeTestConstants.Extensions.Prefab),
                FileTypeTestConstants.Paths.RelativePath + FileTypeTestConstants.Extensions.Material,
                FileTypeTestConstants.Paths.AbsolutePath + FileTypeTestConstants.Extensions.Animation
            };

            // Act & Assert
            foreach (var path in testPaths)
            {
                var fileType = _resolver.GetFileType(path);
                Assert.That(fileType, Is.Not.Null);
                Assert.That(fileType.Extension, Is.EqualTo(Path.GetExtension(path)));
            }
        }
    }
} 