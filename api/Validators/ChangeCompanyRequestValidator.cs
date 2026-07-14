using APIKros.Repositories;
using APIKros.Requests.Employee;
using FluentValidation;

namespace APIKros.Validators;

public class ChangeCompanyRequestValidator : AbstractValidator<ChangeCompanyRequest>
{
    public ChangeCompanyRequestValidator(
        IEmployeeRepository employeeRepository,
        ICompanyRepository companyRepository)
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0)
            .WithMessage("EmployeeId is required.")
            .MustAsync(employeeRepository.ExistsAsync)
            .WithMessage("Employee does not exist.");

        RuleFor(x => x.NewCompanyId)
            .GreaterThan(0)
            .WithMessage("NewCompanyId is required.")
            .MustAsync(companyRepository.ExistsAsync)
            .WithMessage("Company does not exist.");
    }
}