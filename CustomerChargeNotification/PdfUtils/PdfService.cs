using CustomerChargeNotification.Models;

namespace CustomerChargeNotification.PDFGeneration;

public class PdfService : IPdfService
{
    private readonly IPdfGenerator _pdfGenerator;
    private readonly IPdfSaver _pdfSaver;

    public PdfService(IPdfGenerator pdfGenerator, IPdfSaver pdfSaver)
    {
        _pdfGenerator = pdfGenerator;
        _pdfSaver = pdfSaver;
    }

    public async Task SaveToFileAsync(ChargeNotification chargeNotification)
    {
        var fieName = DateTime.UtcNow.ToString("yyyy-MM-dd ") + chargeNotification.CustomerName + ".pdf";

        var data = _pdfGenerator.GetPdfData(chargeNotification);
        await _pdfSaver.SaveToFileAsync(data, fieName);
    }
}
