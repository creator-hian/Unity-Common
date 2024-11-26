using System.Collections.Generic;

namespace Creator_Hian.Unity.Common
{
    public interface IFileTypeResolver
    {
        /// <summary>
        /// 파일의 타입 정보를 반환합니다.
        /// </summary>
        /// <param name="path">파일 경로</param>
        /// <returns>파일 타입 정보</returns>
        IFileTypeInfo GetFileType(string path);

        /// <summary>
        /// 특정 카테고리에 속하는 모든 파일 타입을 반환합니다.
        /// </summary>
        IReadOnlyCollection<IFileTypeInfo> GetTypesByCategory(FileCategory category);

        /// <summary>
        /// 파일이 특정 카테고리에 속하는지 확인합니다.
        /// </summary>
        bool IsTypeOf(string path, FileCategory category);
    }
}
