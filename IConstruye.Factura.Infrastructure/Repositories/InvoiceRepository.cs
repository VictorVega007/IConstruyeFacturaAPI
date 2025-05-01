using IConstruye.Factura.Core.Entities;
using IConstruye.Factura.Core.Repositories;
using IConstruye.Factura.Infrastructure.Data;
using IConstruye.Factura.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace IConstruye.Factura.Infrastructure.Repositories;

public class InvoiceRepository(InvoiceContext invoiceContext) : Repository<Invoice>(invoiceContext), IInvoiceRepository
{
    private readonly InvoiceContext _invoiceContext = invoiceContext;

    public async Task SaveInvoiceAsync(Invoice invoice)
    {
        await _invoiceContext.Invoices.AddAsync(invoice);
        await _invoiceContext.SaveChangesAsync();
    }
    
    public async Task<Invoice> GetInvoiceByUrlAsync(string shortUrl) =>
        await _invoiceContext.Invoices.FirstOrDefaultAsync(i => i.ShortUrl == shortUrl) ?? throw new InvalidOperationException($"An error occured at getting the invoice whit url {shortUrl}.");
}