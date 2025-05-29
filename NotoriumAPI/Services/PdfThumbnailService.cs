using PDFiumSharp;

namespace NotoriumAPI.Services
{
    public class PdfThumbnailService(IWebHostEnvironment env)
    {
        public string GenerateThumbnail(string pdfFilePath)
        {
            var inputPath = Path.Combine(env.WebRootPath, pdfFilePath);

            if (!File.Exists(inputPath))
                throw new FileNotFoundException($"PDF file not found: {inputPath}");

            var thumbnailName = Path.GetFileNameWithoutExtension(pdfFilePath) + "_thumb.png";
            var outputPath = Path.Combine(env.WebRootPath, "uploads", thumbnailName);

            try
            {
                using var pdfDoc = new PdfDocument(inputPath);
                var page = pdfDoc.Pages[0];

                int originalWidth = (int)page.Width;
                int originalHeight = (int)page.Height;

                int maxWidth = 600;
                float scale = (float)maxWidth / originalWidth;

                int thumbWidth = (int)(originalWidth * scale);
                int thumbHeight = (int)(originalHeight * scale);

                using var bitmap = new PDFiumBitmap(thumbWidth, thumbHeight, true);

                page.Render(bitmap, (0, 0, thumbWidth, thumbHeight), PDFiumSharp.Enums.PageOrientations.Normal);

                bitmap.Save(outputPath, 300, 300);

                return Path.Combine("uploads", thumbnailName).Replace("\\", "/");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Thumbnail generation failed: {ex.Message}", ex);
            }
        }
    }
}
