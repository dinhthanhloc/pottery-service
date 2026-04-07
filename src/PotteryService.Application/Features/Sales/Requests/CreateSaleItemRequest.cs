namespace PotteryService.Application.Features.Sales.Requests;

public sealed record CreateSaleItemRequest(
    long ProductId,
    int Quantity);
