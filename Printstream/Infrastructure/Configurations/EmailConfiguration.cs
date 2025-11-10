using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Printstream.Infrastructure.Configurations
{
    public class Email
    {
        public int ID { get; set; }
        public string? email { get; set; } = null!;

        public ICollection<Person> Person { get; set; } = new List<Person>();
    }

    public class EmailConfiguration : IEntityTypeConfiguration<Email>
    {
        public void Configure(EntityTypeBuilder<Email> builder)
        {
            builder.HasKey(z => z.ID);

            builder.Property(z => z.ID)
                   .UseIdentityColumn();

            builder.Property(z => z.email)
                   .HasMaxLength(52)
                   .IsRequired();

            builder.HasIndex(z => z.email)
                   .IsUnique();
        }
    }
}