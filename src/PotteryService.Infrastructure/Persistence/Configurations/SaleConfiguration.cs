using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PotteryService.Domain.Entities;

namespace PotteryService.Infrastructure.Persistence.Configurations;

public sealed class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("sales");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.SaleCode)
            .HasColumnName("sale_code")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.SaleDate)
            .HasColumnName("sale_date")
            .HasDefaultValueSql("NOW()");

        builder.Property(x => x.TotalAmount)
            .HasColumnName("total_amount")
            .HasPrecision(18, 2)
            .HasDefaultValue(0m)
            .IsRequired();

        builder.Property(x => x.CustomerName)
            .HasColumnName("customer_name")
            .HasMaxLength(150);

        builder.Property(x => x.Note)
            .HasColumnName("note")
            .HasMaxLength(500);

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("NOW()");

        builder.HasIndex(x => x.SaleCode)
            .IsUnique();

        builder.HasIndex(x => x.SaleDate)
            .HasDatabaseName("idx_sales_sale_date");

        builder.HasCheckConstraint("ck_sales_total_amount", "\"total_amount\" >= 0");
    }
}
