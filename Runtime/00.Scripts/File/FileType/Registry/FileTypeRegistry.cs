using System;
using System.Collections.Generic;

namespace Creator_Hian.Unity.Common
{
    public static class FileTypeRegistry
    {
        private static readonly Dictionary<string, FileTypeDefinition> _typesByExtension =
            new(StringComparer.OrdinalIgnoreCase);

        private static readonly Dictionary<FileCategory, HashSet<FileTypeDefinition>> _typesByCategory =
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

        public static void RegisterType(FileTypeDefinition definition)
        {
            _typesByExtension[definition.Extension] = definition;

            if (!_typesByCategory.TryGetValue(definition.Category, out var categoryTypes))
            {
                categoryTypes = new HashSet<FileTypeDefinition>();
                _typesByCategory[definition.Category] = categoryTypes;
            }

            categoryTypes.Add(definition);
        }

        public static FileTypeDefinition GetByExtension(string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return CreateUnknownType(extension);

            extension = extension.ToLowerInvariant();
            return _typesByExtension.TryGetValue(extension, out var type)
                ? type
                : CreateUnknownType(extension);
        }

        public static IReadOnlyCollection<FileTypeDefinition> GetByCategory(FileCategory category)
        {
            return _typesByCategory.TryGetValue(category, out var types)
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