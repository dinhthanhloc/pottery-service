using PotteryService.Application.Common.Interfaces;
using PotteryService.Application.Common.Exceptions;
using PotteryService.Application.Features.Products.Dtos;
using PotteryService.Application.Features.Products.Requests;
using PotteryService.Domain.Entities;

namespace PotteryService.Application.Features.Products.Services;

public sealed class ProductService : IProductService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IProductPriceHistoryRepository _productPriceHistoryRepository;

    public ProductService(
        ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        IProductPriceHistoryRepository productPriceHistoryRepository)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _productPriceHistoryRepository = productPriceHistoryRepository;
    }

    public async Task<IReadOnlyCollection<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);

        return products
            .Select(MapToDto)
            .ToArray();
    }

    public async Task<ProductDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken);

        if (product == null)
        {
            throw new KeyNotFoundException($"Product with id '{id}' was not found.");
        }

        return MapToDto(product);
    }

    public async Task<ProductDto> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category == null)
        {
            throw new KeyNotFoundException($"Category with id '{request.CategoryId}' was not found.");
        }

        var normalizedName = NormalizeName(request.Name);
        var normalizedSku = NormalizeSku(request.Sku);
        var normalizedDescription = NormalizeDescription(request.Description);
        var price = NormalizePrice(request.CurrentPrice);

        if (await _productRepository.ExistsByNameAsync(normalizedName, cancellationToken: cancellationToken))
        {
            throw new ConflictException($"Product name '{normalizedName}' already exists.");
        }

        if (!string.IsNullOrWhiteSpace(normalizedSku) &&
            await _productRepository.ExistsBySkuAsync(normalizedSku, cancellationToken: cancellationToken))
        {
            throw new ConflictException($"Product sku '{normalizedSku}' already exists.");
        }

        var product = new Product
        {
            CategoryId = request.CategoryId,
            Name = normalizedName,
            Sku = normalizedSku,
            Description = normalizedDescription,
            CurrentPrice = price,
            IsActive = request.IsActive
        };

        await _productRepository.AddAsync(product, cancellationToken);
        await AddPriceHistoryAsync(product.Id, product.CurrentPrice, "Initial price", cancellationToken);

        return MapToDto(product, category.Name);
    }

    public async Task<ProductDto> UpdateAsync(long id, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdForUpdateAsync(id, cancellationToken);

        if (product == null)
        {
            throw new KeyNotFoundException($"Product with id '{id}' was not found.");
        }

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category == null)
        {
            throw new KeyNotFoundException($"Category with id '{request.CategoryId}' was not found.");
        }

        var normalizedName = NormalizeName(request.Name);
        var normalizedSku = NormalizeSku(request.Sku);
        var normalizedDescription = NormalizeDescription(request.Description);
        var price = NormalizePrice(request.CurrentPrice);
        var previousPrice = product.CurrentPrice;

        if (await _productRepository.ExistsByNameAsync(normalizedName, id, cancellationToken))
        {
            throw new ConflictException($"Product name '{normalizedName}' already exists.");
        }

        if (!string.IsNullOrWhiteSpace(normalizedSku) &&
            await _productRepository.ExistsBySkuAsync(normalizedSku, id, cancellationToken))
        {
            throw new ConflictException($"Product sku '{normalizedSku}' already exists.");
        }

        product.CategoryId = request.CategoryId;
        product.Name = normalizedName;
        product.Sku = normalizedSku;
        product.Description = normalizedDescription;
        product.CurrentPrice = price;
        product.IsActive = request.IsActive;

        await _productRepository.UpdateAsync(product, cancellationToken);

        if (previousPrice != product.CurrentPrice)
        {
            await AddPriceHistoryAsync(product.Id, product.CurrentPrice, "Price updated", cancellationToken);
        }

        return MapToDto(product, category.Name);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdForUpdateAsync(id, cancellationToken);

        if (product == null)
        {
            throw new KeyNotFoundException($"Product with id '{id}' was not found.");
        }

        await _productRepository.DeleteAsync(product, cancellationToken);
    }

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Product name is required.", nameof(name));
        }

        var normalized = name.Trim();

        if (normalized.Length > 150)
        {
            throw new ArgumentException("Product name must not exceed 150 characters.", nameof(name));
        }

        return normalized;
    }

    private static string? NormalizeSku(string? sku)
    {
        if (string.IsNullOrWhiteSpace(sku))
        {
            return null;
        }

        var normalized = sku.Trim();

        if (normalized.Length > 50)
        {
            throw new ArgumentException("Product sku must not exceed 50 characters.", nameof(sku));
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
            throw new ArgumentException("Product description must not exceed 500 characters.", nameof(description));
        }

        return normalized;
    }

    private static decimal NormalizePrice(decimal currentPrice)
    {
        if (currentPrice < 0)
        {
            throw new ArgumentException("Product price must be greater than or equal to 0.", nameof(currentPrice));
        }

        return currentPrice;
    }

    private async Task AddPriceHistoryAsync(long productId, decimal price, string note, CancellationToken cancellationToken)
    {
        var productPriceHistory = new ProductPriceHistory
        {
            ProductId = productId,
            Price = price,
            ValidFrom = DateTimeOffset.UtcNow,
            Note = note
        };

        await _productPriceHistoryRepository.AddAsync(productPriceHistory, cancellationToken);
    }

    private static ProductDto MapToDto(Product product)
    {
        return MapToDto(product, product.Category?.Name ?? string.Empty);
    }

    private static ProductDto MapToDto(Product product, string categoryName)
    {
        return new ProductDto(
            product.Id,
            product.CategoryId,
            categoryName,
            product.Name,
            product.Sku,
            product.Description,
            product.CurrentPrice,
            product.IsActive,
            product.CreatedAt,
            product.UpdatedAt);
    }
}
