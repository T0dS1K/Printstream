using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Printstream.Infrastructure.Configurations
{
    public class PersonData
    {
        public int ID { get; set; }
        public string? LastName { get; set; } = null!;
        public string? FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string? DateOfBirth { get; set; } = null!;
        public bool IsMale { get; set; }

        public ICollection<Person> Person { get; set; } = new List<Person>();
    }

    public class PersonDataConfiguration : IEntityTypeConfiguration<PersonData>
    {
        public void Configure(EntityTypeBuilder<PersonData> builder)
        {
            builder.HasKey(z => z.ID);

            builder.Property(z => z.ID)
                   .UseIdentityColumn();

            builder.Property(z => z.LastName)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(z => z.FirstName)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(z => z.MiddleName)
                   .HasMaxLength(100);

            builder.Property(z => z.DateOfBirth)
                   .HasMaxLength(10)
                   .IsRequired();

            builder.Property(z => z.IsMale)
                   .IsRequired();

            builder.HasIndex(z => new { z.LastName, z.FirstName, z.MiddleName, z.DateOfBirth, z.IsMale })
                   .IsUnique();
        }
    }
}
