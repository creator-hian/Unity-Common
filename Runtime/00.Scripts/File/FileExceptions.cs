using System;

// ReSharper disable once CheckNamespace
namespace Creator_Hian.Unity.Common
{
    /// <summary>
    /// 디렉토리 생성 중 발생하는 예외를 나타냅니다.
    /// </summary>
    [Serializable]
    public class DirectoryCreationException : Exception
    {
        /// <summary>
        /// DirectoryCreationException 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        public DirectoryCreationException()
        { }

        /// <summary>
        /// 지정된 오류 메시지를 사용하여 DirectoryCreationException 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="message">예외를 설명하는 메시지</param>
        public DirectoryCreationException(string message) 
            : base(message) 
        { }

        /// <summary>
        /// 지정된 오류 메시지와 이 예외의 원인이 되는 내부 예외에 대한 참조를 사용하여 
        /// DirectoryCreationException 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="message">예외를 설명하는 메시지</param>
        /// <param name="innerException">현재 예외의 원인인 예외</param>
        public DirectoryCreationException(string message, Exception innerException) 
            : base(message, innerException) 
        { }
    }

    /// <summary>
    /// 파일 경로 관련 오류가 발생할 때 발생하는 예외를 나타냅니다.
    /// </summary>
    [Serializable]
    public class FilePathException : Exception
    {
        /// <summary>
        /// FilePathException 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        public FilePathException()
        { }

        /// <summary>
        /// 지정된 오류 메시지를 사용하여 FilePathException 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="message">예외를 설명하는 메시지</param>
        public FilePathException(string message) 
            : base(message) 
        { }

        /// <summary>
        /// 지정된 오류 메시지와 이 예외의 원인이 되는 내부 예외에 대한 참조를 사용하여 
        /// FilePathException 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="message">예외를 설명하는 메시지</param>
        /// <param name="innerException">현재 예외의 원인인 예외</param>
        public FilePathException(string message, Exception innerException) 
            : base(message, innerException) 
        { }
    }

    /// <summary>
    /// 파일 쓰기 작업 중 발생하는 예외를 나타냅니다.
    /// </summary>
    [Serializable]
    public class FileWriteException : Exception
    {
        /// <summary>
        /// FileWriteException 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        public FileWriteException()
        { }

        /// <summary>
        /// 지정된 오류 메시지를 사용하여 FileWriteException 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="message">예외를 설명하는 메시지</param>
        public FileWriteException(string message) 
            : base(message) 
        { }

        /// <summary>
        /// 지정된 오류 메시지와 이 예외의 원인이 되는 내부 예외에 대한 참조를 사용하여 
        /// FileWriteException 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="message">예외를 설명하는 메시지</param>
        /// <param name="innerException">현재 예외의 원인인 예외</param>
        public FileWriteException(string message, Exception innerException) 
            : base(message, innerException) 
        { }
    }

    /// <summary>
    /// 일반적인 파일 작업 중 발생하는 예외를 나타냅니다.
    /// </summary>
    [Serializable]
    public class FileOperationException : Exception
    {
        /// <summary>
        /// FileOperationException 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        public FileOperationException()
        { }

        /// <summary>
        /// 지정된 오류 메시지를 사용하여 FileOperationException 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="message">예외를 설명하는 메시지</param>
        public FileOperationException(string message) 
            : base(message) 
        { }

        /// <summary>
        /// 지정된 오류 메시지와 이 예외의 원인이 되는 내부 예외에 대한 참조를 사용하여 
        /// FileOperationException 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="message">예외를 설명하는 메시지</param>
        /// <param name="innerException">현재 예외의 원인인 예외</param>
        public FileOperationException(string message, Exception innerException) 
            : base(message, innerException) 
        { }
    }
}