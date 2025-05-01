using IConstruye.Factura.Core.Entities;
using IConstruye.Factura.Core.Repositories.Base;

namespace IConstruye.Factura.Core.Repositories;

public interface IInvoiceRepository :  IRepository<Invoice>
{
    Task SaveInvoiceAsync(Invoice invoice);
    Task<Invoice> GetInvoiceByUrlAsync(string url);
}