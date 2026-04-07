using PotteryService.Domain.Entities;

namespace PotteryService.Application.Common.Interfaces;

public interface ISaleRepository
{
    Task<IReadOnlyCollection<Sale>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Sale> AddAsync(Sale sale, CancellationToken cancellationToken = default);

    Task<Sale?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
}
