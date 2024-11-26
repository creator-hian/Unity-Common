using System;

namespace Creator_Hian.Unity.Common
{
    public record FileTypeDefinition : IFileTypeInfo
    {
        public string Extension { get; }
        public string Description { get; }
        public FileCategory Category { get; }
        public string MimeType { get; }

        public FileTypeDefinition(
            string extension,
            string description,
            FileCategory category,
            string mimeType = null)
        {
            Extension = extension?.ToLowerInvariant() 
                ?? throw new ArgumentNullException(nameof(extension));
            Description = description ?? "Unknown";
            Category = category ?? FileCategory.Common.Unknown;
            MimeType = mimeType ?? "application/octet-stream";
        }
    }
} 