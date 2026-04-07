namespace PotteryService.Application.Features.Products.Dtos;

public sealed record ProductDto(
    long Id,
    long CategoryId,
    string CategoryName,
    string Name,
    string? Sku,
    string? Description,
    decimal CurrentPrice,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
