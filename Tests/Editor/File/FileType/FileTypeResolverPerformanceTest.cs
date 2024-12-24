using System.Diagnostics;
using Creator_Hian.Unity.Common;
using Creator_Hian.Unity.Common.Tests;
using NUnit.Framework;

/// <summary>
/// FileTypeResolver의 성능을 테스트합니다.
/// </summary>
// ReSharper disable once CheckNamespace
namespace FileExtensions.FileType
{
    public class FileTypeResolverPerformanceTest
    {
        private IFileTypeResolver _resolver;

        /// <summary>
        /// 각 테스트 전에 새로운 FileTypeResolver 인스턴스를 생성합니다.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _resolver = FileTypeResolver.Instance;
        }

        /// <summary>
        /// GetFileType 메서드의 성능을 테스트합니다.
        /// txt, png, unity, xyz(알 수 없는 확장자) 파일에 대해 각각 10000회 반복 실행하여
        /// 전체 처리 시간이 1000ms 미만인지 확인합니다.
        /// </summary>
        [Test]
        public void GetFileType_Performance_MultipleIterations()
        {
            // Arrange
            Stopwatch stopwatch = new Stopwatch();
            string[] testPaths = new[]
            {
                $"{FileTypeTestConstants.Paths.ValidFileName}{FileTypeTestConstants.Extensions.Text}",
                $"{FileTypeTestConstants.Paths.ValidFileName}{FileTypeTestConstants.Extensions.Png}",
                $"{FileTypeTestConstants.Paths.ValidFileName}{FileTypeTestConstants.Extensions.Scene}",
                $"{FileTypeTestConstants.Paths.ValidFileName}{FileTypeTestConstants.Extensions.Unknown}",
            };

            // Act
            stopwatch.Start();
            for (int i = 0; i < FileTypeTestConstants.Performance.DefaultIterationCount; i++)
            {
                foreach (string path in testPaths)
                {
                    _ = _resolver.GetFileType(path);
                }
            }
            stopwatch.Stop();

            // Assert
            Assert.That(
                stopwatch.ElapsedMilliseconds,
                Is.LessThan(FileTypeTestConstants.Performance.DefaultTimeoutMilliseconds),
                "파일 타입 해석이 너무 오래 걸립니다"
            );
        }

        /// <summary>
        /// GetTypesByCategory 메서드의 성능을 테스트합니다.
        /// Image, Animation, Text 카테고리에 대해 각각 10000회 반복 조회하여
        /// 전체 처리 시간이 1000ms 미만인지 확인합니다.
        /// </summary>
        [Test]
        public void GetTypesByCategory_Performance_MultipleIterations()
        {
            // Arrange
            Stopwatch stopwatch = new Stopwatch();
            FileCategory[] categories = new[]
            {
                FileCategory.Common.Image,
                FileCategory.Unity.Animation,
                FileCategory.Common.Text,
            };

            // Act
            stopwatch.Start();
            for (int i = 0; i < FileTypeTestConstants.Performance.DefaultIterationCount; i++)
            {
                foreach (FileCategory category in categories)
                {
                    _ = _resolver.GetTypesByCategory(category);
                }
            }
            stopwatch.Stop();

            // Assert
            Assert.That(
                stopwatch.ElapsedMilliseconds,
                Is.LessThan(FileTypeTestConstants.Performance.DefaultTimeoutMilliseconds),
                "카테고리별 타입 조회가 너무 오래 걸립니다"
            );
        }
    }
}
