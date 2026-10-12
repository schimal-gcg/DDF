using DocumentFormat.OpenXml.Packaging;
using OfficeOpenXml;
using System.Text;
using UglyToad.PdfPig;

namespace Skill.D365.Services
{
    public class FileIngestService : IFileIngestService
    {
        public async Task<string> ExtractTextAsync(Microsoft.AspNetCore.Http.IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            ms.Position = 0;

            try
            {
                if (ext == ".docx")
                {
                    return ExtractDocx(ms);
                }
                if (ext == ".pdf")
                {
                    return ExtractPdf(ms);
                }
                if (ext == ".xlsx" || ext == ".xls" || ext == ".csv")
                {
                    return ExtractExcel(ms);
                }

                // fallback: return raw bytes as utf8
                ms.Position = 0;
                using var sr = new StreamReader(ms, Encoding.UTF8);
                return await sr.ReadToEndAsync();
            }
            catch (Exception ex)
            {
                return $""; // ignore errors in scaffold
            }
        }

        private string ExtractDocx(Stream ms)
        {
            ms.Position = 0;
            using var word = WordprocessingDocument.Open(ms, false);
            var body = word.MainDocumentPart?.Document?.Body;
            return body?.InnerText ?? string.Empty;
        }

        private string ExtractPdf(Stream ms)
        {
            ms.Position = 0;
            var sb = new StringBuilder();
            using (var doc = PdfDocument.Open(ms))
            {
                foreach (var page in doc.GetPages())
                {
                    sb.AppendLine(page.Text);
                }
            }
            return sb.ToString();
        }

        private string ExtractExcel(Stream ms)
        {
            ms.Position = 0;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(ms);
            var sb = new StringBuilder();
            foreach (var ws in package.Workbook.Worksheets)
            {
                var rows = ws.Dimension?.Rows ?? 0;
                var cols = ws.Dimension?.Columns ?? 0;
                for (int r = 1; r <= rows; r++)
                {
                    for (int c = 1; c <= cols; c++)
                    {
                        var v = ws.Cells[r, c].Text;
                        if (!string.IsNullOrEmpty(v))
                        {
                            sb.Append(v);
                            sb.Append('\t');
                        }
                    }
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }
    }
}
