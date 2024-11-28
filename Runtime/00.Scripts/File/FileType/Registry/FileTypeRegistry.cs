using System;
using System.Collections.Generic;
using System.Linq;


// ReSharper disable once CheckNamespace
namespace Creator_Hian.Unity.Common
{
    /// <summary>
    /// 파일 타입 정의를 관리하는 정적 레지스트리입니다.
    /// </summary>
    public static partial class FileTypeRegistry
    {
        private static readonly Dictionary<string, FileTypeDefinition> TypesByExtension =
            new(StringComparer.OrdinalIgnoreCase);

        private static readonly Dictionary<FileCategory, HashSet<FileTypeDefinition>> TypesByCategory =
            new();

        private static readonly Dictionary<string, HashSet<FileTypeDefinition>> TypesByMimeType =
            new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 정적 생성자에서 기본 파일 타입들을 등록합니다.
        /// </summary>
        static FileTypeRegistry()
        {
            RegisterBuiltInTypes();
        }

        /// <summary>
        /// 기본 제공되는 파일 타입들을 등록합니다.
        /// </summary>
        private static void RegisterBuiltInTypes()
        {
            // Common Types
            RegisterType(FileTypes.Common.Text);
            RegisterType(FileTypes.Common.Json);
            RegisterType(FileTypes.Common.Xml);
            RegisterType(FileTypes.Common.Csv);
            RegisterType(FileTypes.Common.Html);
            RegisterType(FileTypes.Common.Jpeg);
            RegisterType(FileTypes.Common.Png);
            RegisterType(FileTypes.Common.Gif);
            RegisterType(FileTypes.Common.Pdf);
            RegisterType(FileTypes.Common.Zip);

            // Unity Types
            RegisterType(FileTypes.Unity.Scene);
            RegisterType(FileTypes.Unity.Prefab);
            RegisterType(FileTypes.Unity.Asset);
            RegisterType(FileTypes.Unity.Meta);
            RegisterType(FileTypes.Unity.Material);
            RegisterType(FileTypes.Unity.Shader);
        }
        // ReSharper disable once MemberCanBePrivate.Global
        /// <summary>
        /// 새로운 파일 타입 정의를 레지스트리에 등록합니다.
        /// </summary>
        /// <param name="definition">등록할 파일 타입 정의</param>
        /// <exception cref="ArgumentNullException">definition이 null인 경우</exception>
        public static void RegisterType(FileTypeDefinition definition)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            TypesByExtension[definition.Extension] = definition;

            if (!TypesByCategory.TryGetValue(definition.Category, out var categoryTypes))
            {
                categoryTypes = new HashSet<FileTypeDefinition>();
                TypesByCategory[definition.Category] = categoryTypes;
            }

            categoryTypes.Add(definition);

            foreach (var mimeType in definition.MimeTypes)
            {
                if (!TypesByMimeType.TryGetValue(mimeType, out var mimeTypes))
                {
                    mimeTypes = new HashSet<FileTypeDefinition>();
                    TypesByMimeType[mimeType] = mimeTypes;
                }
                mimeTypes.Add(definition);
            }
        }
        // ReSharper disable once MemberCanBePrivate.Global
        /// <summary>
        /// 지정된 확장자에 해당하는 파일 타입 정의를 반환합니다.
        /// </summary>
        /// <param name="extension">찾을 파일 확장자</param>
        /// <returns>파일 타입 정의, 없으면 Unknown 타입</returns>
        public static FileTypeDefinition GetByExtension(string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return CreateUnknownType(extension);

            extension = extension.ToLowerInvariant();
            return TypesByExtension.TryGetValue(extension, out var type)
                ? type
                : CreateUnknownType(extension);
        }

        /// <summary>
        /// 지정된 카테고리에 속하는 모든 파일 타입 정의를 반환합니다.
        /// </summary>
        /// <param name="category">찾을 파일 카테고리</param>
        /// <returns>파일 타입 정의 컬렉션</returns>
        public static IReadOnlyCollection<FileTypeDefinition> GetByCategory(FileCategory category)
        {
            return TypesByCategory.TryGetValue(category, out var types)
                ? types
                : Array.Empty<FileTypeDefinition>();
        }

        /// <summary>
        /// 지정된 확장자가 특정 카테고리에 속하는지 확인합니다.
        /// </summary>
        /// <param name="extension">확인할 파일 확장자</param>
        /// <param name="category">확인할 카테고리</param>
        /// <returns>해당 카테고리에 속하면 true, 그렇지 않으면 false</returns>
        public static bool IsTypeOf(string extension, FileCategory category)
        {
            var fileType = GetByExtension(extension);
            return fileType.Category == category;
        }

        /// <summary>
        /// Unknown 타입의 FileTypeDefinition을 생성합니다.
        /// </summary>
        /// <param name="extension">파일 확장자</param>
        /// <returns>Unknown 타입의 FileTypeDefinition</returns>
        private static FileTypeDefinition CreateUnknownType(string extension)
        {
            return new FileTypeDefinition(
                extension,
                "Unknown File Type",
                FileCategory.Common.Unknown);
        }

        /// <summary>
        /// 지정된 카테고리에 속하는 모든 파일 타입을 반환합니다.
        /// </summary>
        /// <param name="category">찾을 파일 카테고리</param>
        /// <returns>파일 타입 정의의 열거</returns>
        public static IEnumerable<FileTypeDefinition> GetTypesByCategory(FileCategory category) =>
            TypesByCategory.TryGetValue(category, out var types) ? types : Enumerable.Empty<FileTypeDefinition>();

        /// <summary>
        /// 지정된 MIME 타입에 해당하는 모든 파일 타입을 반환합니다.
        /// </summary>
        /// <param name="mimeType">찾을 MIME 타입</param>
        /// <returns>파일 타입 정의의 열거</returns>
        public static IEnumerable<FileTypeDefinition> GetTypesByMimeType(string mimeType) =>
            TypesByMimeType.TryGetValue(mimeType, out var types) ? types : Enumerable.Empty<FileTypeDefinition>();


#if UNITY_EDITOR && UNITY_INCLUDE_TESTS
        /// <summary>
        /// 레지스트리의 모든 타입 정의를 제거합니다.
        /// </summary>
        /// <remarks>
        /// 경고: 이 메서드는 테스트 목적으로만 사용됩니다.
        /// 프로덕션 코드에서 호출하면 예기치 않은 동작이 발생할 수 있습니다.
        /// </remarks>
        [Obsolete("This method is intended for testing purposes only. Do not use in production code.")]
        public static void ClearRegistryForTesting()
        {
            TypesByExtension.Clear();
            TypesByCategory.Clear();
            TypesByMimeType.Clear();
        }
#endif
    }
}