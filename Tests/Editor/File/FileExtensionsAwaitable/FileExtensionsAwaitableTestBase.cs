#if UNITY_2023_2_OR_NEWER
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

namespace FileExtensions.Awaitable
{
    /// <summary>
    /// FileExtensions Awaitable 테스트들의 기본 클래스입니다.
    /// </summary>
    public abstract class FileExtensionsAwaitableTestBase
    {
        protected string _testDirectory;
        protected string _testFile1;
        protected string _testFile2;
        protected byte[] _testData;

        [SetUp]
        public virtual async Task Setup()
        {
            _testDirectory = Path.Combine(Application.temporaryCachePath, "FileExtensionsAwaitableTest");
            _testFile1 = Path.Combine(_testDirectory, "test1.txt");
            _testFile2 = Path.Combine(_testDirectory, "test2.txt");
            _testData = new byte[] { 1, 2, 3, 4, 5 };

            if (Directory.Exists(_testDirectory))
            {
                await CleanupDirectoryAsync(_testDirectory);
            }
            
            Directory.CreateDirectory(_testDirectory);
        }

        [TearDown]
        public virtual async Task TearDown()
        {
            if (Directory.Exists(_testDirectory))
            {
                await CleanupDirectoryAsync(_testDirectory);
            }
        }

        /// <summary>
        /// 디렉토리와 그 내용을 안전하게 삭제합니다.
        /// </summary>
        protected async Task CleanupDirectoryAsync(string directory)
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    // 파일 핸들이 해제될 때까지 대기
                    await Task.Delay(100);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    // 모든 파일의 핸들을 해제하기 위해 시도
                    if (Directory.Exists(directory))
                    {
                        foreach (string file in Directory.GetFiles(directory, "*", SearchOption.AllDirectories))
                        {
                            try
                            {
                                File.SetAttributes(file, FileAttributes.Normal);
                                File.Delete(file);
                            }
                            catch
                            {
                                // 개별 파일 삭제 실패는 무시
                            }
                        }
                        Directory.Delete(directory, true);
                    }
                    break;
                }
                catch (IOException)
                {
                    if (i == 2) throw;
                    await Task.Delay(100);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                catch (UnauthorizedAccessException)
                {
                    if (i == 2) throw;
                    await Task.Delay(100);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
        }
    }
}
#endif