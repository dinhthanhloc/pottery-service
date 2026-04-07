namespace PotteryService.Application.Features.Reports.Requests;

public sealed record SalesQuantityReportRequest(
    DateTimeOffset From,
    DateTimeOffset To);
