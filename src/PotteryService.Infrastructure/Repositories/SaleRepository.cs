using Microsoft.EntityFrameworkCore;
using PotteryService.Application.Common.Interfaces;
using PotteryService.Domain.Entities;

namespace PotteryService.Infrastructure.Repositories;

public sealed class SaleRepository : ISaleRepository
{
    private readonly Persistence.DbContext _dbContext;

    public SaleRepository(Persistence.DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<Sale>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Sales
            .AsNoTracking()
            .Include(x => x.SaleItems)
                .ThenInclude(x => x.Product)
            .OrderByDescending(x => x.SaleDate)
            .ThenByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Sale> AddAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _dbContext.Sales.AddAsync(sale, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return sale;
    }

    public async Task<Sale?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Sales
            .AsNoTracking()
            .Include(x => x.SaleItems)
                .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
