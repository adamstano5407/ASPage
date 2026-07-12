using APIKros.DTOs;
using APIKros.Exceptions;
using APIKros.Models;
using APIKros.Repositories;
using APIKros.Requests;
using AutoMapper;
using FluentValidation;

namespace APIKros.Services;

public interface IProjectService
    : IHierarchyNodeService<ProjectDto, CreateProjectRequest, UpdateProjectRequest, int>,
        IHasParentService<DivisionDto, int>,
        IHasChildrenService<DepartmentDto, int>
{
}

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepo;
    private readonly IEmployeeRepository _employeeRepo;
    private readonly IValidator<CreateProjectRequest> _createValidator;
    private readonly IValidator<UpdateProjectRequest> _updateValidator;
    private readonly IValidator<AssignManagerRequest> _assignManagerValidator;
    private readonly IMapper _mapper;

    public ProjectService(
        IProjectRepository projectRepo,
        IEmployeeRepository employeeRepo,
        IValidator<CreateProjectRequest> createValidator,
        IValidator<UpdateProjectRequest> updateValidator,
        IValidator<AssignManagerRequest> assignManagerValidator, IMapper mapper)
    {
        _projectRepo = projectRepo;
        _employeeRepo = employeeRepo;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _assignManagerValidator = assignManagerValidator;
        _mapper = mapper;
    }

    public async Task<ProjectDto> GetAsync(int id)
    {
        var project = await _projectRepo.GetByIdAsync(id);
        return project == null ? throw new NotFoundException() : _mapper.Map<ProjectDto>(project);
    }

    public async Task<IEnumerable<ProjectDto>> GetAllAsync()
    {
        var projects = await _projectRepo.GetAllAsync();

        return _mapper.Map<IReadOnlyList<ProjectDto>>(projects);
    }

    public async Task<ProjectDto> CreateAsync(CreateProjectRequest request)
    {
        await _createValidator.ValidateAndThrowAsync(request);

        var project = new Project(
            name: request.Name, code: request.Code,
            divisionId: request.ParentId, managerId: request.ManagerId);

        await _projectRepo.CreateAsync(project);
        await _projectRepo.SaveChangesAsync();

        return _mapper.Map<ProjectDto>(project);
    }

    public async Task UpdateAsync(int id, UpdateProjectRequest request)
    {
        request.Id = id;
        await _updateValidator.ValidateAndThrowAsync(request);

        var project = await GetRequiredProjectAsync(id);

        if (request.Name is not null)
            project.ChangeName(request.Name);

        if (request.Code is not null)
            project.ChangeCode(request.Code);

        if (request.ManagerId.HasValue)
            project.AssignManager(request.ManagerId);

        await _projectRepo.UpdateAsync(project);
        await _projectRepo.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await GetRequiredProjectAsync(id);

        await _projectRepo.DeleteAsync(id);
        await _projectRepo.SaveChangesAsync();
    }

    public async Task UnassignManagerAsync(int id)
    {
        var project = await GetRequiredProjectAsync(id);

        project.AssignManager(null);

        await _projectRepo.UpdateAsync(project);
        await _projectRepo.SaveChangesAsync();
    }

    public async Task AssignManagerAsync(int id, AssignManagerRequest request)
    {
        request.NodeId = id;
        await _assignManagerValidator.ValidateAndThrowAsync(request);

        var project = await GetRequiredProjectAsync(id);

        var employee = await _employeeRepo.GetByIdAsync(request.EmployeeId);

        if (employee is null)
            throw new NotFoundException();

        project.AssignManager(request.EmployeeId);

        await _projectRepo.UpdateAsync(project);
        await _projectRepo.SaveChangesAsync();
    }

    public async Task<DivisionDto> GetParentAsync(int id)
    {
        await GetRequiredProjectAsync(id);

        var parent = await _projectRepo.GetParentOfNodeAsync(id);
        return parent is null ? throw new MissingParentException() : _mapper.Map<DivisionDto>(parent);
    }

    public async Task<IEnumerable<DepartmentDto>> GetChildrenAsync(int id)
    {
        await GetRequiredProjectAsync(id);

        var departments = await _projectRepo.GetAllChildNodesAsync(id);

        return _mapper.Map<IReadOnlyList<DepartmentDto>>(departments);
    }

    private async Task<Project> GetRequiredProjectAsync(int id)
    {
        var project = await _projectRepo.GetByIdAsync(id);

        return project ?? throw new NotFoundException();
    }
}