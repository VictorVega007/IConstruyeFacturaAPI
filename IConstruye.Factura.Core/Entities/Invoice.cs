using IConstruye.Factura.Core.Entities.Base;
using IConstruye.Factura.Core.ValueObjects;

namespace IConstruye.Factura.Core.Entities;

public class Invoice : Entity
{
    public string Issuer { get; set; } = string.Empty;
    public string Receiver { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string OriginalXml { get; set; } = string.Empty;
    public string ShortUrl { get; set; } = string.Empty;
    public QueryCounter QueryCounter { get; private set; } = new();
    public void RegisterView() => QueryCounter.RegisterView();
    public DateTime ExpiresAt { get; set; }
    public bool CanBeConsulted => QueryCounter.CanBeConsulted;
    public int TimesConsulted => QueryCounter.TimesConsulted;
    public bool IsExpired => DateTime.UtcNow > ExpiresAt;
}