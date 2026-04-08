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
        DateTimeOffset fromDate,
        DateTimeOffset toDate,
        CancellationToken cancellationToken = default)
    {
        var query =
            from sale in _dbContext.Sales.AsNoTracking()
            join saleItem in _dbContext.SaleItems.AsNoTracking() on sale.Id equals saleItem.SaleId
            join product in _dbContext.Products.AsNoTracking() on saleItem.ProductId equals product.Id
            where sale.SaleDate >= fromDate && sale.SaleDate <= toDate
            group saleItem by new { saleItem.ProductId, product.Name } into grouped
            orderby grouped.Sum(x => x.Quantity) descending, grouped.Key.Name
            select new SalesQuantityReportItemDto(
                grouped.Key.ProductId,
                grouped.Key.Name,
                grouped.Sum(x => x.Quantity));

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<ProductRevenueReportDto?> GetProductRevenueByDateRangeAsync(
        string productName,
        DateTimeOffset fromDate,
        DateTimeOffset toDate,
        CancellationToken cancellationToken = default)
    {
        var product = await _dbContext.Products
            .AsNoTracking()
            .Where(x => EF.Functions.ILike(x.Name, productName))
            .Select(x => new { x.Id, x.Name })
            .FirstOrDefaultAsync(cancellationToken);

        if (product == null)
        {
            return null;
        }

        var totalRevenue = await (
            from sale in _dbContext.Sales.AsNoTracking()
            join saleItem in _dbContext.SaleItems.AsNoTracking() on sale.Id equals saleItem.SaleId
            where saleItem.ProductId == product.Id
                && sale.SaleDate >= fromDate
                && sale.SaleDate <= toDate
            select (decimal?)saleItem.LineTotal)
            .SumAsync(cancellationToken) ?? 0m;

        return new ProductRevenueReportDto(
            product.Id,
            product.Name,
            fromDate,
            toDate,
            totalRevenue);
    }
}
