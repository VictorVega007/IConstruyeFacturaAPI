using IConstruye.Factura.Core.Entities;
using IConstruye.Factura.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace IConstruye.Factura.Infrastructure.Data;

public class InvoiceContext : DbContext
{
    public InvoiceContext(DbContextOptions<InvoiceContext> options) : base(options)
    {
        
    }
    public DbSet<Invoice> Invoices { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Ignore<QueryCounter>();
    }
}