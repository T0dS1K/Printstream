using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Printstream.Infrastructure.Configurations
{
    public class Address
    {
        public int ID { get; set; }
        public string? address { get; set; } = null!;

        public ICollection<Person> Person { get; set; } = new List<Person>();
    }

    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(z => z.ID);

            builder.Property(z => z.ID)
                   .UseIdentityColumn();

            builder.Property(z => z.address)
                   .HasMaxLength(228)
                   .IsRequired();

            builder.HasIndex(z => z.address)
                   .IsUnique();
        }
    }
}