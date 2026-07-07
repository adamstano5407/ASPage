namespace APIKros.Extensions;
using AutoMapper;


public static class AutoMapperServiceCollectionExtensions
{
    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(_ => { }, typeof(Program));
        return services;
    }
}