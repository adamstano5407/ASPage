using APIKros.Data;
using APIKros.Models;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Repositories;

public interface IEmployeeRepository : IRepository<Employee, int>
{
    Task UnassignEmployeeFromLeadershipPositionsAsync(int employeeId);
    
    Task DeleteEmployeesFromCompany(int companyId);
    
    Task<IEnumerable<Employee>> GetEmployeesByCompanyId(int companyId);
}

public class EmployeeRepository : Repository<Employee, int>, IEmployeeRepository
{
    public EmployeeRepository(AppDbContext db) : base(db)
    {
    }

    public async Task UnassignEmployeeFromLeadershipPositionsAsync(int employeeId)
    {
        await DbContext.Companies
            .Where(c => c.ManagerId == employeeId)
            .ExecuteUpdateAsync(s => s.SetProperty(c => c.ManagerId, (int?)null));

        await DbContext.Divisions
            .Where(d => d.ManagerId == employeeId)
            .ExecuteUpdateAsync(s => s.SetProperty(d => d.ManagerId, (int?)null));

        await DbContext.Projects
            .Where(p => p.ManagerId == employeeId)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.ManagerId, (int?)null));

        await DbContext.Departments
            .Where(d => d.ManagerId == employeeId)
            .ExecuteUpdateAsync(s => s.SetProperty(d => d.ManagerId, (int?)null));
    }

    public async Task DeleteEmployeesFromCompany(int companyId)
    {
        await DbContext.Employees.Where(e => e.CompanyId == companyId).ExecuteDeleteAsync();
    }

    public async Task<IEnumerable<Employee>> GetEmployeesByCompanyId(int companyId)
    {
        var employees = await DbContext.Employees.Where(e => e.CompanyId == companyId).ToListAsync();
        return employees;
    }
}