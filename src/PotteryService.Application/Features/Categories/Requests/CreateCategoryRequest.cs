namespace PotteryService.Application.Features.Categories.Requests;

public sealed record CreateCategoryRequest(
    string Name,
    string? Description);
