using Microsoft.OpenApi;

namespace ASPage.Extensions;

public static class OpenApiServiceCollectionExtensions
{
    public static IServiceCollection AddOpenApiWithServerUrl(
        this IServiceCollection services,
        IHostEnvironment environment,
        string documentName = "v1")
    {
        var serverUrl = environment.IsDevelopment()
            ? "http://localhost"
            : null;

        if (serverUrl is null)
        {
            return services;
        }
        
        services.AddOpenApi(documentName, options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Servers = new List<OpenApiServer>
                {
                    new OpenApiServer { Url = serverUrl }
                };
                return Task.CompletedTask;
            });
        });

        return services;
    }
}