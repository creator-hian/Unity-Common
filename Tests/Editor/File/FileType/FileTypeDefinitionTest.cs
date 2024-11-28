using NUnit.Framework;
using Creator_Hian.Unity.Common;
using Creator_Hian.Unity.Common.Tests;
using System;
using System.Linq;

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
        string[] mimeTypes = { FileConstants.MimeTypes.Text.Plain };

        // Act
        var definition = new FileTypeDefinition(extension, description, category, mimeTypes);

        // Assert
        Assert.That(definition.Extension, Is.EqualTo(extension));
        Assert.That(definition.Description, Is.EqualTo(description));
        Assert.That(definition.Category, Is.EqualTo(category));
        Assert.That(definition.MimeTypes, Is.EquivalentTo(mimeTypes));
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
        Assert.Throws<ArgumentNullException>(() => 
            new FileTypeDefinition(null, description, category));
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
        Assert.That(definition.Description, Is.EqualTo(FileConstants.Descriptions.Unknown));
    }

    /// <summary>
    /// MIME 타입이 null일 때 기본 MIME 타입이 사용되는지 테스트합니다.
    /// </summary>
    [Test]
    public void Constructor_NullMimeTypes_UsesDefaultMimeType()
    {
        // Arrange
        string extension = FileTypeTestConstants.Extensions.Text;
        string description = FileTypeTestConstants.Descriptions.Text;
        FileCategory category = FileCategory.Common.Text;

        // Act
        var definition = new FileTypeDefinition(extension, description, category);

        // Assert
        Assert.That(definition.MimeTypes, Is.EquivalentTo(new[] { FileConstants.MimeTypes.Default }));
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

    /// <summary>
    /// MIME 타입의 대소문자 구분 없는 비교를 테스트합니다.
    /// </summary>
    [Test]
    public void MimeTypes_CaseInsensitiveComparison()
    {
        // Arrange
        var type1 = new FileTypeDefinition(".txt", "Text File", FileCategory.Common.Text, "TEXT/PLAIN");
        var type2 = new FileTypeDefinition(".txt", "Text File", FileCategory.Common.Text, "text/plain");

        // Assert
        Assert.That(string.Equals(
            type1.MimeTypes.First(), 
            type2.MimeTypes.First(), 
            StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// 동일한 속성을 가진 FileTypeDefinition 인스턴스의 동등성을 테스트합니다.
    /// </summary>
    [Test]
    public void FileTypeDefinition_Equality()
    {
        // Arrange
        var type1 = new FileTypeDefinition(".txt", "Text File", FileCategory.Common.Text, "text/plain");
        var type2 = new FileTypeDefinition(".txt", "Text File", FileCategory.Common.Text, "text/plain");

        // Assert
        Assert.That(type1, Is.EqualTo(type2));
        Assert.That(type1.GetHashCode(), Is.EqualTo(type2.GetHashCode()));
    }

    /// <summary>
    /// 여러 MIME 타입을 가진 파일 타입의 동등성을 테스트합니다.
    /// </summary>
    [Test]
    public void FileTypeDefinition_MultiMimeTypeEquality()
    {
        // Arrange
        var type1 = new FileTypeDefinition(".xml", "XML File", FileCategory.Common.Data, 
            "application/xml", "text/xml");
        var type2 = new FileTypeDefinition(".xml", "XML File", FileCategory.Common.Data, 
            "text/xml", "application/xml");  // 순서가 다름

        // Assert
        Assert.That(type1, Is.EqualTo(type2));
        Assert.That(type1.GetHashCode(), Is.EqualTo(type2.GetHashCode()));
    }

    /// <summary>
    /// HasMimeType 메서드가 올바르게 동작하는지 테스트합니다.
    /// </summary>
    [Test]
    public void HasMimeType_ChecksCorrectly()
    {
        // Arrange
        var type = new FileTypeDefinition(".xml", "XML File", FileCategory.Common.Data,
            "application/xml", "text/xml");

        // Assert - 각각의 케이스를 개별적으로 테스트
        Assert.That(type.HasMimeType("application/xml"), Is.True);
        Assert.That(type.HasMimeType("text/xml"), Is.True);
        Assert.That(type.HasMimeType("APPLICATION/XML"), Is.True);  // 대소문자 구분 없음
        Assert.That(type.HasMimeType("application/json"), Is.False);
        Assert.That(type.HasMimeType(null), Is.False);
        Assert.That(type.HasMimeType(""), Is.False);
    }

    /// <summary>
    /// 빈 MIME 타입 배열이 전달될 때의 동작을 테스트합니다.
    /// </summary>
    [Test]
    public void Constructor_EmptyMimeTypes_UsesDefaultMimeType()
    {
        // Arrange & Act
        var definition = new FileTypeDefinition(".txt", "Text File", FileCategory.Common.Text, Array.Empty<string>());

        // Assert
        Assert.That(definition.MimeTypes.ToArray(), Has.Length.EqualTo(1));
        Assert.That(definition.MimeTypes.First(), Is.EqualTo(FileConstants.MimeTypes.Default));
    }

    /// <summary>
    /// null이나 빈 문자열이 포함된 MIME 타입 배열이 전달될 때의 동작을 테스트합니다.
    /// </summary>
    [Test]
    public void Constructor_NullOrEmptyMimeTypeInArray_FiltersInvalidValues()
    {
        // Arrange
        string[] mimeTypes = { "text/plain", null, "", "  ", "application/json" };

        // Act
        var definition = new FileTypeDefinition(".txt", "Text File", FileCategory.Common.Text, mimeTypes);

        // Assert
        var resultArray = definition.MimeTypes.ToArray();
        Assert.That(resultArray, Has.Length.EqualTo(2));
        CollectionAssert.Contains(resultArray, "text/plain");
        CollectionAssert.Contains(resultArray, "application/json");
    }

    /// <summary>
    /// 동일한 MIME 타입이 중복 전달될 때의 동작을 테스트합니다.
    /// </summary>
    [Test]
    public void Constructor_DuplicateMimeTypes_RemovesDuplicates()
    {
        // Arrange
        string[] mimeTypes = { "text/plain", "TEXT/PLAIN", "text/plain" };

        // Act
        var definition = new FileTypeDefinition(".txt", "Text File", FileCategory.Common.Text, mimeTypes);

        // Assert
        var resultArray = definition.MimeTypes.ToArray();
        Assert.That(resultArray, Has.Length.EqualTo(1));
        Assert.That(resultArray[0], Is.EqualTo("text/plain"));
    }
} 