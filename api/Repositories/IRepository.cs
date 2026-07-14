using APIKros.Models;

namespace APIKros.Repositories;


public interface IRepository<T, TK>
    where T : class, IModel<TK>
    where TK : IEquatable<TK>, IComparable<TK>
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<T?> GetByIdAsync(TK id, CancellationToken cancellationToken = default);

    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(TK id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(TK id, CancellationToken cancellationToken = default);

    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}