using System;
using System.Collections.Generic;

namespace Creator_Hian.Unity.Common
{
    public partial record FileCategory
    {
        public string Name { get; }
        public string Description { get; }

        private FileCategory(string name, string description)
        {
            Name = name;
            Description = description;
        }

        private static readonly Dictionary<string, FileCategory> _categories 
            = new(StringComparer.OrdinalIgnoreCase);

        private static readonly Lazy<bool> _initialization = new(() =>
        {
            RegisterBuiltInCategories();
            return true;
        });

        public static void EnsureInitialized()
        {
            _ = _initialization.Value;
        }

        private static void RegisterBuiltInCategories()
        {
            Common.RegisterAll();
            Unity.RegisterAll();
        }

        public static void RegisterCategory(FileCategory category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));
            
            _categories[category.Name] = category;
        }

        public static FileCategory GetCategory(string name)
        {
            EnsureInitialized();
            return _categories.TryGetValue(name, out var category) 
                ? category 
                : Common.Unknown;
        }

        public override string ToString() => Name;
    }
} 