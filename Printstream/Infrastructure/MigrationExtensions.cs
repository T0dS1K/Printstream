using Microsoft.EntityFrameworkCore;

namespace Printstream.Infrastructure
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder App)
        {
            using IServiceScope Scope = App.ApplicationServices.CreateScope();
            using AppDbContext Context = Scope.ServiceProvider.GetRequiredService<AppDbContext>();
            Context.Database.Migrate();
        }
    }
}