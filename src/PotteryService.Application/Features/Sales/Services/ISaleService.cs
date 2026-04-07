using PotteryService.Application.Features.Sales.Dtos;
using PotteryService.Application.Features.Sales.Requests;

namespace PotteryService.Application.Features.Sales.Services;

public interface ISaleService
{
    Task<IReadOnlyCollection<SaleDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<SaleDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<SaleDto> CreateAsync(CreateSaleRequest request, CancellationToken cancellationToken = default);
}
