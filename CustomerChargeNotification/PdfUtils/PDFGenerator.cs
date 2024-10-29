using CustomerChargeNotification.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace CustomerChargeNotification.PDFGeneration;

public class PdfGenerator : IPdfGenerator
{
    private readonly Func<MemoryStream> _memoryStreamFactory;
    private readonly Func<MemoryStream, PdfWriter> _pdfWriterFactory;

    public PdfGenerator(Func<MemoryStream>? memoryStreamFactory = null, Func<MemoryStream, PdfWriter>? pdfWriterFactory = null)
    {
        _memoryStreamFactory = memoryStreamFactory ?? throw new ArgumentNullException(nameof(memoryStreamFactory));
        _pdfWriterFactory = pdfWriterFactory ?? throw new ArgumentNullException(nameof(pdfWriterFactory));
    }

    public byte[] GetPdfData(ChargeNotification notification)
    {
        using var ms = _memoryStreamFactory();
        var writer = _pdfWriterFactory(ms);
        var pdf = new PdfDocument(writer);
        var document = new Document(pdf);

        AddCustomerDetails(notification, document);
        AddChargesTable(notification, document);
        AddTotal(notification, document);

        document.Close();

        return ms.ToArray();
    }

    void AddTotal(ChargeNotification notification, Document document)
    {
        document.Add(new Paragraph("\n")); // Adds spacing
        document.Add(new Paragraph($"TOTAL (pence): {notification.Total}")
            .SetBold()
            .SetFontSize(12)
            .SetTextAlignment(TextAlignment.RIGHT));
    }

    void AddChargesTable(ChargeNotification notification, Document document)
    {
        document.Add(new Paragraph("CHARGES").SetBold().SetFontSize(14));

        Table table = new Table(3).UseAllAvailableWidth();
        table.AddHeaderCell(new Cell().Add(new Paragraph("Date")).SetTextAlignment(TextAlignment.CENTER));
        table.AddHeaderCell(new Cell().Add(new Paragraph("Game")).SetTextAlignment(TextAlignment.CENTER));
        table.AddHeaderCell(new Cell().Add(new Paragraph("Cost (pence)")).SetTextAlignment(TextAlignment.CENTER));

        foreach (var charge in notification.Charges)
        {
            table.AddCell(new Cell().Add(new Paragraph(charge.Date.ToString("dd/MM/yyyy"))).SetTextAlignment(TextAlignment.CENTER));
            table.AddCell(new Cell().Add(new Paragraph(charge.Game)).SetTextAlignment(TextAlignment.CENTER));
            table.AddCell(new Cell().Add(new Paragraph(charge.Cost.ToString())).SetTextAlignment(TextAlignment.CENTER));
        }

        document.Add(table);
    }

    void AddCustomerDetails(ChargeNotification notification, Document document)
    {
        document.Add(new Paragraph($"Customer Number: {notification.CustomerId}").SetBold().SetFontSize(12));
        document.Add(new Paragraph($"Customer Name: {notification.CustomerName}").SetBold().SetFontSize(12));
    }
}
