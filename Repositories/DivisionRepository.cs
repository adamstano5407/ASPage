using System.Linq.Expressions;
using APIKros.Data;
using APIKros.Models;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Repositories;

public class DivisionRepository : HierarchyNodeRepository<Division, int>, IDivisionRepository
{
    protected override string? ParentPropertyName => nameof(Division.CompanyId);
    public DivisionRepository(AppDbContext db) : base(db)
    {
    }

  

    public async Task<Company?> GetParentOfNodeAsync(int id)
    {
        return await DbContext.Divisions.Where(d => d.Id == id).Select(d => d.Company).FirstOrDefaultAsync();
    }

    public async Task DeleteAllByParentIdAsync(int parentId)
    { 
        await DbContext.Divisions.Where(d => d.CompanyId == parentId).ExecuteDeleteAsync();
    }

    public async Task<ICollection<Project>> GetAllChildNodesAsync(int id)
    {
        return await DbContext.Projects.Where(p => p.DivisionId == id).ToListAsync();
    }
    
}