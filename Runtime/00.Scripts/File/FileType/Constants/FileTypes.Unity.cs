using System;
using System.Collections.Generic;
using System.Linq;

namespace Creator_Hian.Unity.Common
{
    public static partial class FileTypes
    {
        public static partial class Unity
        {
            public static readonly FileTypeDefinition Scene = 
                new(".unity", "Unity Scene", FileCategory.Unity.Scene, "application/x-unity-scene");
            public static readonly FileTypeDefinition Prefab = 
                new(".prefab", "Unity Prefab", FileCategory.Unity.Prefab, "application/x-unity-prefab");
            public static readonly FileTypeDefinition Asset = 
                new(".asset", "Unity Asset", FileCategory.Unity.Asset, "application/x-unity-asset");
            public static readonly FileTypeDefinition Meta = 
                new(".meta", "Unity Meta", FileCategory.Unity.Asset, "application/x-unity-meta");
            public static readonly FileTypeDefinition Material = 
                new(".mat", "Unity Material", FileCategory.Unity.Resource, "application/x-unity-material");
            public static readonly FileTypeDefinition Shader = 
                new(".shader", "Unity Shader", FileCategory.Unity.Script, "application/x-unity-shader");
            public static readonly FileTypeDefinition Animation = 
                new(".anim", "Unity Animation", FileCategory.Unity.Animation, "application/x-unity-animation");
            public static readonly FileTypeDefinition AnimatorController = 
                new(".controller", "Unity Animator Controller", FileCategory.Unity.Animation, "application/x-unity-animator-controller");

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
            public static FileTypeDefinition GetByExtension(string extension)
            {
                return GetAll().FirstOrDefault(fileType => fileType.Extension.Equals(extension, StringComparison.OrdinalIgnoreCase));
            }
        }
    }
} 