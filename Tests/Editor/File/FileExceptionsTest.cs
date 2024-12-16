using NUnit.Framework;
using System;
using Creator_Hian.Unity.Common;
using System.IO;

namespace FileExtensions.Exceptions
{
    /// <summary>
    /// 파일 작업 관련 예외 클래스들의 기능을 테스트합니다.
    /// </summary>
    public class FileExceptionsTest
    {
        /// <summary>
        /// DirectoryCreationException의 생성자들이 올바르게 동작하는지 테스트합니다.
        /// </summary>
        /// <remarks>
        /// 검증 항목:
        /// 1. 기본 생성자로 생성된 예외의 메시지 확인
        /// 2. 사용자 지정 메시지를 포함한 예외 생성 확인
        /// 3. 내부 예외를 포함한 예외 생성 및 전파 확인
        /// </remarks>
        [Test]
        public void DirectoryCreationException_Constructor_Tests()
        {
            // 기본 생성자 테스트
            var ex1 = new DirectoryCreationException();
            Assert.That(ex1.Message, Is.EqualTo("Exception of type 'Creator_Hian.Unity.Common.DirectoryCreationException' was thrown."));

            // 메시지와 함께 생성
            string message = "디렉토리 생성 실패";
            var ex2 = new DirectoryCreationException(message);
            Assert.That(ex2.Message, Is.EqualTo(message));

            // 내부 예외와 함께 생성
            var innerException = new Exception("내부 오류");
            var ex3 = new DirectoryCreationException(message, innerException);
            Assert.That(ex3.Message, Is.EqualTo(message));
            Assert.That(ex3.InnerException, Is.EqualTo(innerException));
        }

        /// <summary>
        /// FilePathException의 생성자들이 올바르게 동작하는지 테스트합니다.
        /// </summary>
        /// <remarks>
        /// 검증 항목:
        /// 1. 기본 생성자로 생성된 예외의 메시지 확인
        /// 2. 사용자 지정 메시지를 포함한 예외 생성 확인
        /// 3. 내부 예외를 포함한 예외 생성 및 전파 확인
        /// </remarks>
        [Test]
        public void FilePathException_Constructor_Tests()
        {
            // 기본 생성자 테스트
            var ex1 = new FilePathException();
            Assert.That(ex1.Message, Is.EqualTo("Exception of type 'Creator_Hian.Unity.Common.FilePathException' was thrown."));

            // 메시지와 함께 생성
            string message = "잘못된 파일 경로";
            var ex2 = new FilePathException(message);
            Assert.That(ex2.Message, Is.EqualTo(message));

            // 내부 예외와 함께 생성
            var innerException = new Exception("내부 오류");
            var ex3 = new FilePathException(message, innerException);
            Assert.That(ex3.Message, Is.EqualTo(message));
            Assert.That(ex3.InnerException, Is.EqualTo(innerException));
        }

        /// <summary>
        /// FileWriteException의 생성자들이 올바르게 동작하는지 테스트합니다.
        /// </summary>
        /// <remarks>
        /// 검증 항목:
        /// 1. 기본 생성자로 생성된 예외의 메시지 확인
        /// 2. 사용자 지정 메시지를 포함한 예외 생성 확인
        /// 3. 내부 예외를 포함한 예외 생성 및 전파 확인
        /// </remarks>
        [Test]
        public void FileWriteException_Constructor_Tests()
        {
            // 기본 생성자 테스트
            var ex1 = new FileWriteException();
            Assert.That(ex1.Message, Is.EqualTo("Exception of type 'Creator_Hian.Unity.Common.FileWriteException' was thrown."));

            // 메시지와 함께 생성
            string message = "파일 쓰기 실패";
            var ex2 = new FileWriteException(message);
            Assert.That(ex2.Message, Is.EqualTo(message));

            // 내부 예외와 함께 생성
            var innerException = new Exception("내부 오류");
            var ex3 = new FileWriteException(message, innerException);
            Assert.That(ex3.Message, Is.EqualTo(message));
            Assert.That(ex3.InnerException, Is.EqualTo(innerException));
        }

        /// <summary>
        /// FileTypeResolveException의 생성자들이 올바르게 동작하는지 테스트합니다.
        /// </summary>
        /// <remarks>
        /// 검증 항목:
        /// 1. 기본 생성자로 생성된 예외의 메시지 확인
        /// 2. 사용자 지정 메시지를 포함한 예외 생성 확인
        /// 3. 내부 예외를 포함한 예외 생성 및 전파 확인
        /// </remarks>
        [Test]
        public void FileTypeResolveException_Constructor_Tests()
        {
            // 기본 생성자 테스트
            var ex1 = new FileTypeResolveException();
            Assert.That(ex1.Message, Is.EqualTo("Exception of type 'Creator_Hian.Unity.Common.FileTypeResolveException' was thrown."));

            // 메시지와 함께 생성
            string message = "파일 타입 해석 실패";
            var ex2 = new FileTypeResolveException(message);
            Assert.That(ex2.Message, Is.EqualTo(message));

            // 내부 예외와 함께 생성
            var innerException = new Exception("내부 오류");
            var ex3 = new FileTypeResolveException(message, innerException);
            Assert.That(ex3.Message, Is.EqualTo(message));
            Assert.That(ex3.InnerException, Is.EqualTo(innerException));
        }

        /// <summary>
        /// FileOperationException의 생성자들이 올바르게 동작하는지 테스트합니다.
        /// </summary>
        /// <remarks>
        /// 검증 항목:
        /// 1. 기본 생성자로 생성된 예외의 메시지 확인
        /// 2. 사용자 지정 메시지를 포함한 예외 생성 확인
        /// 3. 내부 예외를 포함한 예외 생성 및 전파 확인
        /// </remarks>
        [Test]
        public void FileOperationException_Constructor_Tests()
        {
            // 기본 생성자 테스트
            var ex1 = new FileOperationException();
            Assert.That(ex1.Message, Is.EqualTo("Exception of type 'Creator_Hian.Unity.Common.FileOperationException' was thrown."));

            // 메시지와 함께 생성
            string message = "파일 작업 실패";
            var ex2 = new FileOperationException(message);
            Assert.That(ex2.Message, Is.EqualTo(message));

            // 내부 예외와 함께 생성
            var innerException = new Exception("내부 오류");
            var ex3 = new FileOperationException(message, innerException);
            Assert.That(ex3.Message, Is.EqualTo(message));
            Assert.That(ex3.InnerException, Is.EqualTo(innerException));
        }

        /// <summary>
        /// FileTypeResolveException의 직렬화 기능을 테스트합니다.
        /// </summary>
        [Test]
        public void FileTypeResolveException_Serialization_PreservesData()
        {
            // Arrange
            var message = "Test error message";
            var innerException = new Exception("Inner exception");
            var original = new FileTypeResolveException(message, innerException);

            // Act - 직렬화 및 역직렬화
            var serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            using var stream = new MemoryStream();
            serializer.Serialize(stream, original);
            stream.Position = 0;
            var deserialized = (FileTypeResolveException)serializer.Deserialize(stream);

            // Assert
            Assert.That(deserialized.Message, Is.EqualTo(original.Message));
            Assert.That(deserialized.InnerException?.Message, Is.EqualTo(original.InnerException?.Message));
        }

        /// <summary>
        /// FileTypeResolveException의 스택 트레이스 보존을 테스트합니다.
        /// </summary>
        [Test]
        public void FileTypeResolveException_PreservesStackTrace()
        {
            // Arrange
            FileTypeResolveException exception = null;

            try
            {
                throw new FileTypeResolveException("Test exception");
            }
            catch (FileTypeResolveException ex)
            {
                exception = ex;
            }

            // Assert
            Assert.That(exception.StackTrace, Is.Not.Null);
            Assert.That(exception.StackTrace, Contains.Substring(nameof(FileTypeResolveException_PreservesStackTrace)));
        }
    }
}