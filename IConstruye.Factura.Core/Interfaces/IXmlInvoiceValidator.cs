namespace IConstruye.Factura.Core.Interfaces;

public interface IXmlInvoiceValidator
{
    Task<(bool IsValid, string? ErrorMessage)> ValidateXmlAsync(string xmlContent);
}