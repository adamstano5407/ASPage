using APIKros.DTOs;
using APIKros.Exceptions;
using APIKros.Models;
using APIKros.Repositories;
using APIKros.Requests;
using AutoMapper;
using FluentValidation;

namespace APIKros.Services;

public interface IDivisionService : IHierarchyNodeService<DivisionDto, CreateDivisionRequest, UpdateDivisionRequest, int>, IHasChildrenService<ProjectDto, int>, IHasParentService<CompanyDto, int>
{
}

public class DivisionService : IDivisionService
{
    private readonly IDivisionRepository _divisionRepo;
    private readonly ICompanyRepository _companyRepo;
    private readonly IEmployeeRepository _employeeRepo;
    private readonly IValidator<CreateDivisionRequest> _createValidator;
    private readonly IValidator<UpdateDivisionRequest> _updateValidator;
    private readonly IValidator<AssignManagerRequest> _assignManagerValidator;
    private readonly IMapper _mapper;
    
    public DivisionService(
        IDivisionRepository divisionRepo,
        ICompanyRepository companyRepo,
        IEmployeeRepository employeeRepo,
        IValidator<CreateDivisionRequest> createValidator,
        IValidator<UpdateDivisionRequest> updateValidator,
        IValidator<AssignManagerRequest> assignManagerValidator, IMapper mapper)
    {
        _divisionRepo = divisionRepo;
        _companyRepo = companyRepo;
        _employeeRepo = employeeRepo;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _assignManagerValidator = assignManagerValidator;
        _mapper = mapper;
    }
    
    public async Task<DivisionDto> GetAsync(int id)
    {
        var division = await _divisionRepo.GetByIdAsync(id);
        return division is null ? throw new NotFoundException() : _mapper.Map<DivisionDto>(division);
    }

    public async Task<IEnumerable<DivisionDto>> GetAllAsync()
    {
        var divisions = await _divisionRepo.GetAllAsync();

        return _mapper.Map<IReadOnlyList<DivisionDto>>(divisions);
    }

    public async Task<DivisionDto> CreateAsync(CreateDivisionRequest request)
    {
        await _createValidator.ValidateAndThrowAsync(request);

        var company = await _companyRepo.GetByIdAsync(request.ParentId);

        if (company is null)
            throw new NotFoundException();

        var division = new Division(
           request.Name,
           request.Code,
           companyId : request.ParentId,
           managerId: request.ManagerId
        );

        await _divisionRepo.CreateAsync(division);
        await _divisionRepo.SaveChangesAsync();
        return _mapper.Map<DivisionDto>(division);
    }

    public async Task UpdateAsync(int id, UpdateDivisionRequest request)
    {
        request.Id = id;
        await _updateValidator.ValidateAndThrowAsync(request);

        var division = await _divisionRepo.GetByIdAsync(id);

        if (division is null)
            throw new NotFoundException();

        if (request.Name is not null)
            division.ChangeName(request.Name);

        if (request.Code is not null)
            division.ChangeCode(request.Code);

        if (request.ManagerId.HasValue)
            division.AssignManager(request.ManagerId.Value);

        await _divisionRepo.UpdateAsync(division);
        await _divisionRepo.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var division = await _divisionRepo.GetByIdAsync(id);

        if (division is null)
            throw new NotFoundException();
        
        await _divisionRepo.DeleteAsync(id);
        await _divisionRepo.SaveChangesAsync();
    }

    public async Task UnassignManagerAsync(int id)
    {
        var division = await _divisionRepo.GetByIdAsync(id);

        if (division is null)
            throw new NotFoundException();

        division.AssignManager(null);

        await _divisionRepo.UpdateAsync(division);
        await _divisionRepo.SaveChangesAsync();
    }

    public async Task AssignManagerAsync(int id, AssignManagerRequest request)
    {
        request.NodeId = id;
        await _assignManagerValidator.ValidateAndThrowAsync(request);

        var division = await _divisionRepo.GetByIdAsync(id);

        if (division is null)
            throw new NotFoundException();

        var employee = await _employeeRepo.GetByIdAsync(request.EmployeeId);

        if (employee is null)
            throw new NotFoundException();

        division.AssignManager(request.EmployeeId); 

        await _divisionRepo.UpdateAsync(division);
        await _divisionRepo.SaveChangesAsync();
        
    }

    public async Task<IEnumerable<ProjectDto>> GetChildrenAsync(int id)
    {
        var division = await _divisionRepo.GetByIdAsync(id);

        if (division is null)
            throw new NotFoundException();

        var projects = await _divisionRepo.GetAllChildNodesAsync(id);

        return _mapper.Map<IReadOnlyList<ProjectDto>>(projects);
    }

    public async Task<CompanyDto> GetParentAsync(int id)
    {
        var division = await _divisionRepo.GetByIdAsync(id);

        if (division is null)
            throw new MissingParentException();

        var company = await _divisionRepo.GetParentOfNodeAsync(id);

        return _mapper.Map<CompanyDto>(company);
    }
}