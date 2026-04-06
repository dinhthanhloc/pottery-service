using Microsoft.Extensions.DependencyInjection;
using PotteryService.Application.Categories.Services;

namespace PotteryService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();

        return services;
    }
}
