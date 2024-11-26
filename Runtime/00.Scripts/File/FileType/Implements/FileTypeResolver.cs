using System;
using System.Collections.Generic;
using System.IO;

namespace Creator_Hian.Unity.Common
{
    /// <summary>
    /// 파일 타입 정보를 제공하는 리졸버입니다.
    /// </summary>
    public class FileTypeResolver : IFileTypeResolver
    {
        private readonly Dictionary<string, FileTypeDefinition> _typesByExtension;
        private readonly Dictionary<FileCategory, HashSet<FileTypeDefinition>> _typesByCategory;

        public FileTypeResolver()
        {
            _typesByExtension = new Dictionary<string, FileTypeDefinition>(StringComparer.OrdinalIgnoreCase);
            _typesByCategory = new Dictionary<FileCategory, HashSet<FileTypeDefinition>>();
            
            // 초기화 보장
            FileCategory.EnsureInitialized();
            RegisterBuiltInTypes();
        }

        private void RegisterBuiltInTypes()
        {
            // Common Types
            foreach (var type in FileTypes.Common.GetAll())
            {
                RegisterType(type);
            }

            // Unity Types
            foreach (var type in FileTypes.Unity.GetAll())
            {
                RegisterType(type);
            }
        }

        private void RegisterType(FileTypeDefinition definition)
        {
            _typesByExtension[definition.Extension] = definition;

            if (!_typesByCategory.TryGetValue(definition.Category, out var categoryTypes))
            {
                categoryTypes = new HashSet<FileTypeDefinition>();
                _typesByCategory[definition.Category] = categoryTypes;
            }

            categoryTypes.Add(definition);
        }

        public IFileTypeInfo GetFileType(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            try
            {
                string extension = Path.GetExtension(path).ToLowerInvariant();
                
                if (string.IsNullOrEmpty(extension))
                    return CreateUnknownType(extension);

                return _typesByExtension.TryGetValue(extension, out var type)
                    ? type
                    : CreateUnknownType(extension);
            }
            catch (Exception ex)
            {
                throw new FileTypeResolveException("파일 타입 확인 중 오류가 발생했습니다.", ex);
            }
        }

        public IReadOnlyCollection<IFileTypeInfo> GetTypesByCategory(FileCategory category)
        {
            return _typesByCategory.TryGetValue(category, out var types)
                ? types
                : Array.Empty<IFileTypeInfo>();
        }

        public bool IsTypeOf(string path, FileCategory category)
        {
            var fileType = GetFileType(path);
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