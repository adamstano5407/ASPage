using APIKros.DTOs;
using APIKros.Exceptions;
using APIKros.Models;
using APIKros.Repositories;
using APIKros.Requests;
using AutoMapper;
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
    private readonly IMapper _mapper;
    public CompanyService(
        ICompanyRepository repo,
        IEmployeeRepository empRepo,
        IValidator<CreateCompanyRequest> createValidator,
        IValidator<UpdateCompanyRequest> updateValidator,
        IValidator<AssignManagerRequest> assignManagerValidator, IMapper mapper)
    {
        _repo = repo;
        _empRepo = empRepo;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _assignManagerValidator = assignManagerValidator;
        _mapper = mapper;
    }
    
    public async Task<CompanyDto> GetAsync(int id)
    {
        var company = await _repo.GetByIdAsync(id);

        return company is null ? throw new NotFoundException() : _mapper.Map<CompanyDto>(company);
    }

    public async Task<IEnumerable<CompanyDto>> GetAllAsync()
    {
        var companies = await _repo.GetAllAsync();
        
        return _mapper.Map<IReadOnlyList<CompanyDto>>(companies);
    }

    public async Task<CompanyDto> CreateAsync(CreateCompanyRequest request)
    {
        await _createValidator.ValidateAndThrowAsync(request);
        
        var company = new Company(
            request.Name,
            request.Code,
            request.ManagerId);
       
        if (request.ManagerId.HasValue)
        {
            company.AssignManager(request.ManagerId.Value);
        }
        
        await _repo.CreateAsync(company);
        await _repo.SaveChangesAsync();

        return _mapper.Map<CompanyDto>(company);
    }

    public async Task UpdateAsync(int id, UpdateCompanyRequest request)
    {
        request.Id = id;
        

        await _updateValidator.ValidateAndThrowAsync(request);

        var company = await _repo.GetByIdAsync(id)
                      ?? throw new NotFoundException();
        
        if (request.ManagerId.HasValue)
        {
            company.AssignManager(request.ManagerId.Value);
        }

        if (request.Name is not null)
        {
            company.ChangeName(request.Name);
        }

        if (request.Code is not null)
        {
            company.ChangeCode(request.Code);
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
        
        company.AssignManager(request.EmployeeId);
        await _repo.UpdateAsync(company);
        await _repo.SaveChangesAsync();
        
    }

    public async Task<IEnumerable<DivisionDto>> GetChildrenAsync(int id)
    {
        var divisions = await _repo.GetAllChildNodesAsync(id);

        return _mapper.Map<IReadOnlyList<DivisionDto>>(divisions);
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllEmployees(int companyId)
    {
        var employees = await _empRepo.GetEmployeesByCompanyId(companyId);
        return _mapper.Map<IReadOnlyList<EmployeeDto>>(employees);
    }
}