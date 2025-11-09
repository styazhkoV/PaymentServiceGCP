// Services/FirestoreService.cs
using Google.Cloud.Firestore;

public class FirestoreService
{
    private readonly FirestoreDb _db;
    private const string CollectionPayments = "payments";

    public FirestoreService(IConfiguration cfg)
    {
        var projectId = cfg["GCP:ProjectId"]; // можно брать и из GOOGLE_PROJECT_ID
        _db = new FirestoreDbBuilder { ProjectId = projectId }.Build();
    }

    public async Task<PaymentRecord> SavePaymentAsync(PaymentRecord record)
    {
        var col = _db.Collection(CollectionPayments);
        var doc = col.Document(); // auto id
        record.Id = doc.Id;
        record.CreatedAt = DateTime.UtcNow;
        await doc.SetAsync(record);
        return record;
    }

    public async Task UpdatePaymentAsync(string id, Dictionary<string, object> updates)
    {
        var doc = _db.Collection(CollectionPayments).Document(id);
        await doc.UpdateAsync(updates);
    }
}