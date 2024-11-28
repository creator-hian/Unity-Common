using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Creator_Hian.Unity.Common
{
    /// <summary>
    /// 파일 타입 정의의 모음을 제공하는 정적 클래스입니다.
    /// </summary>
    public static partial class FileTypes
    {
        /// <summary>
        /// 일반적인 파일 타입 정의를 제공하는 정적 클래스입니다.
        /// </summary>
        public static class Common
        {
            /// <summary>
            /// 텍스트 파일(.txt) 타입 정의
            /// </summary>
            public static readonly FileTypeDefinition Text =
                new(".txt", "Text File", FileCategory.Common.Text, FileConstants.MimeTypes.Text.Plain);

            /// <summary>
            /// JSON 파일(.json) 타입 정의
            /// </summary>
            public static readonly FileTypeDefinition Json =
                new(".json", "JSON File", FileCategory.Common.Data, FileConstants.MimeTypes.Application.Json);

            /// <summary>
            /// XML 파일(.xml) 타입 정의
            /// </summary>
            public static readonly FileTypeDefinition Xml =
                new(".xml", "XML File", FileCategory.Common.Data, 
                    FileConstants.MimeTypes.Application.Xml,  // 주 MIME 타입
                    FileConstants.MimeTypes.Text.Xml);        // 대체 MIME 타입

            /// <summary>
            /// CSV 파일(.csv) 타입 정의
            /// </summary>
            public static readonly FileTypeDefinition Csv =
                new(".csv", "CSV File", FileCategory.Common.Data, FileConstants.MimeTypes.Text.Csv);

            /// <summary>
            /// HTML 파일(.html) 타입 정의
            /// </summary>
            public static readonly FileTypeDefinition Html =
                new(".html", "HTML File", FileCategory.Common.Text, FileConstants.MimeTypes.Text.Html);

            /// <summary>
            /// JPEG 이미지(.jpg) 타입 정의
            /// </summary>
            public static readonly FileTypeDefinition Jpeg =
                new(".jpg", "JPEG Image", FileCategory.Common.Image, FileConstants.MimeTypes.Image.Jpeg);

            /// <summary>
            /// PNG 이미지(.png) 타입 정의
            /// </summary>
            public static readonly FileTypeDefinition Png =
                new(".png", "PNG Image", FileCategory.Common.Image, FileConstants.MimeTypes.Image.Png);

            /// <summary>
            /// GIF 이미지(.gif) 타입 정의
            /// </summary>
            public static readonly FileTypeDefinition Gif =
                new(".gif", "GIF Image", FileCategory.Common.Image, FileConstants.MimeTypes.Image.Gif);

            /// <summary>
            /// PDF 문서(.pdf) 타입 정의
            /// </summary>
            public static readonly FileTypeDefinition Pdf =
                new(".pdf", "PDF Document", FileCategory.Common.Document, FileConstants.MimeTypes.Application.Pdf);

            /// <summary>
            /// ZIP 아카이브(.zip) 타입 정의
            /// </summary>
            public static readonly FileTypeDefinition Zip =
                new(".zip", "ZIP Archive", FileCategory.Common.Archive, FileConstants.MimeTypes.Application.Zip);

            /// <summary>
            /// 정의된 모든 일반 파일 타입을 가져옵니다.
            /// </summary>
            /// <returns>일반 파일 타입 정의의 열거</returns>
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