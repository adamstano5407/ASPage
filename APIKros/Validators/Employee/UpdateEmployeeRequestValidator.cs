using APIKros.Data;
using APIKros.Requests.Employee;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Validators.Employee;

public class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
{
    private readonly AppDbContext _context;

    public UpdateEmployeeRequestValidator(AppDbContext context)
    {
        _context = context;
        
        
        
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
            .EmailAddress().WithMessage("Email has invalid format.")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.CompanyId)
            .MustAsync((companyId, cancellation) =>
                ValidationUtils.EntityExists<Models.Company>(
                    _context,
                    companyId!.Value,
                    cancellation))
            .WithMessage("Company does not exist.")
            .When(x => x.CompanyId.HasValue);
    }
}