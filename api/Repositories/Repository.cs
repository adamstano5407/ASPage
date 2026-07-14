using APIKros.Data;
using APIKros.Models;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Repositories;

public class Repository<T, TK> : IRepository<T, TK>
    where T : class, IModel<TK>
    where TK : IEquatable<TK>, IComparable<TK>
{
    protected readonly AppDbContext DbContext;

    public Repository(AppDbContext db)
    {
        DbContext = db;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>()
            .ToListAsync(cancellationToken);  
    }

    public virtual async Task<T?> GetByIdAsync(TK id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>()
            .FindAsync([id], cancellationToken);
    }

    public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await DbContext.Set<T>()
            .AddAsync(entity, cancellationToken);

        return entity;
    }

    public  virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbContext.Set<T>().Update(entity);
        return Task.CompletedTask;
    }

    public virtual async Task DeleteAsync(TK id, CancellationToken cancellationToken = default)
    {
        var entity = await DbContext.Set<T>()
            .FindAsync([id], cancellationToken);

        if (entity == null)
            return;

        DbContext.Set<T>().Remove(entity);
    }

    public virtual async Task<bool> ExistsAsync(TK id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>()
            .AnyAsync(e => e.Id.Equals(id), cancellationToken);
    }

    public virtual async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}