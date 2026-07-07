using APIKros.DTOs.Department;
using APIKros.DTOs.Division;
using APIKros.DTOs.Project;
using APIKros.Exceptions;
using APIKros.Models;
using APIKros.Repositories;
using APIKros.Requests;
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

    public ProjectService(
        IProjectRepository projectRepo,
        IEmployeeRepository employeeRepo,
        IValidator<CreateProjectRequest> createValidator,
        IValidator<UpdateProjectRequest> updateValidator,
        IValidator<AssignManagerRequest> assignManagerValidator)
    {
        _projectRepo = projectRepo;
        _employeeRepo = employeeRepo;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _assignManagerValidator = assignManagerValidator;
    }

    public async Task<ProjectDto?> GetAsync(int id)
    {
        var project = await _projectRepo.GetByIdAsync(id);

        return project is null
            ? null
            : ProjectDto.CreateInstance(project);
    }

    public async Task<IEnumerable<ProjectDto>> GetAllAsync()
    {
        var projects = await _projectRepo.GetAllAsync();

        return projects
            .Select(ProjectDto.CreateInstance)
            .ToList();
    }

    public async Task<ProjectDto> CreateAsync(CreateProjectRequest request)
    {
        await _createValidator.ValidateAndThrowAsync(request);

        var project = new Project
        {
            Name = request.Name,
            Code = request.Code,
            DivisionId = request.ParentId
        };

        await _projectRepo.CreateAsync(project);
        await _projectRepo.SaveChangesAsync();

        return ProjectDto.CreateInstance(project);
    }

    public async Task UpdateAsync(int id, UpdateProjectRequest request)
    {
        await _updateValidator.ValidateAndThrowAsync(request);

        var project = await GetRequiredProjectAsync(id);

        if (request.Name is not null)
            project.Name = request.Name;

        if (request.Code is not null)
            project.Code = request.Code;

        if (request.ManagerId.HasValue)
            project.ManagerId = request.ManagerId.Value;

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

        project.ManagerId = null;

        await _projectRepo.UpdateAsync(project);
        await _projectRepo.SaveChangesAsync();
    }

    public async Task AssignManagerAsync(int id, AssignManagerRequest request)
    {
        await _assignManagerValidator.ValidateAndThrowAsync(request);

        var project = await GetRequiredProjectAsync(id);

        var employee = await _employeeRepo.GetByIdAsync(request.EmployeeId);

        if (employee is null)
            throw new NotFoundException();

        project.ManagerId = request.EmployeeId;

        await _projectRepo.UpdateAsync(project);
        await _projectRepo.SaveChangesAsync();
    }

    public async Task<DivisionDto?> GetParentAsync(int id)
    {
        await GetRequiredProjectAsync(id);

        var parent = await _projectRepo.GetParentOfNodeAsync(id);

        return parent is null
            ? null
            : DivisionDto.CreateInstance(parent);
    }

    public async Task<ICollection<DepartmentDto>> GetChildrenAsync(int id)
    {
        await GetRequiredProjectAsync(id);

        var departments = await _projectRepo.GetAllChildNodesAsync(id);

        return departments
            .Select(DepartmentDto.CreateInstance)
            .ToList();
    }

    private async Task<Project> GetRequiredProjectAsync(int id)
    {
        var project = await _projectRepo.GetByIdAsync(id);

        if (project is null)
            throw new NotFoundException();

        return project;
    }
}