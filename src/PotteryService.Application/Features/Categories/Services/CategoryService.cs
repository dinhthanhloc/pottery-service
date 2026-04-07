using PotteryService.Application.Common.Exceptions;
using PotteryService.Application.Common.Interfaces;
using PotteryService.Application.Features.Categories.Dtos;
using PotteryService.Application.Features.Categories.Requests;
using PotteryService.Domain.Entities;

namespace PotteryService.Application.Features.Categories.Services;

public sealed class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IReadOnlyCollection<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);

        return categories
            .Select(MapToDto)
            .ToArray();
    }

    public async Task<CategoryDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);

        if (category == null)
        {
            throw new KeyNotFoundException($"Category with id '{id}' was not found.");
        }

        return MapToDto(category);
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var normalizedName = NormalizeName(request.Name);
        var description = NormalizeDescription(request.Description);

        if (await _categoryRepository.ExistsByNameAsync(normalizedName, cancellationToken: cancellationToken))
        {
            throw new ConflictException($"Category name '{normalizedName}' already exists.");
        }

        var category = new Category
        {
            Name = normalizedName,
            Description = description
        };

        await _categoryRepository.AddAsync(category, cancellationToken);

        return MapToDto(category);
    }

    public async Task<CategoryDto> UpdateAsync(long id, UpdateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdForUpdateAsync(id, cancellationToken);

        if (category == null)
        {
            throw new KeyNotFoundException($"Category with id '{id}' was not found.");
        }

        var normalizedName = NormalizeName(request.Name);
        var description = NormalizeDescription(request.Description);

        if (await _categoryRepository.ExistsByNameAsync(normalizedName, id, cancellationToken))
        {
            throw new ConflictException($"Category name '{normalizedName}' already exists.");
        }

        category.Name = normalizedName;
        category.Description = description;

        await _categoryRepository.UpdateAsync(category, cancellationToken);

        return MapToDto(category);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdForUpdateAsync(id, cancellationToken);

        if (category == null)
        {
            throw new KeyNotFoundException($"Category with id '{id}' was not found.");
        }

        await _categoryRepository.DeleteAsync(category, cancellationToken);
    }

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Category name is required.", nameof(name));
        }

        var normalized = name.Trim();

        if (normalized.Length > 100)
        {
            throw new ArgumentException("Category name must not exceed 100 characters.", nameof(name));
        }

        return normalized;
    }

    private static string? NormalizeDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return null;
        }

        var normalized = description.Trim();

        if (normalized.Length > 500)
        {
            throw new ArgumentException("Category description must not exceed 500 characters.", nameof(description));
        }

        return normalized;
    }

    private static CategoryDto MapToDto(Category category)
    {
        return new CategoryDto(
            category.Id,
            category.Name,
            category.Description,
            category.CreatedAt,
            category.UpdatedAt);
    }
}
