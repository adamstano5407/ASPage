using APIKros.DTOs;
using APIKros.Exceptions;
using APIKros.Models;
using APIKros.Repositories;
using APIKros.Requests;
using AutoMapper;
using FluentValidation;

namespace APIKros.Services;

public interface IDepartmentService
    : IHierarchyNodeService<DepartmentDto, CreateDepartmentRequest, UpdateDepartmentRequest, int>,
      IHasParentService<ProjectDto, int>
{
}

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepo;
    private readonly IProjectRepository _projectRepo;
    private readonly IEmployeeRepository _employeeRepo;
    private readonly IValidator<CreateDepartmentRequest> _createValidator;
    private readonly IValidator<UpdateDepartmentRequest> _updateValidator;
    private readonly IValidator<AssignManagerRequest> _assignManagerValidator;
    private readonly IMapper _mapper;

    public DepartmentService(
        IDepartmentRepository departmentRepo,
        IProjectRepository projectRepo,
        IEmployeeRepository employeeRepo,
        IValidator<CreateDepartmentRequest> createValidator,
        IValidator<UpdateDepartmentRequest> updateValidator,
        IValidator<AssignManagerRequest> assignManagerValidator, IMapper mapper)
    {
        _departmentRepo = departmentRepo;
        _projectRepo = projectRepo;
        _employeeRepo = employeeRepo;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _assignManagerValidator = assignManagerValidator;
        _mapper = mapper;
    }

    public async Task<DepartmentDto> GetAsync(int id)
    {
        var department = await _departmentRepo.GetByIdAsync(id);
        if (department == null)
        {
            throw new  NotFoundException();
        }
        return _mapper.Map<DepartmentDto>(department);
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        var departments = await _departmentRepo.GetAllAsync();

        return _mapper.Map<IReadOnlyList<DepartmentDto>>(departments);
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentRequest request)
    {
        await _createValidator.ValidateAndThrowAsync(request);

        var project = await _projectRepo.GetByIdAsync(request.ParentId);

        if (project is null)
            throw new NotFoundException();

        var department = new Department
       (
            request.Name,
            request.Code,
            projectId: request.ParentId, 
            managerId: request.ManagerId
       );

        await _departmentRepo.CreateAsync(department);
        await _departmentRepo.SaveChangesAsync();

        return _mapper.Map<DepartmentDto>(department);
    }

    public async Task UpdateAsync(int id, UpdateDepartmentRequest request)
    {
        request.Id = id;
        await _updateValidator.ValidateAndThrowAsync(request);

        var department = await GetRequiredDepartmentAsync(id);

        if (request.Name is not null)
            department.ChangeName(request.Name);

        if (request.Code is not null)
            department.ChangeCode(request.Code);

        if (request.ManagerId.HasValue)
            department.AssignManager(request.ManagerId.Value);

        await _departmentRepo.UpdateAsync(department);
        await _departmentRepo.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await GetRequiredDepartmentAsync(id);

        await _departmentRepo.DeleteAsync(id);
        await _departmentRepo.SaveChangesAsync();
    }

    public async Task UnassignManagerAsync(int id)
    {
        var department = await GetRequiredDepartmentAsync(id);

        department.AssignManager(null);

        await _departmentRepo.UpdateAsync(department);
        await _departmentRepo.SaveChangesAsync();
    }

    public async Task AssignManagerAsync(int id, AssignManagerRequest request)
    {
        request.NodeId = id;
        await _assignManagerValidator.ValidateAndThrowAsync(request);

        var department = await GetRequiredDepartmentAsync(id);

        var employee = await _employeeRepo.GetByIdAsync(request.EmployeeId);

        if (employee is null)
            throw new NotFoundException();

        department.AssignManager(request.EmployeeId);

        await _departmentRepo.UpdateAsync(department);
        await _departmentRepo.SaveChangesAsync();
    }

    public async Task<ProjectDto> GetParentAsync(int id)
    {
        await GetRequiredDepartmentAsync(id);

        var project = await _departmentRepo.GetParentOfNodeAsync(id);
        return project is null ? throw new MissingParentException() : _mapper.Map<ProjectDto>(project);
    }

    private async Task<Department> GetRequiredDepartmentAsync(int id)
    {
        var department = await _departmentRepo.GetByIdAsync(id);

        if (department is null)
            throw new NotFoundException();

        return department;
    }
}