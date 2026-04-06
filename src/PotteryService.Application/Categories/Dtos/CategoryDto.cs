namespace PotteryService.Application.Categories.Dtos;

public sealed record CategoryDto(
    long Id,
    string Name,
    string? Description,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
