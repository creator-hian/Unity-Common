using System;
using System.Runtime.Serialization;

// ReSharper disable once CheckNamespace
namespace Creator_Hian.Unity.Common
{
    /// <summary>
    /// 파일 타입 해석 중 발생하는 예외를 나타냅니다.
    /// </summary>
    [Serializable]
    public class FileTypeResolveException : Exception
    {
        /// <summary>
        /// FileTypeResolveException 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        public FileTypeResolveException() { }

        /// <summary>
        /// 지정된 오류 메시지를 사용하여 FileTypeResolveException 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="message">예외를 설명하는 메시지</param>
        public FileTypeResolveException(string message)
            : base(message) { }

        /// <summary>
        /// 지정된 오류 메시지와 이 예외의 원인이 되는 내부 예외에 대한 참조를 사용하여
        /// FileTypeResolveException 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="message">예외를 설명하는 메시지</param>
        /// <param name="innerException">현재 예외의 원인인 예외</param>
        public FileTypeResolveException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// 직렬화된 데이터를 사용하여 FileTypeResolveException 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="info">직렬화된 개체 데이터를 보유하는 SerializationInfo</param>
        /// <param name="context">스트림의 대상에 대한 문맥 정보</param>
        protected FileTypeResolveException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
