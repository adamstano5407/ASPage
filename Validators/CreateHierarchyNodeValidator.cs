using APIKros.Data;
using APIKros.Models;
using APIKros.Repositories;
using APIKros.Requests;
using Bogus;
using FluentValidation;

namespace APIKros.Validators;

public abstract class CreateHierarchyNodeValidator<TRequest, TModel, TK, TRepository>
    : AbstractValidator<TRequest>
    where TRequest : CreateHierarchyNodeRequest
    where TModel : HierarchyNode, IModel<TK>
    where TK : IEquatable<TK>, IComparable<TK>
    where TRepository : IHierarchyNodeRepository<TModel, TK>
{
    protected CreateHierarchyNodeValidator(
        IEmployeeRepository employeeRepository,
        TRepository repository,
        string modelName)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(255).WithMessage("Name must not exceed 255 characters.");

        RuleFor(x => x.ManagerId)
            .MustAsync(async (managerId, ct) =>
                await employeeRepository.ExistsAsync(managerId!.Value, ct))
            .WithMessage("Manager does not exist.")
            .When(x => x.ManagerId.HasValue);

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(50).WithMessage("Code must not exceed 50 characters.")
            .MustAsync(async (code, ct) =>
                !await repository.CodeExistsAsync(code!, cancellationToken: ct))
            .WithMessage($"{modelName} with this Code already exists.");
    }
}

public class CreateCompanyValidator
    : CreateHierarchyNodeValidator<CreateCompanyRequest, Company, int, ICompanyRepository>
{
    public CreateCompanyValidator(
        IEmployeeRepository employeeRepository,
        ICompanyRepository companyRepository)
        : base(employeeRepository, companyRepository, "Company")
    {
    }
}