using APIKros.Data;
using APIKros.Models;
using APIKros.Repositories;
using APIKros.Requests;
using FluentValidation;

namespace APIKros.Validators;

public abstract class UpdateHierarchyNodeValidator<TRequest, TModel, TRepository>
    : AbstractValidator<TRequest>
    where TRequest : UpdateHierarchyNodeRequest
    where TModel : HierarchyNode, IModel<int>
    where TRepository : IHierarchyNodeRepository<TModel, int>
{
    protected UpdateHierarchyNodeValidator(
        TRepository repository,
        IEmployeeRepository employeeRepository)
    {
        RuleFor(x => x.Id)
            .MustAsync(repository.ExistsAsync)
            .WithMessage($"{typeof(TModel).Name} does not exist.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255)
            .When(x => x.Name is not null);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(async (request, code, ct) =>
                !await repository.CodeExistsAsync(code!, request.Id, ct))
            .WithMessage($"{typeof(TModel).Name} with this Code already exists.")
            .When(x => x.Code is not null);
        
        RuleFor(x => x.ManagerId)
            .MustAsync((managerId, ct) =>
                employeeRepository.ExistsAsync(managerId!.Value, ct))
            .WithMessage("Manager does not exist.")
            .When(x => x.ManagerId.HasValue
            );
    }

}


public class UpdateCompanyRequestValidator
        : UpdateHierarchyNodeValidator<UpdateCompanyRequest, Company, ICompanyRepository>
    {
        public UpdateCompanyRequestValidator(
            ICompanyRepository companyRepository,
            IEmployeeRepository employeeRepository)
            : base(companyRepository, employeeRepository)
        {
        }
    }


public class UpdateDivisionRequestValidator
    : UpdateHierarchyNodeValidator<UpdateDivisionRequest, Division, IDivisionRepository>
{
    public UpdateDivisionRequestValidator(
        IDivisionRepository divisionRepository,
        IEmployeeRepository employeeRepository)
        : base(divisionRepository, employeeRepository)
    {
    }
}

public class UpdateProjectRequestValidator
    : UpdateHierarchyNodeValidator<UpdateProjectRequest, Project, IProjectRepository>
{
    public UpdateProjectRequestValidator(
        IProjectRepository projectRepository,
        IEmployeeRepository employeeRepository)
        : base(projectRepository, employeeRepository)
    {
    }
}

public class UpdateDepartmentRequestValidator
    : UpdateHierarchyNodeValidator<UpdateDepartmentRequest, Department, IDepartmentRepository>
{
    public UpdateDepartmentRequestValidator(
        IDepartmentRepository departmentRepository,
        IEmployeeRepository employeeRepository)
        : base(departmentRepository, employeeRepository)
    {
    }
}