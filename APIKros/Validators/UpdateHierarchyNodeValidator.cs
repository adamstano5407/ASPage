using APIKros.Data;
using APIKros.Models;
using APIKros.Requests;
using FluentValidation;

namespace APIKros.Validators
{
    public abstract class UpdateHierarchyNodeValidator<TRequest, TModel>
        : AbstractValidator<TRequest>
        where TRequest : CreateHierarchyNodeRequest
        where TModel : HierarchyNode
    {
        protected readonly AppDbContext _context;

        protected UpdateHierarchyNodeValidator(AppDbContext context)
        {
            _context = context;

        }
    }
}