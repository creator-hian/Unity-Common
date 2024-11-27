using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Creator_Hian.Unity.Common
{
    /// <summary>
    /// 파일 타입 정의의 모음을 제공하는 정적 클래스입니다.
    /// </summary>
    public static partial class FileTypes
    {
        private static readonly List<Func<IEnumerable<FileTypeDefinition>>> TypeProviders = new();

        /// <summary>
        /// 파일 타입 제공자를 등록합니다.
        /// </summary>
        /// <param name="provider">파일 타입 정의를 제공하는 함수</param>
        /// <exception cref="ArgumentNullException">provider가 null인 경우</exception>
        public static void RegisterTypeProvider(Func<IEnumerable<FileTypeDefinition>> provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            
            TypeProviders.Add(provider);
        }

        /// <summary>
        /// 등록된 모든 파일 타입을 가져옵니다.
        /// </summary>
        /// <returns>등록된 모든 파일 타입의 열거</returns>
        public static IEnumerable<FileTypeDefinition> GetAllTypes()
        {
            // 기본 타입들
            foreach (var type in Common.GetAll())
                yield return type;
            
            foreach (var type in Unity.GetAll())
                yield return type;
            
            // 확장 타입들
            foreach (var provider in TypeProviders)
            {
                foreach (var type in provider())
                    yield return type;
            }
        }
    }
}