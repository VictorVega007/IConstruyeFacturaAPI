using System.Xml;
using IConstruye.Application.Commands;
using IConstruye.Application.Responses;
using IConstruye.Factura.Core.Entities;
using IConstruye.Factura.Core.Interfaces;
using IConstruye.Factura.Core.Repositories;
using MediatR;

namespace IConstruye.Application.Handlers;

public class UploadInvoiceCommandHandler(
    IXmlInvoiceValidator xmlInvoiceValidador,
    IInvoiceRepository invoiceRepository,
    IInvoiceUrlService invoiceUrlService)
    : IRequestHandler<UploadInvoiceCommand, UploadInvoiceResponse>
{
    public async Task<UploadInvoiceResponse> Handle(UploadInvoiceCommand request, CancellationToken cancellationToken)
    {
        if (request.File.Length == 0)
            return GenerateErrorResponse("The file is empty");

        using var reader =  new StreamReader(request.File.OpenReadStream());
        var xmlContent = await reader.ReadToEndAsync(cancellationToken);
        
        var validationResult = await xmlInvoiceValidador.ValidateXmlAsync(xmlContent);
        
        if (!validationResult.IsValid)
            return GenerateErrorResponse(validationResult.ErrorMessage);

        var xml = new XmlDocument {  PreserveWhitespace = true };
        xml.LoadXml(xmlContent);
        
        var nsmgr = new XmlNamespaceManager(xml.NameTable);
        nsmgr.AddNamespace("ns", "http://www.sii.cl/SiiDte");
        
        var invoice = new Invoice
        {
            OriginalXml = xmlContent,
            Issuer = GetXmlNodeValue(xml, nsmgr, "//ns:Emisor/ns:RUTEmisor", "//Emisor/RUTEmisor"),
            Receiver = GetXmlNodeValue(xml, nsmgr, "//ns:Receptor/ns:RUTRecep", "//Receptor/RUTRecep"),
            Amount = GetDecimalValue(xml, nsmgr, "//ns:Totales/ns:MntTotal", "//Totales/MntTotal"),
            Date = ParseDate(xml.SelectSingleNode("//ns:IdDoc/ns:FchEmis", nsmgr)?.InnerText),
            ShortUrl = invoiceUrlService.GenerateInvoiceUrl(Guid.NewGuid().ToString()),
            ExpiresAt = DateTime.UtcNow.AddMinutes(2)
        };
        
        await invoiceRepository.CreateAsync(invoice);

        return new UploadInvoiceResponse
        {
            Success = true,
            ShortUrl = invoice.ShortUrl,
            Error = null,
            Issuer = invoice.Issuer,
            Receiver = invoice.Receiver,
            Amount = invoice.Amount,
            Date = invoice.Date,
            ExpiresAt = invoice.ExpiresAt
        };
    }
    
    private string GetXmlNodeValue(XmlDocument xml, XmlNamespaceManager nsmgr, string xpathWithNamespace, string xpathWithoutNamespace)
    {
        return xml.SelectSingleNode(xpathWithNamespace, nsmgr)?.InnerText
            ?? xml.SelectSingleNode(xpathWithoutNamespace)?.InnerText ?? "";
    }

    private decimal GetDecimalValue(XmlDocument xml, XmlNamespaceManager nsmgr, string xpathWithNamespace, string xpathWithoutNamespace)
    {
        var value = xml.SelectSingleNode(xpathWithNamespace, nsmgr)?.InnerText ?? xml.SelectSingleNode(xpathWithoutNamespace)?.InnerText ?? "0";
        return decimal.TryParse(value, out var result) ? result : 0;
    }

    private DateTime ParseDate(string? dateText)
    {
        return DateTime.TryParse(dateText, out var date) ? date : DateTime.Now;
    }

    private UploadInvoiceResponse GenerateErrorResponse(string? errorMessage)
    {
        return new UploadInvoiceResponse
        {
            Success = false,
            ShortUrl = "",
            Error = errorMessage,
            Issuer = "",
            Receiver = "",
            Amount = 0,
            Date = DateTime.MinValue
        };
    }
}