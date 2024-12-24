// ReSharper disable once CheckNamespace

using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Creator_Hian.Unity.Common.Tests
{
    public static class CustomCategories
    {
        public static readonly FileCategory Document = FileCategory.CreateCategory(
            "CustomDocument",
            "Custom Document Type"
        );

        public static readonly FileCategory Script = FileCategory.CreateCategory(
            "CustomScript",
            "Custom Script Type"
        );
    }

    public static class CustomFileTypes
    {
        private static readonly FileTypeDefinition Markdown = new(
            ".md",
            "Markdown Document",
            CustomCategories.Document,
            "text/markdown"
        );

        private static readonly FileTypeDefinition Python = new(
            ".py",
            "Python Script",
            CustomCategories.Script,
            "text/x-python"
        );

        // ReSharper disable once MemberCanBePrivate.Global
        public static IEnumerable<FileTypeDefinition> GetTypes()
        {
            yield return Markdown;
            yield return Python;
        }

        public static void Register()
        {
            FileTypes.RegisterTypeProvider(GetTypes);
        }
    }
}
