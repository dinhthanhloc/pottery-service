using PotteryService.Domain.Common;

namespace PotteryService.Domain.Entities;

public sealed class SaleItem : BaseEntity
{
    public long SaleId { get; set; }

    public long ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal LineTotal { get; set; }

    public Sale? Sale { get; set; }

    public Product? Product { get; set; }
}
