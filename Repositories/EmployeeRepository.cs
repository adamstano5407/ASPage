using APIKros.Data;
using APIKros.Models;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Repositories;

public interface IEmployeeRepository : IRepository<Employee, int>
{
    Task UnassignEmployeeFromLeadershipPositionsAsync(int employeeId);
    
    Task DeleteEmployeesFromCompany(int companyId);
    
    Task<IEnumerable<Employee>> GetEmployeesByCompanyId(int companyId);
    
    Task<bool> EmailExistsInCompanyAsync(int companyId, string email, CancellationToken cancellation = default);
    
    Task<bool> EmployeeNumberExistsInCompanyAsync(int companyId, string employeeNumber, CancellationToken cancellation = default);
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
        await SaveChangesAsync();
    }

    public async Task<IEnumerable<Employee>> GetEmployeesByCompanyId(int companyId)
    {
        var employees = await DbContext.Employees.Where(e => e.CompanyId == companyId).ToListAsync();
        return employees;
    }

    public async Task<bool> EmailExistsInCompanyAsync(
        int companyId,
        string email,
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Employees.AnyAsync(
            e => e.CompanyId == companyId &&
                 e.Email == email,
            cancellationToken);
    }

    public async Task<bool> EmployeeNumberExistsInCompanyAsync(
        int companyId,
        string employeeNumber,
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Employees.AnyAsync(
            e => e.CompanyId == companyId &&
                 e.EmployeeNumber == employeeNumber,
            cancellationToken);
    }
}