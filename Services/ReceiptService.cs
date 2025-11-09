// Services/ReceiptService.cs
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

public class ReceiptService
{
    public byte[] GeneratePdfReceipt(PaymentRecord payment)
    {
        var doc = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.Header().Text($"Квитанция #{payment.Id}").FontSize(20);
                page.Content()
                    .Column(col =>
                    {
                        col.Item().Text($"Order: {payment.OrderId}");
                        col.Item().Text($"Amount: {payment.Amount} {payment.Currency}");
                        col.Item().Text($"Status: {payment.Status}");
                        col.Item().Text($"Date: {payment.CreatedAt:yyyy-MM-dd HH:mm:ss} UTC");
                    });
                page.Footer().AlignCenter().Text("Спасибо за оплату");
            });
        });

        using var ms = new MemoryStream();
        doc.GeneratePdf(ms);
        return ms.ToArray();
    }
}