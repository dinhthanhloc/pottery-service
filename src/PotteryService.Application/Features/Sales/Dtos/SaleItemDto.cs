namespace PotteryService.Application.Features.Sales.Dtos;

public sealed record SaleItemDto(
    long ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal LineTotal);
