namespace PotteryService.Application.Features.Reports.Dtos;

public sealed record SalesQuantityReportItemDto(
    long ProductId,
    string ProductName,
    int TotalQuantitySold);
