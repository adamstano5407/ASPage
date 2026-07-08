using System.Linq.Expressions;
using APIKros.Data;
using APIKros.Models;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Repositories;

public class DepartmentRepository : HierarchyNodeRepository<Department, int>, IDepartmentRepository
{
    protected override string? ParentPropertyName => nameof(Department.ProjectId);
    public DepartmentRepository(AppDbContext db) : base(db)
    {
    }
    

    public async Task<Project?> GetParentOfNodeAsync(int id)
    {
        return await  DbContext.Departments.Where(d => d.Id == id).Select(d => d.Project).FirstOrDefaultAsync();
    }

    public async Task DeleteAllByParentIdAsync(int parentId)
    {
        await DbContext.Departments.Where(d => d.ProjectId == parentId).ExecuteDeleteAsync();
    }
}