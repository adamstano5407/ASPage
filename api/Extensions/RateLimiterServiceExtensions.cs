using Microsoft.AspNetCore.RateLimiting;

namespace ASPage.Extensions;

public static class RateLimiterServiceExtensions
{
    public static IServiceCollection AddRateLimiterCustom(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("default", limiter =>
            {
                limiter.Window = TimeSpan.FromHours(1);
                limiter.PermitLimit = 100;
            });
        });
        
        return services;
    }

}