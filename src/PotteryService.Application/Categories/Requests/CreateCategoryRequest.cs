namespace PotteryService.Application.Categories.Requests;

public sealed record CreateCategoryRequest(
    string Name,
    string? Description);
