using APIKros.Models;
using APIKros.Repositories;
using APIKros.Requests;
using FluentValidation;

namespace APIKros.Validators;

public abstract class CreateNonRootHierarchyNodeValidator<TRequest, TModel, TRepository>
    : CreateHierarchyNodeValidator<TRequest, TModel, int, TRepository>
    where TRequest : CreateNonRootHierarchyNodeRequest
    where TModel : HierarchyNode, IModel<int>
    where TRepository : IHierarchyNodeRepository<TModel, int>
{
    protected CreateNonRootHierarchyNodeValidator(
        IEmployeeRepository employeeRepository,
        TRepository repository,
        string modelName)
        : base(employeeRepository, repository, modelName)
    {
        RuleFor(x => x.ParentId)
            .GreaterThan(0)
            .WithMessage("ParentId is required.");

        RuleFor(x => x.Code)
            .MustAsync(async (request, code, ct) =>
                !await repository.CodeExistsWithinParentAsync(request.ParentId, code!, cancellationToken:ct))
            .WithMessage($"{modelName} with this Code already exists in parent.");
    }
}


public class CreateDivisionRequestValidator
    : CreateNonRootHierarchyNodeValidator<
        CreateDivisionRequest,
        Division,
        IDivisionRepository>
{
    public CreateDivisionRequestValidator(
        IEmployeeRepository employeeRepository,
        IDivisionRepository divisionRepository)
        : base(employeeRepository, divisionRepository, "Division")
    {
    }
}
public class CreateProjectRequestValidator
    : CreateNonRootHierarchyNodeValidator<
        CreateProjectRequest,
        Project,
        IProjectRepository>
{
    public CreateProjectRequestValidator(
        IEmployeeRepository employeeRepository,
        IProjectRepository projectRepository)
        : base(employeeRepository, projectRepository, "Project")
    {
    }
}
    
public class CreateDepartmentRequestValidator
    : CreateNonRootHierarchyNodeValidator<
        CreateDepartmentRequest,
        Department,
        IDepartmentRepository>
{
    public CreateDepartmentRequestValidator(
        IEmployeeRepository employeeRepository,
        IDepartmentRepository departmentRepository)
        : base(employeeRepository, departmentRepository, "Department")
    {
    }
}