// Models/PaymentRecord.cs

using Google.Cloud.Firestore;

[FirestoreData]
public class PaymentRecord
{
    [FirestoreProperty] public string Id { get; set; }
    [FirestoreProperty] public string OrderId { get; set; }
    [FirestoreProperty] public decimal Amount { get; set; }
    [FirestoreProperty] public string Currency { get; set; }
    [FirestoreProperty] public string Status { get; set; }
    [FirestoreProperty] public DateTime CreatedAt { get; set; }
    [FirestoreProperty] public string ReceiptUrl { get; set; }
}