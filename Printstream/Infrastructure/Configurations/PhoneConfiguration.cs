using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Printstream.Infrastructure.Configurations
{
    public class Phone
    {
        public int ID { get; set; }
        public string? Number { get; set; } = null!;

        public ICollection<Person> Person { get; set; } = new List<Person>();
    }

    public class PhoneConfiguration : IEntityTypeConfiguration<Phone>
    {
        public void Configure(EntityTypeBuilder<Phone> builder)
        {
            builder.ToTable("Phones");

            builder.HasKey(z => z.ID);

            builder.Property(z => z.ID)
                   .UseIdentityColumn();

            builder.Property(z => z.Number)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.HasIndex(z => z.Number)
                   .IsUnique();
        }
    }
}