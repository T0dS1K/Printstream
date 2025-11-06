using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Printstream.Infrastructure.Configurations
{
    public class Person
    {
        public int ID { get; set; }
        public string? LastName { get; set; } = null!;
        public string? FirstName { get; set; } = null!;
        public string? MiddleName { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public bool IsMale { get; set; }

        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<Phone>   Phones    { get; set; } = new List<Phone>();
        public ICollection<Email>   Emails    { get; set; } = new List<Email>();
    }

    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("Persons");

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
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(z => z.DateOfBirth)
                   .IsRequired();

            builder.Property(z => z.IsMale)
                   .IsRequired();

            builder.HasMany(z => z.Addresses)
                   .WithMany(z => z.Person)
                   .UsingEntity(j => j.ToTable("Person_Addresses"));

            builder.HasMany(z => z.Phones)
                   .WithMany(z => z.Person)
                   .UsingEntity(j => j.ToTable("Person_Phones"));

            builder.HasMany(z => z.Emails)
                   .WithMany(z => z.Person)
                   .UsingEntity(j => j.ToTable("Person_Emails"));

            builder.HasIndex(z => new { z.LastName, z.FirstName, z.MiddleName });
        }
    }
}
//cd C:\Users\T0dS1K\Desktop\Printstream\Printstream; dotnet ef migrations add Printstream