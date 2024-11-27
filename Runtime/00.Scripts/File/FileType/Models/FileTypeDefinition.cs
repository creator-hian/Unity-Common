using System;

// ReSharper disable once CheckNamespace
namespace Creator_Hian.Unity.Common
{
    /// <summary>
    /// 파일 타입의 정의를 나타내는 클래스입니다.
    /// </summary>
    public class FileTypeDefinition : IFileTypeInfo
    {
        /// <summary>
        /// 파일의 확장자 (예: ".txt")
        /// </summary>
        public string Extension { get; }

        /// <summary>
        /// 파일 종류에 대한 설명 (예: "Text File")
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// 파일의 카테고리
        /// </summary>
        public FileCategory Category { get; }

        /// <summary>
        /// MIME 타입 (예: "text/plain")
        /// </summary>
        public string MimeType { get; }

        /// <summary>
        /// FileTypeDefinition의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="extension">파일 확장자 (예: ".txt")</param>
        /// <param name="description">파일 종류 설명</param>
        /// <param name="category">파일 카테고리</param>
        /// <param name="mimeType">MIME 타입 (기본값: "application/octet-stream")</param>
        public FileTypeDefinition(
            string extension,
            string description,
            FileCategory category,
            string mimeType = "application/octet-stream")
        {
            Extension = extension?.ToLowerInvariant() 
                ?? throw new ArgumentNullException(nameof(extension));
            Description = description ?? "Unknown";
            Category = category ?? FileCategory.Common.Unknown;
            MimeType = mimeType ?? "application/octet-stream";
        }

        /// <summary>
        /// 현재 파일 타입 정의의 문자열 표현을 반환합니다.
        /// </summary>
        /// <returns>파일 타입의 설명</returns>
        public override string ToString() => Description;

        /// <summary>
        /// 현재 파일 타입 정의가 지정된 카테고리에 속하는지 확인합니다.
        /// </summary>
        /// <param name="category">확인할 카테고리</param>
        /// <returns>지정된 카테고리에 속하면 true, 그렇지 않으면 false</returns>
        public bool IsCategory(FileCategory category) => Category == category;
    }
} 