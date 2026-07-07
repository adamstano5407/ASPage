using APIKros.DTOs.Company;
using APIKros.DTOs.Division;
using APIKros.DTOs.Employee;
using APIKros.Exceptions;
using APIKros.Models;
using APIKros.Repositories;
using APIKros.Requests;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Services;


public interface ICompanyService : IHierarchyNodeService<CompanyDto, CreateCompanyRequest, UpdateCompanyRequest, int>, IHasChildrenService<DivisionDto, int>
{
    public Task<IEnumerable<EmployeeDto>> GetAllEmployees(int companyId);
}

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _repo;
    private readonly IEmployeeRepository _empRepo;
    private readonly IValidator<CreateCompanyRequest> _createValidator;
    private readonly IValidator<UpdateCompanyRequest> _updateValidator;
    private readonly IValidator<AssignManagerRequest> _assignManagerValidator;

    public CompanyService(
        ICompanyRepository repo,
        IEmployeeRepository empRepo,
        IValidator<CreateCompanyRequest> createValidator,
        IValidator<UpdateCompanyRequest> updateValidator,
        IValidator<AssignManagerRequest> assignManagerValidator
        )
    {
        _repo = repo;
        _empRepo = empRepo;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _assignManagerValidator = assignManagerValidator;
    }
    
    public async Task<CompanyDto?> GetAsync(int id)
    {
        var company = await _repo.GetByIdAsync(id);

        return company is null ? throw new NotFoundException() : CompanyDto.CreateInstance(company);
    }

    public async Task<IEnumerable<CompanyDto>> GetAllAsync()
    {
        var companies = await _repo.GetAllAsync();

        return companies
            .Select(CompanyDto.CreateInstance)
            .ToList();
    }

    public async Task<CompanyDto> CreateAsync(CreateCompanyRequest request)
    {
        await _createValidator.ValidateAndThrowAsync(request);
        
        var company = new Company
        {
            Name = request.Name,
            Code = request.Code,
            ManagerId = request.ManagerId
        };
       
        if (request.ManagerId.HasValue)
        {
            company.ManagerId = request.ManagerId.Value;
        }
        
        await _repo.CreateAsync(company);
        await _repo.SaveChangesAsync();

        return CompanyDto.CreateInstance(company);
    }

    public async Task UpdateAsync(int id, UpdateCompanyRequest request)
    {
        request.Id = id;
        

        await _updateValidator.ValidateAndThrowAsync(request);

        var company = await _repo.GetByIdAsync(id)
                      ?? throw new NotFoundException();
        
        if (request.ManagerId.HasValue)
        {
            company.ManagerId = request.ManagerId.Value;
        }

        if (request.Name is not null)
        {
            company.Name = request.Name;
        }

        if (request.Code is not null)
        {
            company.Code = request.Code;
        }

        await _repo.UpdateAsync(company);
        await _repo.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var exists = await _repo.ExistsAsync(id);

        if (!exists)
        {
            throw new NotFoundException();
        }

        await _empRepo.DeleteEmployeesFromCompany(id);

        await _repo.DeleteAsync(id);
        await _repo.SaveChangesAsync();
    }

    public async Task UnassignManagerAsync(int id)
    {
        await _repo.UnassignManagerAsync(id);
        await _repo.SaveChangesAsync();
    }

    public async Task AssignManagerAsync(int id, AssignManagerRequest request)
    {
        request.NodeId = id;
        await _assignManagerValidator.ValidateAndThrowAsync(request);
        var company = await _repo.GetByIdAsync(id) ?? throw  new NotFoundException();
        
        company.ManagerId = request.EmployeeId;
        await _repo.UpdateAsync(company);
        await _repo.SaveChangesAsync();
        
    }

    public async Task<ICollection<DivisionDto>> GetChildrenAsync(int id)
    {
        var divisions = await _repo.GetAllChildNodesAsync(id);

        return divisions
            .Select(DivisionDto.CreateInstance)
            .ToList();
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllEmployees(int companyId)
    {
        var employees = await _empRepo.GetEmployeesByCompanyId(companyId);
        return employees.Select(EmployeeDto.CreateInstance).ToList(); 
    }
}