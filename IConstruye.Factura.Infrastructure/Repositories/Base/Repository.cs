using IConstruye.Factura.Core.Entities.Base;
using IConstruye.Factura.Core.Repositories.Base;
using IConstruye.Factura.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IConstruye.Factura.Infrastructure.Repositories.Base;

public class Repository<T>(InvoiceContext invoiceContext)  : IRepository<T> 
    where T : Entity
{
    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await invoiceContext.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await invoiceContext.Set<T>().FindAsync(id);
    }

    public async Task<T> CreateAsync(T entity)
    {
        await invoiceContext.Set<T>().AddAsync(entity);
        await invoiceContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        invoiceContext.Entry(entity).State = EntityState.Modified;
        await invoiceContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        invoiceContext.Set<T>().Remove(entity);
        await invoiceContext.SaveChangesAsync();
    }
    
    public async Task SaveChangesAsync()
    {
        await invoiceContext.SaveChangesAsync();
    }
}