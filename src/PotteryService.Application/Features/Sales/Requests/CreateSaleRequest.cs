namespace PotteryService.Application.Features.Sales.Requests;

public sealed record CreateSaleRequest(
    string? CustomerName,
    string? Note,
    DateTimeOffset? SaleDate,
    IReadOnlyCollection<CreateSaleItemRequest> Items);
