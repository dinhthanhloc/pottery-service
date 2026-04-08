namespace PotteryService.Application.Features.Reports.Requests;

public sealed record ProductRevenueReportRequest(
    string ProductName,
    DateTimeOffset From,
    DateTimeOffset To);
