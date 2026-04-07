using PotteryService.Application.Features.Reports.Dtos;

namespace PotteryService.Application.Common.Interfaces;

public interface IReportRepository
{
    Task<IReadOnlyCollection<SalesQuantityReportItemDto>> GetSalesQuantityByDateRangeAsync(
        DateTimeOffset from,
        DateTimeOffset to,
        CancellationToken cancellationToken = default);
}
