using NUnit.Framework;
using Creator_Hian.Unity.Common;
using System;
using System.Linq;

/// <summary>
/// FileTypeRegistry 클래스의 기능을 테스트합니다.
/// </summary>
namespace FileExtensions.FileType
{
    public class FileTypeRegistryTest
    {
        /// <summary>
        /// 각 테스트 실행 전에 레지스트리를 초기화합니다.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            #pragma warning disable CS0618
            FileTypeRegistry.ClearRegistryForTesting();
            #pragma warning restore CS0618
        }

        /// <summary>
        /// 유효한 파일 타입을 등록하고 검색할 수 있는지 테스트합니다.
        /// </summary>
        /// <remarks>
        /// 검증 항목:
        /// 1. 파일 타입 등록 성공
        /// 2. 등록된 파일 타입 검색 가능
        /// 3. 검색된 파일 타입이 등록한 것과 동일
        /// </remarks>
        [Test]
        public void RegisterFileType_ValidType_Succeeds()
        {
            // Arrange
            var fileType = new FileTypeDefinition(".test", "Test File", FileCategory.Common.Text);

            // Act
            FileTypeRegistry.RegisterType(fileType);
            var resolvedType = FileTypeRegistry.GetByExtension(".test");

            // Assert
            Assert.That(resolvedType, Is.EqualTo(fileType));
        }

        /// <summary>
        /// null 파일 타입 등록 시 ArgumentNullException이 발생하는지 테스트합니다.
        /// </summary>
        [Test]
        public void RegisterFileType_NullType_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => FileTypeRegistry.RegisterType(null));
        }

        /// <summary>
        /// 등록되지 않은 확장자로 검색 시 Unknown 타입을 반환하는지 테스트합니다.
        /// </summary>
        [Test]
        public void GetFileType_UnregisteredExtension_ReturnsUnknownType()
        {
            // Act
            var fileType = FileTypeRegistry.GetByExtension(".unknown");

            // Assert
            Assert.That(fileType.Category, Is.EqualTo(FileCategory.Common.Unknown));
        }

        /// <summary>
        /// 카테고리별 파일 타입 검색이 올바르게 동작하는지 테스트합니다.
        /// </summary>
        /// <remarks>
        /// 검증 항목:
        /// 1. 동일 카테고리의 여러 파일 타입 등록
        /// 2. 등록된 모든 파일 타입 검색 가능
        /// 3. 검색된 파일 타입 수가 정확함
        /// 4. 검색된 파일 타입들이 원본과 일치
        /// </remarks>
        [Test]
        public void GetFileTypesByCategory_ReturnsCorrectTypes()
        {
            // Arrange
            #pragma warning disable CS0618
            FileTypeRegistry.ClearRegistryForTesting();
            #pragma warning restore CS0618
            var type1 = new FileTypeDefinition(".txt", "Text File", FileCategory.Common.Text);
            var type2 = new FileTypeDefinition(".doc", "Document File", FileCategory.Common.Text);
            FileTypeRegistry.RegisterType(type1);
            FileTypeRegistry.RegisterType(type2);

            // Act
            var textTypes = FileTypeRegistry.GetTypesByCategory(FileCategory.Common.Text).ToList();

            // Assert
            Assert.That(textTypes, Has.Count.EqualTo(2));
            Assert.That(textTypes, Contains.Item(type1));
            Assert.That(textTypes, Contains.Item(type2));
        }

        /// <summary>
        /// MIME 타입별 파일 타입 검색이 올바르게 동작하는지 테스트합니다.
        /// </summary>
        /// <remarks>
        /// 검증 항목:
        /// 1. 동일 MIME 타입의 여러 파일 타입 등록
        /// 2. 등록된 모든 파일 타입 검색 가능
        /// 3. 검색된 파일 타입 수가 정확함
        /// 4. 검색된 파일 타입들이 원본과 일치
        /// </remarks>
        [Test]
        public void GetFileTypesByMimeType_ReturnsCorrectTypes()
        {
            // Arrange
            #pragma warning disable CS0618
            FileTypeRegistry.ClearRegistryForTesting();
            #pragma warning restore CS0618
            var type1 = new FileTypeDefinition(".txt", "Text File", FileCategory.Common.Text, "text/plain");
            var type2 = new FileTypeDefinition(".log", "Log File", FileCategory.Common.Text, "text/plain");
            FileTypeRegistry.RegisterType(type1);
            FileTypeRegistry.RegisterType(type2);

            // Act
            var textTypes = FileTypeRegistry.GetTypesByMimeType("text/plain").ToList();

            // Assert
            Assert.That(textTypes, Has.Count.EqualTo(2));
            Assert.That(textTypes, Contains.Item(type1));
            Assert.That(textTypes, Contains.Item(type2));
        }

        /// <summary>
        /// 동일한 파일 타입을 중복 등록할 때의 동작을 테스트합니다.
        /// </summary>
        [Test]
        public void RegisterType_DuplicateType_UpdatesExisting()
        {
            // Arrange
            var type1 = new FileTypeDefinition(".txt", "Text File", FileCategory.Common.Text);
            var type2 = new FileTypeDefinition(".txt", "Updated Text File", FileCategory.Common.Text);

            // Act
            FileTypeRegistry.RegisterType(type1);
            FileTypeRegistry.RegisterType(type2);
            var resolvedType = FileTypeRegistry.GetByExtension(".txt");

            // Assert
            Assert.That(resolvedType, Is.EqualTo(type2));
        }

        /// <summary>
        /// 대소문자가 다른 확장자로 등록된 파일 타입을 검색할 때의 동작을 테스트합니다.
        /// </summary>
        [Test]
        public void GetByExtension_CaseInsensitive_ReturnsCorrectType()
        {
            // Arrange
            var fileType = new FileTypeDefinition(".TXT", "Text File", FileCategory.Common.Text);
            FileTypeRegistry.RegisterType(fileType);

            // Act
            var resolvedType1 = FileTypeRegistry.GetByExtension(".txt");
            var resolvedType2 = FileTypeRegistry.GetByExtension(".TXT");
            var resolvedType3 = FileTypeRegistry.GetByExtension(".Txt");

            // Assert
            Assert.That(resolvedType1, Is.EqualTo(fileType));
            Assert.That(resolvedType2, Is.EqualTo(fileType));
            Assert.That(resolvedType3, Is.EqualTo(fileType));
        }
    }
} 