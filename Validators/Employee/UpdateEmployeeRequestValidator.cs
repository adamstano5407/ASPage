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

        RuleFor(x => x.Id).MustAsync((id, cancellation) =>
            ValidationUtils.EntityExists<Models.Employee>(
                _context,
                id,
                cancellation
            )).WithMessage("Employee does not exist.");
        
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