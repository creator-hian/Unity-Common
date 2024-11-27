using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Creator_Hian.Unity.Common
{
    /// <summary>
    /// 파일 타입 해석 기능을 제공하는 인터페이스입니다.
    /// </summary>
    public interface IFileTypeResolver
    {
        /// <summary>
        /// 지정된 경로의 파일 타입 정보를 가져옵니다.
        /// </summary>
        /// <param name="path">파일 경로</param>
        /// <returns>파일 타입 정보</returns>
        /// <exception cref="ArgumentNullException">path가 null인 경우</exception>
        /// <exception cref="FileTypeResolveException">파일 타입 해석 중 오류가 발생한 경우</exception>
        IFileTypeInfo GetFileType(string path);

        /// <summary>
        /// 특정 카테고리에 속하는 모든 파일 타입을 가져옵니다.
        /// </summary>
        /// <param name="category">파일 카테고리</param>
        /// <returns>해당 카테고리의 모든 파일 타입 정보</returns>
        IReadOnlyCollection<IFileTypeInfo> GetTypesByCategory(FileCategory category);

        /// <summary>
        /// 파일이 특정 카테고리에 속하는지 확인합니다.
        /// </summary>
        /// <param name="path">파일 경로</param>
        /// <param name="category">확인할 카테고리</param>
        /// <returns>해당 카테고리에 속하면 true, 그렇지 않으면 false</returns>
        /// <exception cref="ArgumentNullException">path가 null인 경우</exception>
        /// <exception cref="FileTypeResolveException">파일 타입 해석 중 오류가 발생한 경우</exception>
        bool IsTypeOf(string path, FileCategory category);

        /// <summary>
        /// 지정된 MIME 타입에 해당하는 모든 파일 타입을 가져옵니다.
        /// </summary>
        /// <param name="mimeType">MIME 타입</param>
        /// <returns>해당 MIME 타입의 모든 파일 타입 정보</returns>
        IReadOnlyCollection<IFileTypeInfo> GetTypesByMimeType(string mimeType);
    }
}
