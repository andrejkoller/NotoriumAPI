using PDFiumSharp;

namespace NotoriumAPI.Services
{
    public class PdfThumbnailService
    {
        private readonly IWebHostEnvironment _env;

        public PdfThumbnailService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string GenerateThumbnail(string pdfFilePath)
        {
            var inputPath = Path.Combine(_env.WebRootPath, pdfFilePath);

            if (!File.Exists(inputPath))
                throw new FileNotFoundException($"PDF file not found: {inputPath}");

            var thumbnailName = Path.GetFileNameWithoutExtension(pdfFilePath) + "_thumb.png";
            var outputPath = Path.Combine(_env.WebRootPath, "uploads", thumbnailName);

            try
            {
                using var pdfDoc = new PdfDocument(inputPath);
                var page = pdfDoc.Pages[0];

                int originalWidth = (int)page.Width;
                int originalHeight = (int)page.Height;

                int maxWidth = 300;
                float scale = (float)maxWidth / originalWidth;

                int thumbWidth = (int)(originalWidth * scale);
                int thumbHeight = (int)(originalHeight * scale);

                using var bitmap = new PDFiumBitmap(thumbWidth, thumbHeight, true);

                page.Render(bitmap, (0, 0, thumbWidth, thumbHeight), PDFiumSharp.Enums.PageOrientations.Normal);

                bitmap.Save(outputPath, 150, 150);

                return Path.Combine("uploads", thumbnailName).Replace("\\", "/");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Thumbnail generation failed: {ex.Message}", ex);
            }
        }
    }
}
