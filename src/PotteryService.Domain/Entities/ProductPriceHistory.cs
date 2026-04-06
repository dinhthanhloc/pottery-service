using PotteryService.Domain.Common;

namespace PotteryService.Domain.Entities;

public sealed class ProductPriceHistory : BaseEntity
{
    public long ProductId { get; set; }

    public decimal Price { get; set; }

    public DateTimeOffset ValidFrom { get; set; } = DateTimeOffset.UtcNow;

    public string? Note { get; set; }

    public Product? Product { get; set; }
}
