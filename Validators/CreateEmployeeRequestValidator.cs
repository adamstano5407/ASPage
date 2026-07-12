using APIKros.Repositories;
using APIKros.Requests.Employee;
using FluentValidation;

namespace APIKros.Validators;

public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
{
    public CreateEmployeeRequestValidator(
        IEmployeeRepository employeeRepository,
        ICompanyRepository companyRepository)
    {
        RuleFor(x => x.EmployeeNumber)
            .NotEmpty()
            .WithMessage("EmployeeNumber is required.")
            .MustAsync(async (request, employeeNumber, cancellation) =>
                !await employeeRepository.EmployeeNumberExistsInCompanyAsync(
                    request.CompanyId,
                    employeeNumber,
                    cancellation: cancellation))
            .WithMessage("Employee number already exists in this company.");

        RuleFor(x => x.Title)
            .MaximumLength(80)
            .Must(ValidationUtils.IsAllowedTitle)
            .WithMessage("Title contains invalid value.")
            .When(x => x.Title is not null);

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters.")
            .Matches(@"^[\p{L}\s'-]+$")
            .WithMessage("First name can contain only letters, spaces, hyphens and apostrophes.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.")
            .Matches(@"^[\p{L}\s'-]+$")
            .WithMessage("Last name can contain only letters, spaces, hyphens and apostrophes.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(async (request, email, cancellation) =>
                !await employeeRepository.EmailExistsInCompanyAsync(
                    request.CompanyId,
                    email,
                    cancellation: cancellation))
            .WithMessage("Employee with this Email already exists in this company.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required.")
            .MaximumLength(30).WithMessage("Phone must not exceed 30 characters.")
            .Must(ValidationUtils.IsValidPhone)
            .WithMessage("Phone must contain only digits, spaces, +, - or /.");

        RuleFor(x => x.CompanyId)
            .NotNull().WithMessage("Company id is required.")
            .GreaterThan(0).WithMessage("Company id must be greater than 0.")
            .MustAsync(companyRepository.ExistsAsync)
            .WithMessage("Company does not exist.");
    }
}