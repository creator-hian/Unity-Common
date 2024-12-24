// ReSharper disable once CheckNamespace
namespace Creator_Hian.Unity.Common.Tests
{
    /// <summary>
    /// FileType 관련 테스트에서 사용되는 상수값들을 정의합니다.
    /// </summary>
    public static class FileTypeTestConstants
    {
        /// <summary>
        /// 파일 확장자 관련 상수
        /// </summary>
        public static class Extensions
        {
            // Common Types
            public const string Text = ".txt";
            public const string Json = ".json";
            public const string Xml = ".xml";
            public const string Csv = ".csv";
            public const string Html = ".html";

            public const string Jpeg = ".jpg";
            public const string Png = ".png";
            public const string Gif = ".gif";

            public const string Pdf = ".pdf";
            public const string Zip = ".zip";

            // Unity Types
            public const string Scene = ".unity";
            public const string Prefab = ".prefab";
            public const string Asset = ".asset";
            public const string Meta = ".meta";
            public const string Material = ".mat";
            public const string Shader = ".shader";
            public const string Animation = ".anim";
            public const string Controller = ".controller";

            // Invalid Types
            public const string Unknown = ".xyz";
        }

        /// <summary>
        /// MIME 타입 관련 상수
        /// </summary>
        public static class MimeTypes
        {
            public const string TextPlain = "text/plain";
            public const string ApplicationJson = "application/json";
            public const string ApplicationXml = "application/xml";
            public const string TextCsv = "text/csv";
            public const string TextHtml = "text/html";

            public const string ImageJpeg = "image/jpeg";
            public const string ImagePng = "image/png";
            public const string ImageGif = "image/gif";

            public const string ApplicationPdf = "application/pdf";
            public const string ApplicationZip = "application/zip";

            public const string UnityScene = "application/x-unity-scene";
            public const string UnityPrefab = "application/x-unity-prefab";
            public const string UnityAsset = "application/x-unity-asset";
            public const string UnityMeta = "application/x-unity-meta";
            public const string UnityMaterial = "application/x-unity-material";
            public const string UnityShader = "application/x-unity-shader";
            public const string UnityAnimation = "application/x-unity-animation";
            public const string UnityController = "application/x-unity-animator-controller";

            public const string Default = "application/octet-stream";
        }

        /// <summary>
        /// 테스트용 파일 경로 관련 상수
        /// </summary>
        public static class Paths
        {
            public const string ValidFileName = "test";
            public const string SpecialCharacters = "파일#with spaces-and_symbols";
            public const string NestedPath = "nested/folders/test";
            public const string RelativePath = "./relative/path/test";
            public const string AbsolutePath = "C:/absolute/path/test";

            public static readonly string[] MultipleExtensions = new[]
            {
                "test.tar.gz",
                "script.cs.meta",
                "archive.zip.backup",
            };

            public static readonly string[] CaseSensitiveExtensions = new[]
            {
                "test.TXT",
                "test.Txt",
                "test.txt",
                "TEST.PNG",
                "test.png",
            };

            public static readonly string[] SpecialCharacterPaths = new[]
            {
                "파일.txt",
                "file with spaces.unity",
                "file#with#hash.prefab",
                "file-with-dashes.mat",
                "file_with_underscore.anim",
            };
        }

        /// <summary>
        /// 테스트용 파일 내용 관련 상수
        /// </summary>
        public static class Contents
        {
            public const string TextContent = "Test Data Content";
            public const string JsonContent = /*lang=json,strict*/
                "{\"test\": true}";
            public const string XmlContent =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?><test>true</test>";
        }

        /// <summary>
        /// 테스트용 파일 설명 관련 상수
        /// </summary>
        public static class Descriptions
        {
            public const string Text = "Text File";
            public const string Json = "JSON File";
            public const string Image = "Image File";
            public const string Unknown = "Unknown";
        }

        /// <summary>
        /// 성능 테스트 관련 상수
        /// </summary>
        public static class Performance
        {
            public const int DefaultIterationCount = 10000;
            public const int DefaultTimeoutMilliseconds = 1000;
            public const int DefaultBufferSize = 4096;
        }
    }
}
