namespace Creator_Hian.Unity.Common
{
    public partial record FileCategory
    {
        public static partial class Common
        {
            private static readonly FileCategory _unknown = new("Unknown", "Unknown File Type");
            public static FileCategory Unknown => _unknown;

            public static readonly FileCategory Text = new("Text", "Text File");
            public static readonly FileCategory Image = new("Image", "Image File");
            public static readonly FileCategory Audio = new("Audio", "Audio File");
            public static readonly FileCategory Video = new("Video", "Video File");
            public static readonly FileCategory Document = new("Document", "Document File");
            public static readonly FileCategory Archive = new("Archive", "Archive File");
            public static readonly FileCategory Data = new("Data", "Data File");

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