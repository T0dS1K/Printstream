using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Printstream.Infrastructure.Configurations;

namespace Printstream.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<PersonData> PersonData { get; set; } = null!;
        public DbSet<Person> Person { get; set; } = null!;
        public DbSet<Bunch> Bunch { get; set; } = null!;
        public DbSet<Address> Address { get; set; } = null!;
        public DbSet<Phone> Phone { get; set; } = null!;
        public DbSet<Email> Email { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
//cd C:\Users\T0dS1K\Desktop\Printstream\Printstream; dotnet ef migrations add Printstream