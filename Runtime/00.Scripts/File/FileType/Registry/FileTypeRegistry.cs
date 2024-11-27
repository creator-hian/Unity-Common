using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Creator_Hian.Unity.Common
{
    public static class FileTypeRegistry
    {
        private static readonly Dictionary<string, FileTypeDefinition> TypesByExtension =
            new(StringComparer.OrdinalIgnoreCase);

        private static readonly Dictionary<FileCategory, HashSet<FileTypeDefinition>> TypesByCategory =
            new();

        static FileTypeRegistry()
        {
            RegisterBuiltInTypes();
        }

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
        public static void RegisterType(FileTypeDefinition definition)
        {
            TypesByExtension[definition.Extension] = definition;

            if (!TypesByCategory.TryGetValue(definition.Category, out var categoryTypes))
            {
                categoryTypes = new HashSet<FileTypeDefinition>();
                TypesByCategory[definition.Category] = categoryTypes;
            }

            categoryTypes.Add(definition);
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public static FileTypeDefinition GetByExtension(string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return CreateUnknownType(extension);

            extension = extension.ToLowerInvariant();
            return TypesByExtension.TryGetValue(extension, out var type)
                ? type
                : CreateUnknownType(extension);
        }

        public static IReadOnlyCollection<FileTypeDefinition> GetByCategory(FileCategory category)
        {
            return TypesByCategory.TryGetValue(category, out var types)
                ? types
                : Array.Empty<FileTypeDefinition>();
        }

        public static bool IsTypeOf(string extension, FileCategory category)
        {
            var fileType = GetByExtension(extension);
            return fileType.Category == category;
        }

        private static FileTypeDefinition CreateUnknownType(string extension)
        {
            return new FileTypeDefinition(
                extension,
                "Unknown File Type",
                FileCategory.Common.Unknown);
        }
    }
}