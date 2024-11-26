namespace Creator_Hian.Unity.Common
{
    public partial record FileCategory
    {
        public static partial class Unity
        {
            public static readonly FileCategory Asset = new("UnityAsset", "Unity Asset File");
            public static readonly FileCategory Script = new("Script", "Script File");
            public static readonly FileCategory Scene = new("Scene", "Unity Scene File");
            public static readonly FileCategory Prefab = new("Prefab", "Unity Prefab File");
            public static readonly FileCategory Resource = new("Resource", "Unity Resource File");
            public static readonly FileCategory Animation = new("Animation", "Unity Animation File");

            internal static void RegisterAll()
            {
                RegisterCategory(Asset);
                RegisterCategory(Script);
                RegisterCategory(Scene);
                RegisterCategory(Prefab);
                RegisterCategory(Resource);
                RegisterCategory(Animation);
            }
        }
    }
} 