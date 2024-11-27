// ReSharper disable once CheckNamespace
namespace Creator_Hian.Unity.Common
{
    /// <summary>
    /// 파일 타입 정보를 제공하는 인터페이스
    /// </summary>
    public interface IFileTypeInfo
    {
        /// <summary>
        /// 파일의 확장자
        /// </summary>
        string Extension { get; }

        /// <summary>
        /// 파일 종류에 대한 설명
        /// </summary>
        string Description { get; }

        /// <summary>
        /// 파일의 카테고리
        /// </summary>
        FileCategory Category { get; }

        /// <summary>
        /// MIME 타입
        /// </summary>
        string MimeType { get; }
    }
} 