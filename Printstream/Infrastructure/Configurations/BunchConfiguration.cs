using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Printstream.Infrastructure.Configurations
{
    public class Bunch
    {
        public int ID { get; set; }
        public string? bunch { get; set; } = null!;

        public ICollection<Person> Person { get; set; } = new List<Person>();
    }

    public class BunchConfiguration : IEntityTypeConfiguration<Bunch>
    {
        public void Configure(EntityTypeBuilder<Bunch> builder)
        {
            builder.HasKey(z => z.ID);

            builder.Property(z => z.ID)
                   .UseIdentityColumn();

            builder.Property(z => z.bunch)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.HasIndex(z => z.bunch)
                   .IsUnique();
        }
    }
}
