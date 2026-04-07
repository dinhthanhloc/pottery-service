using PotteryService.Domain.Entities;

namespace PotteryService.Application.Common.Interfaces;

public interface IProductPriceHistoryRepository
{
    Task<ProductPriceHistory> AddAsync(ProductPriceHistory productPriceHistory, CancellationToken cancellationToken = default);
}
