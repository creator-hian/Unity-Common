using NUnit.Framework;
using Creator_Hian.Unity.Common;
using Creator_Hian.Unity.Common.Tests;
using System;

/// <summary>
/// FileTypeDefinition 클래스의 생성과 동작을 테스트합니다.
/// </summary>
// ReSharper disable once CheckNamespace
public class FileTypeDefinitionTest
{
    /// <summary>
    /// 유효한 매개변수로 FileTypeDefinition을 생성할 때 모든 속성이 올바르게 설정되는지 테스트합니다.
    /// Extension, Description, Category, MimeType이 입력값과 일치하는지 확인합니다.
    /// </summary>
    [Test]
    public void Constructor_ValidParameters_CreatesInstance()
    {
        // Arrange
        string extension = FileTypeTestConstants.Extensions.Text;
        string description = FileTypeTestConstants.Descriptions.Text;
        FileCategory category = FileCategory.Common.Text;
        string mimeType = FileTypeTestConstants.MimeTypes.TextPlain;

        // Act
        var definition = new FileTypeDefinition(extension, description, category, mimeType);

        // Assert
        Assert.That(definition.Extension, Is.EqualTo(extension));
        Assert.That(definition.Description, Is.EqualTo(description));
        Assert.That(definition.Category, Is.EqualTo(category));
        Assert.That(definition.MimeType, Is.EqualTo(mimeType));
    }

    /// <summary>
    /// 확장자가 null일 때 ArgumentNullException이 발생하는지 테스트합니다.
    /// </summary>
    [Test]
    public void Constructor_NullExtension_ThrowsArgumentNullException()
    {
        // Arrange
        string description = FileTypeTestConstants.Descriptions.Text;
        FileCategory category = FileCategory.Common.Text;

        // Act & Assert
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentNullException>(() => { new FileTypeDefinition(null, description, category); });
    }

    /// <summary>
    /// 카테고리가 null일 때 Unknown 카테고리가 사용되는지 테스트합니다.
    /// </summary>
    [Test]
    public void Constructor_NullCategory_UsesUnknownCategory()
    {
        // Arrange
        string extension = FileTypeTestConstants.Extensions.Text;
        string description = FileTypeTestConstants.Descriptions.Text;

        // Act
        var definition = new FileTypeDefinition(extension, description, null);

        // Assert
        Assert.That(definition.Category, Is.EqualTo(FileCategory.Common.Unknown));
    }

    /// <summary>
    /// 설명이 null일 때 "Unknown"이 사용되는지 테스트합니다.
    /// </summary>
    [Test]
    public void Constructor_NullDescription_UsesUnknown()
    {
        // Arrange
        string extension = FileTypeTestConstants.Extensions.Text;
        FileCategory category = FileCategory.Common.Text;

        // Act
        var definition = new FileTypeDefinition(extension, null, category);

        // Assert
        Assert.That(definition.Description, Is.EqualTo(FileTypeTestConstants.Descriptions.Unknown));
    }

    /// <summary>
    /// MIME 타입이 null일 때 기본 MIME 타입이 사용되는지 테스트합니다.
    /// </summary>
    [Test]
    public void Constructor_NullMimeType_UsesDefaultMimeType()
    {
        // Arrange
        string extension = FileTypeTestConstants.Extensions.Text;
        string description = FileTypeTestConstants.Descriptions.Text;
        FileCategory category = FileCategory.Common.Text;

        // Act
        var definition = new FileTypeDefinition(extension, description, category);

        // Assert
        Assert.That(definition.MimeType, Is.EqualTo(FileTypeTestConstants.MimeTypes.Default));
    }

    /// <summary>
    /// 확장자가 항상 소문자로 저장되는지 테스트합니다.
    /// </summary>
    [Test]
    public void Extension_AlwaysLowerCase()
    {
        // Arrange & Act
        var definition = new FileTypeDefinition(
            FileTypeTestConstants.Extensions.Text.ToUpper(), 
            FileTypeTestConstants.Descriptions.Text, 
            FileCategory.Common.Text);

        // Assert
        Assert.That(definition.Extension, Is.EqualTo(FileTypeTestConstants.Extensions.Text));
    }
} 