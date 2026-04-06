using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PotteryService.Domain.Entities;

namespace PotteryService.Infrastructure.Persistence.Configurations;

public sealed class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("sale_items");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.SaleId)
            .HasColumnName("sale_id")
            .IsRequired();

        builder.Property(x => x.ProductId)
            .HasColumnName("product_id")
            .IsRequired();

        builder.Property(x => x.Quantity)
            .HasColumnName("quantity")
            .IsRequired();

        builder.Property(x => x.UnitPrice)
            .HasColumnName("unit_price")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.LineTotal)
            .HasColumnName("line_total")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasIndex(x => x.ProductId)
            .HasDatabaseName("idx_sale_items_product_id");

        builder.HasIndex(x => new { x.SaleId, x.ProductId })
            .IsUnique()
            .HasDatabaseName("uq_sale_product");

        builder.HasCheckConstraint("ck_sale_items_quantity", "\"quantity\" > 0");
        builder.HasCheckConstraint("ck_sale_items_unit_price", "\"unit_price\" >= 0");
        builder.HasCheckConstraint("ck_sale_items_line_total", "\"line_total\" >= 0");

        builder.HasOne(x => x.Sale)
            .WithMany(x => x.SaleItems)
            .HasForeignKey(x => x.SaleId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_sale_items_sale");

        builder.HasOne(x => x.Product)
            .WithMany(x => x.SaleItems)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_sale_items_product");
    }
}
