using APIKros.Data;
using APIKros.Repositories;
using APIKros.Requests;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace APIKros.Validators
{

    public class AssignManagerValidator : AbstractValidator<AssignManagerRequest>
    {
        
        public AssignManagerValidator(IEmployeeRepository employeeRepository)
        {

            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("EmployeeId is required.")
                .MustAsync(async (employeeId, cancellation) =>
                    await employeeRepository.ExistsAsync(employeeId, cancellation))
                .WithMessage("Employee does not exist.");
            
        }
    }    
}

