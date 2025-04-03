using IronPdf;
namespace BillingApp.Web.Services
{
  

    public class PdfService
    {
        public byte[] GeneratePdfFromHtml(string htmlContent)
        {
            var Renderer = new HtmlToPdf();
            var pdf = Renderer.RenderHtmlAsPdf(htmlContent);
            return pdf.BinaryData;
        }
    }

}
