using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Creator_Hian.Unity.Common
{
    /// <summary>
    /// 파일의 카테고리를 나타내는 레코드입니다.
    /// </summary>
    public partial record FileCategory
    {
        /// <summary>
        /// 카테고리의 이름
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 카테고리에 대한 설명
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// FileCategory의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="name">카테고리 이름</param>
        /// <param name="description">카테고리 설명</param>
        private FileCategory(string name, string description)
        {
            Name = name;
            Description = description;
        }

        private static readonly Dictionary<string, FileCategory> Categories
            = new(StringComparer.OrdinalIgnoreCase);

        private static readonly HashSet<Action> CustomCategoryInitializers = new();

        /// <summary>
        /// 커스텀 카테고리 초기화 함수를 등록합니다.
        /// </summary>
        /// <param name="initializeAction">초기화 함수</param>
        /// <exception cref="ArgumentNullException">initializeAction이 null인 경우</exception>
        public static void RegisterCustomCategoryInitializer(Action initializeAction)
        {
            if (initializeAction == null)
                throw new ArgumentNullException(nameof(initializeAction));

            CustomCategoryInitializers.Add(initializeAction);
        }

        private static void InitializeAllCategories()
        {
            RegisterBuiltInCategories();

            // 등록된 모든 커스텀 카테고리 초기화
            foreach (var initializer in CustomCategoryInitializers)
            {
                initializer.Invoke();
            }
        }

        private static readonly Lazy<bool> Initialization = new(() =>
        {
            InitializeAllCategories();
            return true;
        });

        /// <summary>
        /// 카테고리 시스템이 초기화되었는지 확인하고, 필요한 경우 초기화합니다.
        /// </summary>
        public static void EnsureInitialized()
        {
            _ = Initialization.Value;
        }

        private static void RegisterBuiltInCategories()
        {
            Common.RegisterAll();
            Unity.RegisterAll();
        }
        // ReSharper disable once MemberCanBePrivate.Global
        /// <summary>
        /// 새로운 카테고리를 등록합니다.
        /// </summary>
        /// <param name="category">등록할 카테고리</param>
        /// <exception cref="ArgumentNullException">category가 null인 경우</exception>
        public static void RegisterCategory(FileCategory category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            Categories[category.Name] = category;
        }

        /// <summary>
        /// 지정된 이름의 카테고리를 가져옵니다.
        /// </summary>
        /// <param name="name">카테고리 이름</param>
        /// <returns>해당하는 카테고리, 없으면 Unknown 카테고리</returns>
        public static FileCategory GetCategory(string name)
        {
            EnsureInitialized();

            if (string.IsNullOrWhiteSpace(name))
                return Common.Unknown;

            return Categories.TryGetValue(name, out var category)
                ? category
                : Common.Unknown;
        }

        /// <summary>
        /// 현재 카테고리의 문자열 표현을 반환합니다.
        /// </summary>
        /// <returns>카테고리의 이름</returns>
        public override string ToString() => Name;

        /// <summary>
        /// 새로운 FileCategory를 생성하고 등록합니다.
        /// </summary>
        public static FileCategory CreateCategory(string name, string description)
        {
            var category = new FileCategory(name, description);
            RegisterCategory(category);
            return category;
        }
    }
}