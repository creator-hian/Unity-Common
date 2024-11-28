using NUnit.Framework;
using Creator_Hian.Unity.Common;
using Creator_Hian.Unity.Common.Tests;

// ReSharper disable once CheckNamespace
/// <summary>
/// 사용자 정의 파일 타입의 등록과 해석을 테스트합니다.
/// </summary>
public class CustomFileTypesTest
{
    private IFileTypeResolver _resolver;

    /// <summary>
    /// 테스트에 필요한 커스텀 타입을 등록하고 FileTypeResolver를 초기화합니다.
    /// </summary>
    [OneTimeSetUp]
    public void Setup()
    {
        // 커스텀 타입 등록
        CustomFileTypes.Register();
        _resolver = FileTypeResolver.Instance;
    }

    /// <summary>
    /// 커스텀 파일 타입이 올바르게 해석되는지 테스트합니다.
    /// </summary>
    /// <remarks>
    /// 검증 항목:
    /// 1. Markdown 파일(.md)이 Document 카테고리로 해석되는지 확인
    /// 2. Python 파일(.py)이 Script 카테고리로 해석되는지 확인
    /// </remarks>
    [Test]
    public void GetFileType_CustomTypes_ReturnsCorrectType()
    {
        // Arrange & Act
        var markdownType = _resolver.GetFileType("test.md");
        var pythonType = _resolver.GetFileType("script.py");

        // Assert
        Assert.That(markdownType.Category, Is.EqualTo(CustomCategories.Document));
        Assert.That(pythonType.Category, Is.EqualTo(CustomCategories.Script));
    }
} 