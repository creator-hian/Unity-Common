using NUnit.Framework;
using Creator_Hian.Unity.Common;
using Creator_Hian.Unity.Common.Tests;

/// <summary>
/// FileTypeResolver의 엣지 케이스 처리를 테스트합니다.
/// </summary>
// ReSharper disable once CheckNamespace
public class FileTypeEdgeCaseTest
{
    private IFileTypeResolver _resolver;

    [SetUp]
    public void Setup()
    {
        _resolver = new FileTypeResolver();
    }

    /// <summary>
    /// 특수 문자가 포함된 파일 경로에 대한 처리를 테스트합니다.
    /// 한글, 공백, 특수문자(#), 대시(-), 언더스코어(_)가 포함된 파일명을 처리할 수 있는지 확인합니다.
    /// </summary>
    [Test]
    public void GetFileType_SpecialCharacters_HandlesCorrectly()
    {
        // Act & Assert
        foreach (var path in FileTypeTestConstants.Paths.SpecialCharacterPaths)
        {
            Assert.DoesNotThrow(() =>
            {
                var fileType = _resolver.GetFileType(path);
                Assert.That(fileType, Is.Not.Null);
            });
        }
    }

    /// <summary>
    /// 다중 확장자를 가진 파일명에 대한 처리를 테스트합니다.
    /// .tar.gz, .cs.meta, .zip.backup와 같은 다중 확장자에서 마지막 확장자만 사용되는지 확인합니다.
    /// </summary>
    [Test]
    public void GetFileType_MultipleExtensions_UsesLastExtension()
    {
        // Act & Assert
        foreach (var path in FileTypeTestConstants.Paths.MultipleExtensions)
        {
            var fileType = _resolver.GetFileType(path);
            Assert.That(fileType.Extension, Is.EqualTo(
                System.IO.Path.GetExtension(path)));
        }
    }

    /// <summary>
    /// 파일 확장자의 대소문자 구분 없는 처리를 테스트합니다.
    /// .TXT, .Txt, .txt와 같이 다양한 대소문자 조합의 확장자가 동일하게 처리되는지 확인합니다.
    /// </summary>
    [Test]
    public void GetFileType_CaseSensitivity_HandlesCorrectly()
    {
        // Act & Assert
        foreach (var path in FileTypeTestConstants.Paths.CaseSensitiveExtensions)
        {
            var fileType = _resolver.GetFileType(path);
            Assert.That(fileType.Category, Is.Not.EqualTo(FileCategory.Common.Unknown));
        }
    }
} 