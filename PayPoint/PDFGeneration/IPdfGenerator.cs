using PayPoint.Models;

namespace PayPoint.PDFGeneration;

public interface IPdfGenerator
{
    void Generate(ChargeNotification notification);
}