using PotteryService.Domain.Entities;

namespace PotteryService.Application.Common.Interfaces;

public interface IProductRepository
{
    Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Product?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Product>> GetByIdsAsync(IEnumerable<long> ids, CancellationToken cancellationToken = default);

    Task<Product?> GetByIdForUpdateAsync(long id, CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(string name, long? excludeId = null, CancellationToken cancellationToken = default);

    Task<bool> ExistsBySkuAsync(string sku, long? excludeId = null, CancellationToken cancellationToken = default);

    Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default);

    Task UpdateAsync(Product product, CancellationToken cancellationToken = default);

    Task DeleteAsync(Product product, CancellationToken cancellationToken = default);
}
