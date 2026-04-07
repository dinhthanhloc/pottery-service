using PotteryService.Application.Features.Products.Dtos;
using PotteryService.Application.Features.Products.Requests;

namespace PotteryService.Application.Features.Products.Services;

public interface IProductService
{
    Task<IReadOnlyCollection<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<ProductDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<ProductDto> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);

    Task<ProductDto> UpdateAsync(long id, UpdateProductRequest request, CancellationToken cancellationToken = default);

    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
