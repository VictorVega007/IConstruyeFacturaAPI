using IConstruye.Factura.Core.Entities.Base;

namespace IConstruye.Factura.Core.Repositories.Base;

public interface IRepository<T> where T : Entity
{
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task SaveChangesAsync();
}