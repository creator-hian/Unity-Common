using System;
using System.IO;
using System.Text;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using Creator_Hian.Unity.Common;
using Creator_Hian.Unity.Common.Tests;

namespace FileExtensions.Sync
{
    /// <summary>
    /// FileExtensions 동기 테스트들의 기본 클래스입니다.
    /// </summary>
    /// <remarks>
    /// 공통 기능:
    /// - 테스트 디렉토리 설정
    /// - 테스트 데이터 준비
    /// - 리소스 정리
    /// </remarks>
    public abstract class FileExtensionsTestBase
    {
        protected string _testDirectoryPath;
        protected byte[] _testData;

        /// <summary>
        /// 각 테스트 전에 실행되어 테스트 환경을 준비합니다.
        /// </summary>
        [SetUp]
        public virtual void SetUp()
        {
            // 각 테스트마다 새로운 디렉토리 생성
            _testDirectoryPath = Path.Combine(
                Application.temporaryCachePath,
                "FileExtensionsTest",
                Guid.NewGuid().ToString());

            Directory.CreateDirectory(_testDirectoryPath);
            // 테스트용 데이터 생성
            _testData = Encoding.UTF8.GetBytes(FileTypeTestConstants.Contents.TextContent);
        }

        /// <summary>
        /// 각 테스트 후에 실행되어 테스트 환경을 정리합니다.
        /// </summary>
        [TearDown]
        public virtual void TearDown()
        {
            try
            {
                if (Directory.Exists(_testDirectoryPath))
                {
                    // 파일 핸들이 해제될 때까지 대기
                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
                            Directory.Delete(_testDirectoryPath, true);
                            break;
                        }
                        catch (IOException)
                        {
                            if (i < 2)
                            {
                                Thread.Sleep(100);
                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                            }
                        }
                    }
                }
            }
            catch
            {
                // 정리 실패는 무시
            }
        }
    }
} 