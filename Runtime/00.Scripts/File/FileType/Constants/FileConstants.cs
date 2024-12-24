namespace Creator_Hian.Unity.Common
{
    /// <summary>
    /// 파일 관련 상수들을 정의합니다.
    /// </summary>
    public static class FileConstants
    {
        /// <summary>
        /// MIME 타입 관련 상수
        /// </summary>
        public static class MimeTypes
        {
            /// <summary>
            /// 기본 MIME 타입
            /// </summary>
            public const string Default = "application/octet-stream";

            /// <summary>
            /// 텍스트 관련 MIME 타입
            /// </summary>
            public static class Text
            {
                public const string Plain = "text/plain";
                public const string Html = "text/html";
                public const string Css = "text/css";
                public const string Csv = "text/csv";
                public const string Xml = "text/xml";
                public const string JavaScript = "text/javascript";
            }

            /// <summary>
            /// 이미지 관련 MIME 타입
            /// </summary>
            public static class Image
            {
                public const string Jpeg = "image/jpeg";
                public const string Png = "image/png";
                public const string Gif = "image/gif";
                public const string Svg = "image/svg+xml";
                public const string WebP = "image/webp";
                public const string Tiff = "image/tiff";
            }

            /// <summary>
            /// 오디오 관련 MIME 타입
            /// </summary>
            public static class Audio
            {
                public const string Mpeg = "audio/mpeg";
                public const string Wav = "audio/wav";
                public const string Ogg = "audio/ogg";
                public const string Midi = "audio/midi";
            }

            /// <summary>
            /// 비디오 관련 MIME 타입
            /// </summary>
            public static class Video
            {
                public const string Mp4 = "video/mp4";
                public const string Mpeg = "video/mpeg";
                public const string WebM = "video/webm";
                public const string Quicktime = "video/quicktime";
            }

            /// <summary>
            /// 애플리케이션 관련 MIME 타입
            /// </summary>
            public static class Application
            {
                public const string Json = "application/json";
                public const string Pdf = "application/pdf";
                public const string Zip = "application/zip";
                public const string Xml = "application/xml";
                public const string Stream = "application/octet-stream";
            }

            /// <summary>
            /// Unity 관련 MIME 타입
            /// </summary>
            public static class Unity
            {
                public const string Scene = "application/x-unity-scene";
                public const string Prefab = "application/x-unity-prefab";
                public const string Asset = "application/x-unity-asset";
                public const string Meta = "application/x-unity-meta";
                public const string Material = "application/x-unity-material";
                public const string Shader = "application/x-unity-shader";
                public const string Animation = "application/x-unity-animation";
                public const string AnimatorController = "application/x-unity-animator-controller";
            }
        }

        /// <summary>
        /// 파일 설명 관련 상수
        /// </summary>
        public static class Descriptions
        {
            /// <summary>
            /// 알 수 없는 파일 타입에 대한 설명
            /// </summary>
            public const string Unknown = "Unknown";
        }
    }
}
