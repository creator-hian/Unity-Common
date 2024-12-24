// ReSharper disable once CheckNamespace
namespace Creator_Hian.Unity.Common
{
    public partial record FileCategory
    {
        /// <summary>
        /// Unity 관련 파일 카테고리를 정의하는 정적 클래스입니다.
        /// </summary>
        // ReSharper disable once PartialTypeWithSinglePart
        public static partial class Unity
        {
            // ReSharper disable MemberCanBePrivate.Global
            /// <summary>
            /// Unity 씬 파일을 나타내는 카테고리입니다.
            /// </summary>
            public static readonly FileCategory Scene = new("Scene", "Unity Scene File");

            /// <summary>
            /// Unity 프리팹 파일을 나타내는 카테고리입니다.
            /// </summary>
            public static readonly FileCategory Prefab = new("Prefab", "Unity Prefab File");

            /// <summary>
            /// Unity 에셋 파일을 나타내는 카테고리입니다.
            /// </summary>
            public static readonly FileCategory Asset = new("Asset", "Unity Asset File");

            /// <summary>
            /// Unity 리소스 파일을 나타내는 카테고리입니다.
            /// </summary>
            public static readonly FileCategory Resource = new("Resource", "Unity Resource File");

            /// <summary>
            /// 스크립트 파일을 나타내는 카테고리입니다.
            /// </summary>
            public static readonly FileCategory Script = new("Script", "Script File");

            /// <summary>
            /// Unity 애니메이션 파일을 나타내는 카테고리입니다.
            /// </summary>
            public static readonly FileCategory Animation = new(
                "Animation",
                "Unity Animation File"
            );

            // ReSharper restore MemberCanBePrivate.Global

            /// <summary>
            /// 모든 Unity 관련 카테고리를 등록합니다.
            /// </summary>
            internal static void RegisterAll()
            {
                RegisterCategory(Scene);
                RegisterCategory(Prefab);
                RegisterCategory(Asset);
                RegisterCategory(Resource);
                RegisterCategory(Script);
                RegisterCategory(Animation);
            }
        }
    }
}
