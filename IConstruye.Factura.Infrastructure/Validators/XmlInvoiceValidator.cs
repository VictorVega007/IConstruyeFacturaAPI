using System.Xml;
using IConstruye.Factura.Core.Interfaces;

namespace IConstruye.Factura.Infrastructure.Validators;

public class XmlInvoiceValidator(ICertificateProvider certificateProvider) : IXmlInvoiceValidator
{
    public async Task<(bool IsValid, string? ErrorMessage)> ValidateXmlAsync(string xmlContent)
    {
        return await Task.Run(() =>
        {
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlContent);

                if (xmlDoc.GetElementsByTagName("Signature").Count == 0)
                    return (false, "Missing <Signature> node");

                var invoice = xmlDoc.GetElementsByTagName("Documento").Cast<XmlElement>().FirstOrDefault();
                if (invoice == null) return (false, "Missing <Invoice> node.");

                var issuer = invoice.GetElementsByTagName("Emisor").Cast<XmlElement>().FirstOrDefault()?.InnerText;
                var receiver = invoice.GetElementsByTagName("Receptor").Cast<XmlElement>().FirstOrDefault()?.InnerText;
                var amount = invoice.GetElementsByTagName("Totales").Cast<XmlElement>().FirstOrDefault()?.InnerText;

                if (string.IsNullOrEmpty(issuer))
                    return (false, "The field 'Issuer' is empty.");
                if (string.IsNullOrEmpty(receiver))
                    return (false, "The field 'Receiver' is empty.");
                if (string.IsNullOrEmpty(amount) || !decimal.TryParse(amount, out _))
                    return (false, "The field 'Amount' is empty.");

                var invoiceDate = invoice.GetElementsByTagName("FchEmis").Cast<XmlElement>().FirstOrDefault()?.InnerText;
                if (string.IsNullOrEmpty(invoiceDate) || !DateTime.TryParse(invoiceDate, out _))
                    return (false, "The 'Date' is empty.");

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, $"There is an error in XML validation: {ex.Message}");
            }
        });
    }
}