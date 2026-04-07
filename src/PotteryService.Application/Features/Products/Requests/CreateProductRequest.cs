namespace PotteryService.Application.Features.Products.Requests;

public sealed record CreateProductRequest(
    long CategoryId,
    string Name,
    string? Sku,
    string? Description,
    decimal CurrentPrice,
    bool IsActive);
