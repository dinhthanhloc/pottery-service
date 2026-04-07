namespace PotteryService.Application.Features.Sales.Dtos;

public sealed record SaleDto(
    long Id,
    string SaleCode,
    DateTimeOffset SaleDate,
    decimal TotalAmount,
    string? CustomerName,
    string? Note,
    DateTimeOffset CreatedAt,
    IReadOnlyCollection<SaleItemDto> Items);
