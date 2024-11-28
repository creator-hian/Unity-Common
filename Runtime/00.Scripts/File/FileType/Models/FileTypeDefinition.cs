using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Creator_Hian.Unity.Common
{
    /// <summary>
    /// 파일 타입의 정의를 나타내는 클래스입니다.
    /// </summary>
    public class FileTypeDefinition : IEquatable<FileTypeDefinition>, IFileTypeInfo
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
        /// MIME 타입들
        /// </summary>
        public IReadOnlyCollection<string> MimeTypes { get; }

        /// <summary>
        /// FileTypeDefinition의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="extension">파일 확장자 (예: ".txt")</param>
        /// <param name="description">파일 종류 설명</param>
        /// <param name="category">파일 카테고리</param>
        /// <param name="mimeTypes">MIME 타입들</param>
        public FileTypeDefinition(
            string extension,
            string description,
            FileCategory category,
            params string[] mimeTypes)
        {
            if (string.IsNullOrEmpty(extension))
                throw new ArgumentNullException(nameof(extension));

            Extension = extension.ToLowerInvariant();
            Description = description ?? FileConstants.Descriptions.Unknown;
            Category = category ?? FileCategory.Common.Unknown;
            MimeTypes = mimeTypes?.Select(m => m.ToLowerInvariant()).ToArray() 
                ?? new[] { FileConstants.MimeTypes.Default };
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

        /// <summary>
        /// 지정된 MIME 타입이 파일 타입 정의에 포함되는지 확인합니다.
        /// </summary>
        /// <param name="mimeType">확인할 MIME 타입</param>
        /// <returns>지정된 MIME 타입이 포함되면 true, 그렇지 않으면 false</returns>
        public bool HasMimeType(string mimeType)
        {
            if (string.IsNullOrEmpty(mimeType)) return false;
            mimeType = mimeType.ToLowerInvariant();
            return MimeTypes.Any(m => m.Equals(mimeType, StringComparison.OrdinalIgnoreCase));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FileTypeDefinition)obj);
        }

        public bool Equals(FileTypeDefinition other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Extension, other.Extension, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Description, other.Description) &&
                   Equals(Category, other.Category) &&
                   MimeTypes.Count == other.MimeTypes.Count &&
                   MimeTypes.All(m => other.MimeTypes.Contains(m, StringComparer.OrdinalIgnoreCase));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Extension?.ToLowerInvariant().GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (Description?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Category?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (MimeTypes?.OrderBy(m => m.ToLowerInvariant())
                    .Aggregate(0, (h, m) => h ^ m.GetHashCode()) ?? 0);
                return hashCode;
            }
        }

        public static bool operator ==(FileTypeDefinition left, FileTypeDefinition right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FileTypeDefinition left, FileTypeDefinition right)
        {
            return !Equals(left, right);
        }
    }
} 