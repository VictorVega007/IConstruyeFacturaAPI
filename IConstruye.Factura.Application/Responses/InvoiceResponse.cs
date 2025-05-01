namespace IConstruye.Application.Responses;

public class InvoiceResponse
{
    public string Issuer { get; set; } = string.Empty;
    public string Receiver { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string ShortUrl { get; set; } = string.Empty;

    public int TimesConsulted { get; set; }
    public bool CanBeConsulted { get; set; }
}