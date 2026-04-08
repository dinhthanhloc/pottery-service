namespace PotteryService.Application.Features.Reports.Dtos;

public sealed record ProductRevenueReportDto(
    long ProductId,
    string ProductName,
    DateTimeOffset From,
    DateTimeOffset To,
    decimal TotalRevenue);
