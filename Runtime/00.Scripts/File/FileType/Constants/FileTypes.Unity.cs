using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Creator_Hian.Unity.Common
{
    public static partial class FileTypes
    {
        /// <summary>
        /// Unity 관련 파일 타입 정의를 제공하는 정적 클래스입니다.
        /// </summary>
        public static class Unity
        {// ReSharper disable MemberCanBePrivate.Global
            /// <summary>
            /// Unity 씬(.unity) 파일 타입 정의
            /// </summary>
            public static readonly FileTypeDefinition Scene = 
                new(".unity", "Unity Scene", FileCategory.Unity.Scene, FileConstants.MimeTypes.Unity.Scene);

            /// <summary>
            /// Unity 프리팹(.prefab) 파일 타입 정의
            /// </summary>
            public static readonly FileTypeDefinition Prefab = 
                new(".prefab", "Unity Prefab", FileCategory.Unity.Prefab, FileConstants.MimeTypes.Unity.Prefab);

            /// <summary>
            /// Unity 에셋(.asset) 파일 타입 정의
            /// </summary>
            public static readonly FileTypeDefinition Asset = 
                new(".asset", "Unity Asset", FileCategory.Unity.Asset, FileConstants.MimeTypes.Unity.Asset);

            /// <summary>
            /// Unity 메타(.meta) 파일 타입 정의
            /// </summary>
            public static readonly FileTypeDefinition Meta = 
                new(".meta", "Unity Meta", FileCategory.Unity.Asset, FileConstants.MimeTypes.Unity.Meta);

            /// <summary>
            /// Unity 머티리얼(.mat) 파일 타입 정의
            /// </summary>
            public static readonly FileTypeDefinition Material = 
                new(".mat", "Unity Material", FileCategory.Unity.Resource, FileConstants.MimeTypes.Unity.Material);

            /// <summary>
            /// Unity 셰이더(.shader) 파일 타입 정의
            /// </summary>
            public static readonly FileTypeDefinition Shader = 
                new(".shader", "Unity Shader", FileCategory.Unity.Script, FileConstants.MimeTypes.Unity.Shader);

            /// <summary>
            /// Unity 애니메이션(.anim) 파일 타입 정의
            /// </summary>
            public static readonly FileTypeDefinition Animation = 
                new(".anim", "Unity Animation", FileCategory.Unity.Animation, FileConstants.MimeTypes.Unity.Animation);

            /// <summary>
            /// Unity 애니메이터 컨트롤러(.controller) 파일 타입 정의
            /// </summary>
            public static readonly FileTypeDefinition AnimatorController = 
                new(".controller", "Unity Animator Controller", FileCategory.Unity.Animation, FileConstants.MimeTypes.Unity.AnimatorController);
            // ReSharper restore MemberCanBePrivate.Global

            /// <summary>
            /// 정의된 모든 Unity 파일 타입을 가져옵니다.
            /// </summary>
            /// <returns>Unity 파일 타입 정의의 열거</returns>
            public static IEnumerable<FileTypeDefinition> GetAll()
            {
                yield return Scene;
                yield return Prefab;
                yield return Asset;
                yield return Meta;
                yield return Material;
                yield return Shader;
                yield return Animation;
                yield return AnimatorController;
            }

            /// <summary>
            /// 지정된 확장자에 해당하는 Unity 파일 타입을 가져옵니다.
            /// </summary>
            /// <param name="extension">찾을 파일 확장자</param>
            /// <returns>해당하는 파일 타입 정의, 없으면 null</returns>
            public static FileTypeDefinition GetByExtension(string extension)
            {
                return GetAll().FirstOrDefault(fileType => 
                    fileType.Extension.Equals(extension, StringComparison.OrdinalIgnoreCase));
            }
        }
    }
} 