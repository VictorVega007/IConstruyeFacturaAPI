namespace IConstruye.Application.Responses;

public class UploadInvoiceResponse
{
    public bool Success { get; set; }
    public string? ShortUrl { get; set; }
    public string? Error { get; set; }
    public string? Issuer { get; set; }
    public string? Receiver { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public DateTime ExpiresAt { get; set; }
}