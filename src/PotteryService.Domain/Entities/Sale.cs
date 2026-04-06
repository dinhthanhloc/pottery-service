using PotteryService.Domain.Common;

namespace PotteryService.Domain.Entities;

public sealed class Sale : BaseEntity
{
    public string SaleCode { get; set; } = string.Empty;

    public DateTimeOffset SaleDate { get; set; } = DateTimeOffset.UtcNow;

    public decimal TotalAmount { get; set; }

    public string? CustomerName { get; set; }

    public string? Note { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}
