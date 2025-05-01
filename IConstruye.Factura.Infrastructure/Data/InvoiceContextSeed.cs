using IConstruye.Factura.Core.Entities;
using Microsoft.Extensions.Logging;

namespace IConstruye.Factura.Infrastructure.Data;

public class InvoiceContextSeed
{
    public static async Task SeedAsync(InvoiceContext invoiceContext, ILoggerFactory loggerFactory, int? retry = 0)
    {
        if (retry != null)
        {
            var retryForAvailability = retry.Value;

            try
            {
                await invoiceContext.Database.EnsureCreatedAsync();

                if (!invoiceContext.Invoices.Any())
                {
                    invoiceContext.Invoices.AddRange(entities: GetInvoices());
                    await invoiceContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 3)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<InvoiceContextSeed>();
                    log.LogError(message: $"Exception occured while connecting: {ex.Message}");
                    await SeedAsync(invoiceContext, loggerFactory, retryForAvailability);
                }
            }
        }
    }
    
    private static Invoice GetInvoices()
    {
        return new Invoice
        {
            Issuer = "Empresa A",
            Receiver = "Cliente B",
            Amount = 100000,
            // Date = DateTime.UtcNow.ToString("s"),
            // ExpiresAt = DateTime.UtcNow.AddMinutes(5).ToString("s"),
            OriginalXml = "<Factura>...</Factura>"
        };
    }
}