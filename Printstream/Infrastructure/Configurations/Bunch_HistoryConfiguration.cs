using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Printstream.Infrastructure.Configurations
{
    public class Bunch_History
    {
        public int ID { get; set; }
        public int PersonID { get; set; }
        public int BunchID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string OperationType { get; set; } = null!;

        public Person Person { get; set; } = null!;
        public Bunch Bunch { get; set; } = null!;
    }

    public class Bunch_HistoryConfiguration : IEntityTypeConfiguration<Bunch_History>
    {
        public void Configure(EntityTypeBuilder<Bunch_History> builder)
        {
            builder.HasKey(z => z.ID);

            builder.Property(z => z.ID)
                   .UseIdentityColumn();

            builder.Property(z => z.OperationType)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(z => z.DateFrom)
                   .IsRequired();

            builder.HasOne(z => z.Person)
                   .WithMany()
                   .HasForeignKey(z => z.PersonID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(z => z.Bunch)
                   .WithMany()
                   .HasForeignKey(z => z.BunchID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(z => z.PersonID);
            builder.HasIndex(z => z.BunchID);
            builder.HasIndex(z => z.DateFrom);
            builder.HasIndex(z => z.DateTo);
            builder.HasIndex(z => new { z.PersonID, z.DateFrom, z.DateTo });
        }
    }
}
