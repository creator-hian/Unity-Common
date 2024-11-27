using NUnit.Framework;
using Creator_Hian.Unity.Common;

/// <summary>
/// FileCategory 클래스의 기본 기능을 테스트합니다.
/// </summary>
// ReSharper disable once CheckNamespace
public class FileCategoryTest
{
    /// <summary>
    /// 등록된 카테고리 이름으로 GetCategory를 호출할 때 Text와 Image 카테고리가 
    /// 각각 Common.Text와 Common.Image로 올바르게 반환되는지 테스트합니다.
    /// </summary>
    [Test]
    public void GetCategory_RegisteredName_ReturnsCorrectCategory()
    {
        // Arrange & Act
        var textCategory = FileCategory.GetCategory("Text");
        var imageCategory = FileCategory.GetCategory("Image");

        // Assert
        Assert.That(textCategory, Is.EqualTo(FileCategory.Common.Text));
        Assert.That(imageCategory, Is.EqualTo(FileCategory.Common.Image));
    }

    /// <summary>
    /// 등록되지 않은 카테고리 이름으로 GetCategory를 호출할 때 
    /// Common.Unknown이 반환되는지 테스트합니다.
    /// </summary>
    [Test]
    public void GetCategory_UnregisteredName_ReturnsUnknown()
    {
        // Arrange & Act
        var category = FileCategory.GetCategory("NonExistentCategory");

        // Assert
        Assert.That(category, Is.EqualTo(FileCategory.Common.Unknown));
    }

    /// <summary>
    /// null, 빈 문자열, 공백 문자열로 GetCategory를 호출할 때 
    /// Common.Unknown이 반환되는지 테스트합니다.
    /// </summary>
    [Test]
    public void GetCategory_NullOrEmptyName_ReturnsUnknown()
    {
        // Act & Assert
        Assert.That(FileCategory.GetCategory(null), Is.EqualTo(FileCategory.Common.Unknown));
        Assert.That(FileCategory.GetCategory(""), Is.EqualTo(FileCategory.Common.Unknown));
        Assert.That(FileCategory.GetCategory(" "), Is.EqualTo(FileCategory.Common.Unknown));
    }

    /// <summary>
    /// Common 네임스페이스의 카테고리들(Unknown, Text, Image)이 
    /// 올바른 이름과 설명을 가지고 있는지 테스트합니다.
    /// </summary>
    [Test]
    public void CommonCategories_HaveCorrectProperties()
    {
        // Arrange & Act
        var unknownCategory = FileCategory.Common.Unknown;
        var textCategory = FileCategory.Common.Text;
        var imageCategory = FileCategory.Common.Image;

        // Assert
        Assert.That(unknownCategory.Name, Is.EqualTo("Unknown"));
        Assert.That(unknownCategory.Description, Is.EqualTo("Unknown File Type"));
        
        Assert.That(textCategory.Name, Is.EqualTo("Text"));
        Assert.That(textCategory.Description, Is.EqualTo("Text File"));
        
        Assert.That(imageCategory.Name, Is.EqualTo("Image"));
        Assert.That(imageCategory.Description, Is.EqualTo("Image File"));
    }

    /// <summary>
    /// Unity 네임스페이스의 카테고리들(Scene, Prefab, Script)이 
    /// 올바른 이름과 설명을 가지고 있는지 테스트합니다.
    /// </summary>
    [Test]
    public void UnityCategories_HaveCorrectProperties()
    {
        // Arrange & Act
        var sceneCategory = FileCategory.Unity.Scene;
        var prefabCategory = FileCategory.Unity.Prefab;
        var scriptCategory = FileCategory.Unity.Script;

        // Assert
        Assert.That(sceneCategory.Name, Is.EqualTo("Scene"));
        Assert.That(sceneCategory.Description, Is.EqualTo("Unity Scene File"));
        
        Assert.That(prefabCategory.Name, Is.EqualTo("Prefab"));
        Assert.That(prefabCategory.Description, Is.EqualTo("Unity Prefab File"));
        
        Assert.That(scriptCategory.Name, Is.EqualTo("Script"));
        Assert.That(scriptCategory.Description, Is.EqualTo("Script File"));
    }

    /// <summary>
    /// ToString 메서드가 카테고리의 Name 속성을 반환하는지 테스트합니다.
    /// </summary>
    [Test]
    public void ToString_ReturnsName()
    {
        // Arrange
        var textCategory = FileCategory.Common.Text;
        var imageCategory = FileCategory.Common.Image;
        var unknownCategory = FileCategory.Common.Unknown;

        // Act & Assert
        Assert.That(textCategory.ToString(), Is.EqualTo("Text"));
        Assert.That(imageCategory.ToString(), Is.EqualTo("Image"));
        Assert.That(unknownCategory.ToString(), Is.EqualTo("Unknown"));
    }
} 