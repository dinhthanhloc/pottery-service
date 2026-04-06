using PotteryService.Domain.Common;

namespace PotteryService.Domain.Entities;

public sealed class Product : AuditableEntity
{
    public long CategoryId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Sku { get; set; }

    public string? Description { get; set; }

    public decimal CurrentPrice { get; set; }

    public bool IsActive { get; set; } = true;

    public Category? Category { get; set; }

    public ICollection<ProductPriceHistory> PriceHistories { get; set; } = new List<ProductPriceHistory>();

    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}
