using APIKros.Data;
using APIKros.Models;
using APIKros.Requests;
using FluentValidation;

namespace APIKros.Validators;

public abstract class UpdateHierarchyNodeValidator<TRequest, TModel>
    : AbstractValidator<TRequest>
    where TRequest : UpdateHierarchyNodeRequest
    where TModel : HierarchyNode
{
    protected readonly AppDbContext Context;

    protected UpdateHierarchyNodeValidator(AppDbContext context)
    {
        Context = context;

        RuleFor(x => x.Id)
            .MustAsync((id, ct) =>
                ValidationUtils.EntityExists<TModel>(Context, id, ct))
            .WithMessage($"{typeof(TModel).Name} does not exist.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255)
            .When(x => x.Name is not null);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50)
            .When(x => x.Code is not null);

        RuleFor(x => x.ManagerId)
            .MustAsync((managerId, ct) =>
                ValidationUtils.EntityExists<Models.Employee>(
                    Context,
                    managerId!.Value,
                    ct))
            .WithMessage("Manager does not exist.")
            .When(x => x.ManagerId.HasValue);
    }
}