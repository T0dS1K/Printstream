using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Printstream.Infrastructure.Configurations;

namespace Printstream.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; } = null!;

        public DbSet<Address> Addresses { get; set; } = null!;

        public DbSet<Phone> Phones { get; set; } = null!;

        public DbSet<Email> Emails { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}