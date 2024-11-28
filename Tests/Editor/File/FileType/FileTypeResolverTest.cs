using System;
using System.IO;
using NUnit.Framework;
using System.Linq;
using Creator_Hian.Unity.Common;
using Creator_Hian.Unity.Common.Tests;

/// <summary>
/// FileTypeResolver의 기본 기능을 테스트합니다.
/// </summary>
// ReSharper disable once CheckNamespace
public class FileTypeResolverTest
{
    private IFileTypeResolver _resolver;

    /// <summary>
    /// 각 테스트 전에 FileTypeResolver 인스턴스를 가져옵니다.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _resolver = FileTypeResolver.Instance;
    }

    /// <summary>
    /// 일반적인 파일 타입에 대한 해석이 올바르게 동작하는지 테스트합니다.
    /// 텍스트(.txt), JSON(.json), 이미지(.png), 압축(.zip) 파일의 카테고리가 올바르게 매핑되는지 확인합니다.
    /// </summary>
    [Test]
    public void GetFileType_CommonTypes_ReturnsCorrectType()
    {
        // Arrange
        var testCases = new[]
        {
            ($"{FileTypeTestConstants.Paths.ValidFileName}{FileTypeTestConstants.Extensions.Text}", 
                FileCategory.Common.Text),
            ($"{FileTypeTestConstants.Paths.ValidFileName}{FileTypeTestConstants.Extensions.Json}", 
                FileCategory.Common.Data),
            ($"{FileTypeTestConstants.Paths.ValidFileName}{FileTypeTestConstants.Extensions.Png}", 
                FileCategory.Common.Image),
            ($"{FileTypeTestConstants.Paths.ValidFileName}{FileTypeTestConstants.Extensions.Zip}", 
                FileCategory.Common.Archive)
        };

        // Act & Assert
        foreach (var (path, expectedCategory) in testCases)
        {
            var fileType = _resolver.GetFileType(path);
            Assert.That(fileType.Category, Is.EqualTo(expectedCategory),
                $"Failed for extension: {Path.GetExtension(path)}");
        }
    }

    /// <summary>
    /// Unity 관련 파일 타입에 대한 해석이 올바르게 동작하는지 테스트합니다.
    /// 씬(.unity), 프리팹(.prefab), 애니메이터(.controller), 애니메이션(.anim) 파일의 카테고리가 올바르게 매핑되는지 확인합니다.
    /// </summary>
    [Test]
    public void GetFileType_UnityTypes_ReturnsCorrectType()
    {
        // Arrange
        var testCases = new[]
        {
            ($"Level1{FileTypeTestConstants.Extensions.Scene}", FileCategory.Unity.Scene),
            ($"Player{FileTypeTestConstants.Extensions.Prefab}", FileCategory.Unity.Prefab),
            ($"Character{FileTypeTestConstants.Extensions.Controller}", FileCategory.Unity.Animation),
            ($"Walk{FileTypeTestConstants.Extensions.Animation}", FileCategory.Unity.Animation)
        };

        // Act & Assert
        foreach (var (path, expectedCategory) in testCases)
        {
            var fileType = _resolver.GetFileType(path);
            Assert.That(fileType.Category, Is.EqualTo(expectedCategory),
                $"Failed for extension: {Path.GetExtension(path)}");
        }
    }

    /// <summary>
    /// Image 카테고리에 속한 모든 파일 타입(png, jpg, gif)을 올바르게 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void GetTypesByCategory_ImageCategory_ReturnsAllImageTypes()
    {
        // Act
        var imageTypes = _resolver.GetTypesByCategory(FileCategory.Common.Image).ToList();

        // Assert
        Assert.That(imageTypes, Has.Count.EqualTo(3));
        Assert.That(imageTypes.Select(t => t.Extension),
            Contains.Item(FileTypeTestConstants.Extensions.Png)
                .And.Contains(FileTypeTestConstants.Extensions.Jpeg)
                .And.Contains(FileTypeTestConstants.Extensions.Gif));
    }

    /// <summary>
    /// Animation 카테고리에 속한 모든 파일 타입(.anim, .controller)을 올바르게 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void GetTypesByCategory_AnimationCategory_ReturnsAllAnimationTypes()
    {
        // Act
        var animTypes = _resolver.GetTypesByCategory(FileCategory.Unity.Animation).ToList();

        // Assert
        Assert.That(animTypes, Has.Count.EqualTo(2));
        Assert.That(animTypes.Select(t => t.Extension),
            Contains.Item(FileTypeTestConstants.Extensions.Animation)
                .And.Contains(FileTypeTestConstants.Extensions.Controller));
    }

    /// <summary>
    /// IsTypeOf 메서드가 파일 경로와 카테고리를 올바르게 비교하는지 테스트합니다.
    /// 이미지, 텍스트, 애니메이터, 씬 파일에 대한 카테고리 매칭을 검증합니다.
    /// </summary>
    [Test]
    public void IsTypeOf_ValidPaths_ReturnsCorrectResult()
    {
        // Arrange
        var testCases = new[]
        {
            ($"test{FileTypeTestConstants.Extensions.Png}", FileCategory.Common.Image, true),
            ($"test{FileTypeTestConstants.Extensions.Text}", FileCategory.Common.Image, false),
            ($"character{FileTypeTestConstants.Extensions.Controller}", FileCategory.Unity.Animation, true),
            ($"level{FileTypeTestConstants.Extensions.Scene}", FileCategory.Unity.Scene, true)
        };

        // Act & Assert
        foreach (var (path, category, expected) in testCases)
        {
            bool result = _resolver.IsTypeOf(path, category);
            Assert.That(result, Is.EqualTo(expected),
                $"Failed for path: {path}, category: {category}");
        }
    }

    /// <summary>
    /// 알 수 없는 확장자(.xyz)를 가진 파일에 대해 Unknown 타입을 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void GetFileType_InvalidPath_ReturnsUnknownType()
    {
        // Arrange
        string invalidPath = $"file{FileTypeTestConstants.Extensions.Unknown}";

        // Act
        var fileType = _resolver.GetFileType(invalidPath);

        // Assert
        Assert.That(fileType.Category, Is.EqualTo(FileCategory.Common.Unknown));
    }

    /// <summary>
    /// null이나 빈 문자열 경로에 대해 ArgumentNullException이 발생하는지 테스트합니다.
    /// </summary>
    [Test]
    public void GetFileType_NullOrEmptyPath_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _resolver.GetFileType(null));
        Assert.Throws<ArgumentNullException>(() => _resolver.GetFileType(string.Empty));
    }

    /// <summary>
    /// 대소문자가 다른 MIME 타입으로 검색 시 올바르게 동작하는지 테스트합니다.
    /// </summary>
    [Test]
    public void GetTypesByMimeType_CaseInsensitive_ReturnsCorrectTypes()
    {
        // Arrange
        var resolver = FileTypeResolver.Instance;
        var type = new FileTypeDefinition(".txt", "Text File", FileCategory.Common.Text, "text/plain");

        // Act
        var upperCaseTypes = resolver.GetTypesByMimeType("TEXT/PLAIN").ToList();
        var lowerCaseTypes = resolver.GetTypesByMimeType("text/plain").ToList();

        // Assert
        Assert.That(upperCaseTypes, Is.EqualTo(lowerCaseTypes));
    }

    /// <summary>
    /// 존재하지 않는 MIME 타입으로 검색 시 빈 컬렉션을 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void GetTypesByMimeType_NonExistentMimeType_ReturnsEmptyCollection()
    {
        // Arrange
        var resolver = FileTypeResolver.Instance;

        // Act
        var types = resolver.GetTypesByMimeType("application/nonexistent").ToList();

        // Assert
        Assert.That(types, Is.Empty);
    }

    /// <summary>
    /// null이나 빈 MIME 타입으로 검색 시 빈 컬렉션을 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void GetTypesByMimeType_NullOrEmptyMimeType_ReturnsEmptyCollection()
    {
        // Arrange
        var resolver = FileTypeResolver.Instance;

        // Act & Assert
        Assert.That(resolver.GetTypesByMimeType(null).ToList(), Is.Empty);
        Assert.That(resolver.GetTypesByMimeType(string.Empty).ToList(), Is.Empty);
        Assert.That(resolver.GetTypesByMimeType("  ").ToList(), Is.Empty);
    }
}