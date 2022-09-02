using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(150).IsRequired();
            builder.Property(x => x.PictureUrl).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.QuantityInStock).IsRequired();
            builder.Property(x => x.Brand).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(400).IsRequired();
            builder.Property(x => x.Type).HasMaxLength(100).IsRequired();
        }
    }
}
