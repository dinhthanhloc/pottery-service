using Microsoft.EntityFrameworkCore;
using PotteryService.Application.Common.Interfaces;
using PotteryService.Application.Features.Reports.Dtos;

namespace PotteryService.Infrastructure.Repositories;

public sealed class ReportRepository : IReportRepository
{
    private readonly Persistence.DbContext _dbContext;

    public ReportRepository(Persistence.DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<SalesQuantityReportItemDto>> GetSalesQuantityByDateRangeAsync(
        DateTimeOffset from,
        DateTimeOffset to,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaleItems
            .AsNoTracking()
            .Where(x => x.Sale != null && x.Sale.SaleDate >= from && x.Sale.SaleDate <= to)
            .GroupBy(x => new
            {
                x.ProductId,
                ProductName = x.Product != null ? x.Product.Name : string.Empty
            })
            .Select(group => new SalesQuantityReportItemDto(
                group.Key.ProductId,
                group.Key.ProductName,
                group.Sum(x => x.Quantity)))
            .OrderByDescending(x => x.TotalQuantitySold)
            .ThenBy(x => x.ProductName)
            .ToListAsync(cancellationToken);
    }
}
