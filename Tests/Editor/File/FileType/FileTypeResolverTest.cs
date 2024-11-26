using System;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using System.Linq;
using Creator_Hian.Unity.Common;

public class FileTypeResolverTest
{
    private IFileTypeResolver _resolver;

    [SetUp]
    public void Setup()
    {
        _resolver = new FileTypeResolver();
    }

    [Test]
    public void GetFileType_CommonTypes_ReturnsCorrectType()
    {
        // Arrange
        var testCases = new[]
        {
            ("test.txt", FileCategory.Common.Text),
            ("data.json", FileCategory.Common.Data),
            ("image.png", FileCategory.Common.Image),
            ("archive.zip", FileCategory.Common.Archive)
        };

        // Act & Assert
        foreach (var (path, expectedCategory) in testCases)
        {
            var fileType = _resolver.GetFileType(path);
            Assert.That(fileType.Category, Is.EqualTo(expectedCategory),
                $"Failed for extension: {Path.GetExtension(path)}");
        }
    }

    [Test]
    public void GetFileType_UnityTypes_ReturnsCorrectType()
    {
        // Arrange
        var testCases = new[]
        {
            ("Level1.unity", FileCategory.Unity.Scene),
            ("Player.prefab", FileCategory.Unity.Prefab),
            ("Character.controller", FileCategory.Unity.Animation),
            ("Walk.anim", FileCategory.Unity.Animation)
        };

        // Act & Assert
        foreach (var (path, expectedCategory) in testCases)
        {
            var fileType = _resolver.GetFileType(path);
            Assert.That(fileType.Category, Is.EqualTo(expectedCategory),
                $"Failed for extension: {Path.GetExtension(path)}");
        }
    }

    [Test]
    public void GetTypesByCategory_ImageCategory_ReturnsAllImageTypes()
    {
        // Act
        var imageTypes = _resolver.GetTypesByCategory(FileCategory.Common.Image).ToList();

        // Assert
        Assert.That(imageTypes, Has.Count.EqualTo(3)); // png, jpg, gif
        Assert.That(imageTypes.Select(t => t.Extension),
            Contains.Item(".png").And.Contains(".jpg").And.Contains(".gif"));
    }

    [Test]
    public void GetTypesByCategory_AnimationCategory_ReturnsAllAnimationTypes()
    {
        // Act
        var animTypes = _resolver.GetTypesByCategory(FileCategory.Unity.Animation).ToList();

        // Assert
        Assert.That(animTypes, Has.Count.EqualTo(2)); // .anim, .controller
        Assert.That(animTypes.Select(t => t.Extension),
            Contains.Item(".anim").And.Contains(".controller"));
    }

    [Test]
    public void IsTypeOf_ValidPaths_ReturnsCorrectResult()
    {
        // Arrange
        var testCases = new[]
        {
            ("test.png", FileCategory.Common.Image, true),
            ("test.txt", FileCategory.Common.Image, false),
            ("character.controller", FileCategory.Unity.Animation, true),
            ("level.unity", FileCategory.Unity.Scene, true)
        };

        // Act & Assert
        foreach (var (path, category, expected) in testCases)
        {
            bool result = _resolver.IsTypeOf(path, category);
            Assert.That(result, Is.EqualTo(expected),
                $"Failed for path: {path}, category: {category}");
        }
    }

    [Test]
    public void GetFileType_InvalidPath_ReturnsUnknownType()
    {
        // Arrange
        string invalidPath = "file.xyz";

        // Act
        var fileType = _resolver.GetFileType(invalidPath);

        // Assert
        Assert.That(fileType.Category, Is.EqualTo(FileCategory.Common.Unknown));
    }

    [Test]
    public void GetFileType_NullOrEmptyPath_ThrowsArgumentNullException()
    {
        // Arrange
        string nullPath = null;
        string emptyPath = "";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _resolver.GetFileType(nullPath));
        Assert.Throws<ArgumentNullException>(() => _resolver.GetFileType(emptyPath));
    }
}