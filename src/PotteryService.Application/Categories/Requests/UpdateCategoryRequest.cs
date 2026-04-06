namespace PotteryService.Application.Categories.Requests;

public sealed record UpdateCategoryRequest(
    string Name,
    string? Description);
