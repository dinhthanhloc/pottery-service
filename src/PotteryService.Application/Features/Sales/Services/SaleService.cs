using PotteryService.Application.Common.Interfaces;
using PotteryService.Application.Features.Sales.Dtos;
using PotteryService.Application.Features.Sales.Requests;
using PotteryService.Domain.Entities;

namespace PotteryService.Application.Features.Sales.Services;

public sealed class SaleService : ISaleService
{
    private readonly IProductRepository _productRepository;
    private readonly ISaleRepository _saleRepository;

    public SaleService(IProductRepository productRepository, ISaleRepository saleRepository)
    {
        _productRepository = productRepository;
        _saleRepository = saleRepository;
    }

    public async Task<IReadOnlyCollection<SaleDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var sales = await _saleRepository.GetAllAsync(cancellationToken);

        return sales
            .Select(MapToDto)
            .ToArray();
    }

    public async Task<SaleDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var sale = await _saleRepository.GetByIdAsync(id, cancellationToken);

        if (sale == null)
        {
            throw new KeyNotFoundException($"Sale with id '{id}' was not found.");
        }

        return MapToDto(sale);
    }

    public async Task<SaleDto> CreateAsync(CreateSaleRequest request, CancellationToken cancellationToken = default)
    {
        ValidateRequest(request);

        var duplicatedProductId = request.Items
            .GroupBy(x => x.ProductId)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key)
            .FirstOrDefault();

        if (duplicatedProductId != 0)
        {
            throw new ArgumentException($"Product '{duplicatedProductId}' appears more than once in the sale.");
        }

        var productIds = request.Items
            .Select(x => x.ProductId)
            .Distinct()
            .ToArray();

        var products = await _productRepository.GetByIdsAsync(productIds, cancellationToken);
        var productLookup = products.ToDictionary(x => x.Id);

        foreach (var productId in productIds)
        {
            if (!productLookup.ContainsKey(productId))
            {
                throw new KeyNotFoundException($"Product with id '{productId}' was not found.");
            }
        }

        foreach (var product in products)
        {
            if (!product.IsActive)
            {
                throw new ArgumentException($"Product '{product.Name}' is inactive and cannot be sold.");
            }
        }

        var saleItems = request.Items.Select(item =>
        {
            var product = productLookup[item.ProductId];
            var lineTotal = product.CurrentPrice * item.Quantity;

            return new SaleItem
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = product.CurrentPrice,
                LineTotal = lineTotal
            };
        }).ToList();

        var sale = new Sale
        {
            SaleCode = GenerateSaleCode(),
            SaleDate = request.SaleDate?.ToUniversalTime() ?? DateTimeOffset.UtcNow,
            CustomerName = NormalizeOptionalText(request.CustomerName, 150, "Customer name"),
            Note = NormalizeOptionalText(request.Note, 500, "Note"),
            TotalAmount = saleItems.Sum(x => x.LineTotal),
            SaleItems = saleItems
        };

        await _saleRepository.AddAsync(sale, cancellationToken);

        return MapToDto(sale, productLookup);
    }

    private static void ValidateRequest(CreateSaleRequest request)
    {
        if (request.Items == null || request.Items.Count == 0)
        {
            throw new ArgumentException("Sale must contain at least one item.", nameof(request.Items));
        }

        foreach (var item in request.Items)
        {
            if (item.ProductId <= 0)
            {
                throw new ArgumentException("Product id must be greater than 0.", nameof(item.ProductId));
            }

            if (item.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than 0.", nameof(item.Quantity));
            }
        }
    }

    private static string? NormalizeOptionalText(string? value, int maxLength, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var normalized = value.Trim();

        if (normalized.Length > maxLength)
        {
            throw new ArgumentException($"{fieldName} must not exceed {maxLength} characters.", fieldName);
        }

        return normalized;
    }

    private static string GenerateSaleCode()
    {
        var suffix = Guid.NewGuid().ToString("N")[..6].ToUpperInvariant();
        return $"SALE-{DateTimeOffset.UtcNow:yyyyMMddHHmmss}-{suffix}";
    }

    private static SaleDto MapToDto(Sale sale)
    {
        return MapToDto(
            sale,
            sale.SaleItems
                .Where(x => x.Product != null)
                .Select(x => x.Product!)
                .ToDictionary(x => x.Id));
    }

    private static SaleDto MapToDto(Sale sale, IReadOnlyDictionary<long, Product> productLookup)
    {
        var items = sale.SaleItems
            .Select(item => new SaleItemDto(
                item.ProductId,
                productLookup.TryGetValue(item.ProductId, out var product) ? product.Name : string.Empty,
                item.Quantity,
                item.UnitPrice,
                item.LineTotal))
            .ToArray();

        return new SaleDto(
            sale.Id,
            sale.SaleCode,
            sale.SaleDate,
            sale.TotalAmount,
            sale.CustomerName,
            sale.Note,
            sale.CreatedAt,
            items);
    }
}
