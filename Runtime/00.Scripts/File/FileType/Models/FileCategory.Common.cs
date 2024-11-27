// ReSharper disable once CheckNamespace
namespace Creator_Hian.Unity.Common
{
    public partial record FileCategory
    {
        /// <summary>
        /// 일반적인 파일 카테고리를 정의하는 정적 클래스입니다.
        /// </summary>
        // ReSharper disable once PartialTypeWithSinglePart
        public static partial class Common
        {
            // ReSharper disable once InconsistentNaming
            private static readonly FileCategory _unknown = new("Unknown", "Unknown File Type");
            /// <summary>
            /// 알 수 없는 파일 타입을 나타내는 카테고리입니다.
            /// </summary>
            public static FileCategory Unknown => _unknown;
            
            // ReSharper disable MemberCanBePrivate.Global
            /// <summary>
            /// 텍스트 파일을 나타내는 카테고리입니다.
            /// </summary>
            public static readonly FileCategory Text = new("Text", "Text File");

            /// <summary>
            /// 이미지 파일을 나타내는 카테고리입니다.
            /// </summary>
            public static readonly FileCategory Image = new("Image", "Image File");

            /// <summary>
            /// 오디오 파일을 나타내는 카테고리입니다.
            /// </summary>
            public static readonly FileCategory Audio = new("Audio", "Audio File");

            /// <summary>
            /// 비디오 파일을 나타내는 카테고리입니다.
            /// </summary>
            public static readonly FileCategory Video = new("Video", "Video File");

            /// <summary>
            /// 문서 파일을 나타내는 카테고리입니다.
            /// </summary>
            public static readonly FileCategory Document = new("Document", "Document File");

            /// <summary>
            /// 압축 파일을 나타내는 카테고리입니다.
            /// </summary>
            public static readonly FileCategory Archive = new("Archive", "Archive File");

            /// <summary>
            /// 데이터 파일을 나타내는 카테고리입니다.
            /// </summary>
            public static readonly FileCategory Data = new("Data", "Data File");
            // ReSharper restore MemberCanBePrivate.Global

            /// <summary>
            /// 모든 일반 카테고리를 등록합니다.
            /// </summary>
            internal static void RegisterAll()
            {
                RegisterCategory(_unknown);
                RegisterCategory(Text);
                RegisterCategory(Image);
                RegisterCategory(Audio);
                RegisterCategory(Video);
                RegisterCategory(Document);
                RegisterCategory(Archive);
                RegisterCategory(Data);
            }
        }
    }
} 