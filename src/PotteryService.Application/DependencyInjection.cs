using Microsoft.Extensions.DependencyInjection;
using PotteryService.Application.Features.Categories.Services;
using PotteryService.Application.Features.Products.Services;
using PotteryService.Application.Features.Sales.Services;

namespace PotteryService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ISaleService, SaleService>();

        return services;
    }
}
