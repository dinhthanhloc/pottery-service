using PotteryService.Application.Common.Interfaces;
using PotteryService.Application.Features.Reports.Dtos;
using PotteryService.Application.Features.Reports.Requests;

namespace PotteryService.Application.Features.Reports.Services;

public sealed class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;

    public ReportService(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<IReadOnlyCollection<SalesQuantityReportItemDto>> GetSalesQuantityByDateRangeAsync(
        SalesQuantityReportRequest request,
        CancellationToken cancellationToken = default)
    {
        var from = request.From.ToUniversalTime();
        var to = request.To.ToUniversalTime();

        if (from > to)
        {
            throw new ArgumentException("'From' must be less than or equal to 'To'.");
        }

        return await _reportRepository.GetSalesQuantityByDateRangeAsync(from, to, cancellationToken);
    }
}
