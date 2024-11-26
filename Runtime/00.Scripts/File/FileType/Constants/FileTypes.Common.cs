using System.Collections.Generic;

namespace Creator_Hian.Unity.Common
{
    public static partial class FileTypes
    {
        public static partial class Common
        {
            public static readonly FileTypeDefinition Text = 
                new(".txt", "Text File", FileCategory.Common.Text, "text/plain");
            public static readonly FileTypeDefinition Json = 
                new(".json", "JSON File", FileCategory.Common.Data, "application/json");
            public static readonly FileTypeDefinition Xml = 
                new(".xml", "XML File", FileCategory.Common.Data, "application/xml");
            public static readonly FileTypeDefinition Csv = 
                new(".csv", "CSV File", FileCategory.Common.Data, "text/csv");
            public static readonly FileTypeDefinition Html = 
                new(".html", "HTML File", FileCategory.Common.Text, "text/html");
            
            public static readonly FileTypeDefinition Jpeg = 
                new(".jpg", "JPEG Image", FileCategory.Common.Image, "image/jpeg");
            public static readonly FileTypeDefinition Png = 
                new(".png", "PNG Image", FileCategory.Common.Image, "image/png");
            public static readonly FileTypeDefinition Gif = 
                new(".gif", "GIF Image", FileCategory.Common.Image, "image/gif");
            
            public static readonly FileTypeDefinition Pdf = 
                new(".pdf", "PDF Document", FileCategory.Common.Document, "application/pdf");
            public static readonly FileTypeDefinition Zip = 
                new(".zip", "ZIP Archive", FileCategory.Common.Archive, "application/zip");

            public static IEnumerable<FileTypeDefinition> GetAll()
            {
                yield return Text;
                yield return Json;
                yield return Xml;
                yield return Csv;
                yield return Html;
                yield return Jpeg;
                yield return Png;
                yield return Gif;
                yield return Pdf;
                yield return Zip;
            }
        }
    }
} 