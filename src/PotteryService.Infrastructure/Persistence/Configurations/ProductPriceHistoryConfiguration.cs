using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PotteryService.Domain.Entities;

namespace PotteryService.Infrastructure.Persistence.Configurations;

public sealed class ProductPriceHistoryConfiguration : IEntityTypeConfiguration<ProductPriceHistory>
{
    public void Configure(EntityTypeBuilder<ProductPriceHistory> builder)
    {
        builder.ToTable("product_price_histories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.ProductId)
            .HasColumnName("product_id")
            .IsRequired();

        builder.Property(x => x.Price)
            .HasColumnName("price")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.ValidFrom)
            .HasColumnName("valid_from")
            .HasDefaultValueSql("NOW()");

        builder.Property(x => x.Note)
            .HasColumnName("note")
            .HasMaxLength(255);

        builder.HasCheckConstraint("ck_product_price_histories_price", "\"price\" >= 0");

        builder.HasOne(x => x.Product)
            .WithMany(x => x.PriceHistories)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_price_history_product");
    }
}
