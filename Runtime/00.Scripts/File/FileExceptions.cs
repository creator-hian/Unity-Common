using System;

namespace Creator_Hian.Unity.Common
{
    public class DirectoryCreationException : Exception
    {
        public DirectoryCreationException()
        {
        }

        public DirectoryCreationException(string message) : base(message)
        {
        }

        public DirectoryCreationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class FilePathException : Exception
    {
        public FilePathException()
        {
        }

        public FilePathException(string message) : base(message)
        {
        }

        public FilePathException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class FileWriteException : Exception
    {
        public FileWriteException()
        {
        }

        public FileWriteException(string message) : base(message)
        {
        }

        public FileWriteException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class FileOperationException : Exception
    {
        public FileOperationException() { }
        public FileOperationException(string message) : base(message) { }
        public FileOperationException(string message, Exception innerException) : base(message, innerException) { }
    }
}