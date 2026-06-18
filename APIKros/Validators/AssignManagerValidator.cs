using APIKros.Data;
using APIKros.Requests;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Validators
{

    public class AssignManagerValidator : AbstractValidator<AssignManagerRequest>
    {
        private readonly AppDbContext _context;
        
        public AssignManagerValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("EmployeeId is required.")
                .MustAsync(async (employeeId, cancellation) =>
                    await _context.Employees.AnyAsync(e => e.Id == employeeId, cancellation))
                .WithMessage("Employee does not exist.");
            
        }
    }    
}

