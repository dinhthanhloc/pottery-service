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
        var (from, to) = NormalizeDateRange(request.From, request.To);

        return await _reportRepository.GetSalesQuantityByDateRangeAsync(from, to, cancellationToken);
    }

    public async Task<ProductRevenueReportDto> GetProductRevenueByDateRangeAsync(
        ProductRevenueReportRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.ProductName))
        {
            throw new ArgumentException("Product name is required.", nameof(request.ProductName));
        }

        var normalizedProductName = request.ProductName.Trim();
        var (from, to) = NormalizeDateRange(request.From, request.To);

        var report = await _reportRepository.GetProductRevenueByDateRangeAsync(
            normalizedProductName,
            from,
            to,
            cancellationToken);

        if (report == null)
        {
            throw new KeyNotFoundException($"Product with name '{normalizedProductName}' was not found.");
        }

        return report;
    }

    private static (DateTimeOffset From, DateTimeOffset To) NormalizeDateRange(DateTimeOffset from, DateTimeOffset to)
    {
        var normalizedFrom = from.ToUniversalTime();
        var normalizedTo = to.ToUniversalTime();

        if (normalizedFrom > normalizedTo)
        {
            throw new ArgumentException("'From' must be less than or equal to 'To'.");
        }

        return (normalizedFrom, normalizedTo);
    }
}
