using System;
using System.Collections.Generic;
using System.IO;

// ReSharper disable once CheckNamespace
namespace Creator_Hian.Unity.Common
{
    /// <summary>
    /// 파일 타입 정보를 제공하는 리졸버입니다.
    /// </summary>
    public class FileTypeResolver : IFileTypeResolver
    {
        private static FileTypeResolver _instance;
        private static readonly object _lock = new();
        
        private readonly Dictionary<string, FileTypeDefinition> _typesByExtension;
        private readonly Dictionary<FileCategory, HashSet<FileTypeDefinition>> _typesByCategory;
        private readonly Dictionary<string, HashSet<FileTypeDefinition>> _typesByMimeType;

        /// <summary>
        /// FileTypeResolver의 인스턴스를 가져옵니다.
        /// </summary>
        public static FileTypeResolver Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new FileTypeResolver();
                    }
                }
                return _instance;
            }
        }

        private FileTypeResolver()
        {
            _typesByExtension = new Dictionary<string, FileTypeDefinition>(StringComparer.OrdinalIgnoreCase);
            _typesByCategory = new Dictionary<FileCategory, HashSet<FileTypeDefinition>>();
            _typesByMimeType = new Dictionary<string, HashSet<FileTypeDefinition>>(StringComparer.OrdinalIgnoreCase);
            
            FileCategory.EnsureInitialized();
            RegisterBuiltInTypes();
        }

        private void RegisterBuiltInTypes()
        {
            foreach (var type in FileTypes.GetAllTypes())
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

            foreach (var mimeType in definition.MimeTypes)
            {
                if (!_typesByMimeType.TryGetValue(mimeType, out var mimeTypes))
                {
                    mimeTypes = new HashSet<FileTypeDefinition>();
                    _typesByMimeType[mimeType] = mimeTypes;
                }
                mimeTypes.Add(definition);
            }
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

        public IReadOnlyCollection<IFileTypeInfo> GetTypesByMimeType(string mimeType)
        {
            if (string.IsNullOrEmpty(mimeType))
                return Array.Empty<IFileTypeInfo>();

            try
            {
                return _typesByMimeType.TryGetValue(mimeType, out var types)
                    ? types
                    : Array.Empty<IFileTypeInfo>();
            }
            catch (Exception ex)
            {
                throw new FileTypeResolveException($"MIME 타입으로 파일 타입을 조회하는 중 오류가 발생했습니다: {mimeType}", ex);
            }
        }

        private static FileTypeDefinition CreateUnknownType(string extension)
        {
            return new FileTypeDefinition(
                extension,
                "Unknown File Type",
                FileCategory.Common.Unknown,
                FileConstants.MimeTypes.Default);
        }
    }
} 