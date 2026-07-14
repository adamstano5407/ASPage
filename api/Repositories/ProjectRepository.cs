using System.Linq.Expressions;
using APIKros.Data;
using APIKros.Models;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Repositories;


public class ProjectRepository : HierarchyNodeRepository<Project, int>, IProjectRepository
{
    protected override string? ParentPropertyName => nameof(Project.DivisionId);
    public ProjectRepository(AppDbContext db) : base(db)
    {
    }
    

    public async Task<Division?> GetParentOfNodeAsync(int id)
    {
        return await DbContext.Projects.Where(p => p.Id == id).Select(p => p.Division).FirstOrDefaultAsync();
    }

    public async Task DeleteAllByParentIdAsync(int parentId)
    {
        await DbContext.Projects.Where(p => p.DivisionId == parentId).ExecuteDeleteAsync();
    }

    public async Task<ICollection<Department>> GetAllChildNodesAsync(int id)
    {
        return await DbContext.Departments.Where(d => d.ProjectId == id).ToListAsync();        
    }
    
}