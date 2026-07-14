using System.Linq.Expressions;
using APIKros.Data;
using APIKros.Models;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Repositories;

public abstract class HierarchyNodeRepository<T, TK>
    : Repository<T, TK>, IHierarchyNodeRepository<T, TK>
    where T : HierarchyNode, IModel<TK>
    where TK : IEquatable<TK>, IComparable<TK>
{
    protected virtual string? ParentPropertyName => null;

    protected HierarchyNodeRepository(AppDbContext db)
        : base(db)
    {
    }

    public override async Task<IEnumerable<T>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>()
            .Include(x => x.Manager)
            .ToListAsync(cancellationToken);
    }

    public override async Task<T?> GetByIdAsync(
        TK id,
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>()
            .Include(x => x.Manager)
            .FirstOrDefaultAsync(x => x.Id!.Equals(id), cancellationToken);
    }

    public async Task<bool> CodeExistsAsync(
        string code,
        TK? excludeId = default,
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>()
            .AnyAsync(x =>
                    x.Code == code &&
                    (excludeId == null || !x.Id.Equals(excludeId)),
                cancellationToken);
    }

    public virtual async Task UnassignManagerAsync(TK id)
    {
        await DbContext.Set<T>().AsNoTracking().Where(x => x.Id.Equals(id))
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.ManagerId, _ => null));
    }

    public virtual async Task<Employee?> GetManagerOfNodeAsync(TK id)
    {
        return await DbContext.Set<T>().AsNoTracking().Where(x => x.Id.Equals(id)).Select(x => x.Manager)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> CodeExistsWithinParentAsync(
        TK parentId,
        string code,
        TK? excludeId = default,
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>()
            .AnyAsync(x =>
                    EF.Property<TK>(x, ParentPropertyName!)!.Equals(parentId) &&
                    x.Code == code &&
                    (excludeId == null || !x.Id.Equals(excludeId)),
                cancellationToken);
    }
}