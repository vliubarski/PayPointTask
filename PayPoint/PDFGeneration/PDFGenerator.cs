using PayPoint.Models;
using System.Reflection.Metadata;

namespace PayPoint.PDFGeneration;

public class PdfGenerator : IPdfGenerator
{
    public void Generate(ChargeNotification notification)
    {
        //using (var ms = new MemoryStream())
        //{
        //    Document doc = new Document();
        //    PdfWriter.GetInstance(doc, ms);
        //    doc.Open();

        //    doc.Add(new Paragraph($"Customer Number: {notification.CustomerId}"));
        //    doc.Add(new Paragraph($"Customer Name: {notification.CustomerName}"));
        //    doc.Add(new Paragraph("CHARGES"));

        //    foreach (var charge in notification.Charges)
        //    {
        //        doc.Add(new Paragraph($"Game: {charge.Game}"));
        //        doc.Add(new Paragraph($"Total (pence): {charge.Total}"));
        //    }

        //    doc.Close();

            // todo: Save to file or database, or send via email
        // }
    }
}
