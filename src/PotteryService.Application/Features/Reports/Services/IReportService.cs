using PotteryService.Application.Features.Reports.Dtos;
using PotteryService.Application.Features.Reports.Requests;

namespace PotteryService.Application.Features.Reports.Services;

public interface IReportService
{
    Task<IReadOnlyCollection<SalesQuantityReportItemDto>> GetSalesQuantityByDateRangeAsync(
        SalesQuantityReportRequest request,
        CancellationToken cancellationToken = default);
}
