// Controllers/PaymentsController.cs

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly FirestoreService _firestore;
    private readonly ReceiptService _receipt;
    private readonly StorageService _storage;
    private readonly ILogger<PaymentsController> _log;

    public PaymentsController(FirestoreService firestore, ReceiptService receipt, StorageService storage, ILogger<PaymentsController> log)
    {
        _firestore = firestore;
        _receipt = receipt;
        _storage = storage;
        _log = log;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PaymentRequest req)
    {
        // 1. Тут вы вызываете платежный шлюз или симулируете оплату.
        // Для демо пометим как SUCCESS.
        var record = new PaymentRecord
        {
            OrderId = req.OrderId,
            Amount = req.Amount,
            Currency = req.Currency,
            Status = "PAID"
        };

        // 2. Сохраняем в Firestore
        record = await _firestore.SavePaymentAsync(record);

        // 3. Генерируем квитанцию PDF
        var pdfBytes = _receipt.GeneratePdfReceipt(record);
        var objectName = $"receipts/{record.Id}.pdf";
        var url = await _storage.UploadReceiptAsync(objectName, pdfBytes);

        // 4. Обновляем запись с ссылкой на квитанцию
        await _firestore.UpdatePaymentAsync(record.Id, new Dictionary<string, object>
        {
            {"ReceiptUrl", url}
        });

        // 5. (Опционально) публикуем событие в Pub/Sub для downstream обработчиков
        // await _pubsub.PublishPaymentCompleted(record);

        return Ok(new { id = record.Id, receiptUrl = url });
    }
}