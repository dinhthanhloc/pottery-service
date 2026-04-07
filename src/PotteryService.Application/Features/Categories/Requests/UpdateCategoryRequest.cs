namespace PotteryService.Application.Features.Categories.Requests;

public sealed record UpdateCategoryRequest(
    string Name,
    string? Description);
