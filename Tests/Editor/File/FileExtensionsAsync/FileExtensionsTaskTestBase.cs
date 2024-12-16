using System;
using System.IO;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

namespace FileExtensions.Async
{
    /// <summary>
    /// FileExtensions Task 테스트들의 기본 클래스입니다.
    /// </summary>
    /// <remarks>
    /// 공통 기능:
    /// - 테스트 디렉토리 설정
    /// - 테스트 파일 경로 관리
    /// - 테스트 데이터 준비
    /// - 리소스 정리
    /// </remarks>
    public abstract class FileExtensionsTaskTestBase
    {
        protected string _testDirectory;
        protected string _testFile1;
        protected string _testFile2;
        protected byte[] _testData;

        /// <summary>
        /// 각 테스트 전에 실행되어 테스트 환경을 준비합니다.
        /// </summary>
        [SetUp]
        public virtual void Setup()
        {
            _testDirectory = Path.Combine(Application.temporaryCachePath, "FileExtensionsTaskTest");
            _testFile1 = Path.Combine(_testDirectory, "test1.txt");
            _testFile2 = Path.Combine(_testDirectory, "test2.txt");
            _testData = new byte[] { 1, 2, 3, 4, 5 };

            if (Directory.Exists(_testDirectory))
            {
                CleanupDirectory(_testDirectory);
            }
            
            Directory.CreateDirectory(_testDirectory);
        }

        /// <summary>
        /// 각 테스트 후에 실행되어 테스트 환경을 정리합니다.
        /// </summary>
        [TearDown]
        public virtual void TearDown()
        {
            if (Directory.Exists(_testDirectory))
            {
                CleanupDirectory(_testDirectory);
            }
        }

        /// <summary>
        /// 디렉토리와 그 내용을 안전하��� 삭제합니다.
        /// </summary>
        /// <param name="directory">삭제할 디렉토리 경로</param>
        /// <remarks>
        /// 파일 핸들이 해제될 때까지 최대 3번 시도하며,
        /// 실패 시 GC를 실행하여 리소스를 정리합니다.
        /// </remarks>
        protected void CleanupDirectory(string directory)
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    Directory.Delete(directory, true);
                    break;
                }
                catch (IOException)
                {
                    if (i == 2) throw;
                    Thread.Sleep(100);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
        }
    }
} 