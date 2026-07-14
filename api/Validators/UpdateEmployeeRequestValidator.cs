using APIKros.Data;
using APIKros.Repositories;
using APIKros.Requests.Employee;
using FluentValidation;

namespace APIKros.Validators;

public class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
{
    
    public UpdateEmployeeRequestValidator(IEmployeeRepository employeeRepository, ICompanyRepository companyRepository)
    {

        RuleFor(x => x.Id).MustAsync(employeeRepository.ExistsAsync).WithMessage("Employee does not exist.");
        
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100)
            .Matches(@"^[\p{L}\s'-]+$")
            .When(x => x.FirstName is not null);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100)
            .Matches(@"^[\p{L}\s'-]+$")
            .When(x => x.LastName is not null);

        RuleFor(x => x.EmployeeNumber)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(async (request, employeeNumber, cancellationToken) =>
                !await employeeRepository.EmployeeNumberExistsInCompanyAsync(
                    request.CompanyId!.Value,
                    employeeNumber,
                    request.Id,
                    cancellationToken))
            .WithMessage("Employee number already exists in this company.")
            .When(x => !string.IsNullOrWhiteSpace(x.EmployeeNumber));
        
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email has invalid format.")
            .MustAsync(async (request, email, cancellationToken) =>
                !await employeeRepository.EmailExistsInCompanyAsync(
                    request.CompanyId!.Value,
                    email,
                    request.Id,
                    cancellationToken))
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.CompanyId)
            .MustAsync(async (companyId, ct) =>
                !companyId.HasValue || await companyRepository.ExistsAsync(companyId.Value, ct))
            .WithMessage("Company does not exist.");
    }
}