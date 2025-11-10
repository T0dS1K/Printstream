using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Printstream.Infrastructure.Configurations
{
    public class Person
    {
        public int ID { get; set; }
        public int BunchID { get; set; }
        public int PersonDataID { get; set; }

        public Bunch Bunch { get; set; } = null!;
        public PersonData PersonData { get; set; } = null!;
        public ICollection<Phone>   Phone   { get; set; } = new List<Phone>();
        public ICollection<Email>   Email   { get; set; } = new List<Email>();
        public ICollection<Address> Address { get; set; } = new List<Address>();
    }

    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(z => z.ID);

            builder.Property(z => z.ID)
                   .UseIdentityColumn();

            builder.HasOne(z => z.PersonData)
                   .WithMany(z => z.Person)
                   .HasForeignKey(z => z.PersonDataID)
                   .IsRequired();

            builder.HasOne(z => z.Bunch)
                   .WithMany(z => z.Person)
                   .HasForeignKey(z => z.BunchID)
                   .IsRequired();

            builder.HasMany(z => z.Phone)
                   .WithMany(z => z.Person)
                   .UsingEntity(z => z.ToTable("Person_Phone"));

            builder.HasMany(z => z.Email)
                   .WithMany(z => z.Person)
                   .UsingEntity(z => z.ToTable("Person_Email"));

            builder.HasMany(z => z.Address)
                   .WithMany(z => z.Person)
                   .UsingEntity(z => z.ToTable("Person_Address"));
        }
    }
}