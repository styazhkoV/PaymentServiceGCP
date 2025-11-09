// Services/StorageService.cs
using Google.Cloud.Storage.V1;

public class StorageService
{
    private readonly StorageClient _client;
    private readonly string _bucket;

    public StorageService(IConfiguration cfg)
    {
        _client = StorageClient.Create();
        _bucket = cfg["GCP:BucketName"];
    }

    public async Task<string> UploadReceiptAsync(string objectName, byte[] pdfBytes)
    {
        using var ms = new MemoryStream(pdfBytes);
        await _client.UploadObjectAsync(_bucket, objectName, "application/pdf", ms);
        // билд публичного URL если публичный доступ включен:
        return $"https://storage.googleapis.com/{_bucket}/{Uri.EscapeDataString(objectName)}";
    }
}