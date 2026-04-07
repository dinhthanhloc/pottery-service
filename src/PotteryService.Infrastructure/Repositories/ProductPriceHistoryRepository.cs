using PotteryService.Application.Common.Interfaces;
using PotteryService.Domain.Entities;

namespace PotteryService.Infrastructure.Repositories;

public sealed class ProductPriceHistoryRepository : IProductPriceHistoryRepository
{
    private readonly Persistence.DbContext _dbContext;

    public ProductPriceHistoryRepository(Persistence.DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ProductPriceHistory> AddAsync(ProductPriceHistory productPriceHistory, CancellationToken cancellationToken = default)
    {
        await _dbContext.ProductPriceHistories.AddAsync(productPriceHistory, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return productPriceHistory;
    }
}
