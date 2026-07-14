using APIKros.Validators;
using FluentValidation;

namespace APIKros.Extensions;

public static class ValidationServiceCollectionExtensions
{
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AssignManagerValidator>(
            ServiceLifetime.Scoped);
        return services;
    }
}