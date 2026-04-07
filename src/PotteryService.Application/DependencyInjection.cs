using Microsoft.Extensions.DependencyInjection;
using PotteryService.Application.Features.Categories.Services;
using PotteryService.Application.Features.Products.Services;

namespace PotteryService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}
