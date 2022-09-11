using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    internal sealed class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
    {
        private const string TABLE_NAME = "BasketItems";

        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
            builder.ToTable(TABLE_NAME);
        }
    }
}
