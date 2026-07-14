using System.Linq.Expressions;
using APIKros.Data;
using APIKros.Models;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Repositories;

public class CompanyRepository : HierarchyNodeRepository<Company, int>, ICompanyRepository
{
    
    public CompanyRepository(AppDbContext db) : base(db)
    {
    }
    
    public async Task<ICollection<Division>> GetAllChildNodesAsync(int id)
    {
        return await DbContext.Companies.Where(c => c.Id == id).SelectMany(c => c.Divisions).ToListAsync();
    }
    
}