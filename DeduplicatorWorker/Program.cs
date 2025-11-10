using DeduplicatorWorker.Services;
using Microsoft.EntityFrameworkCore;
using Printstream.Infrastructure;

namespace DeduplicatorWorker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            var MQ_Queue = Environment.GetEnvironmentVariable("MQ_Queue")!;
            var MQ_HostName = Environment.GetEnvironmentVariable("MQ_HostName")!;

            builder.Services.AddDbContext<AppDbContext>(
                options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IDeduplicator, DeduplicationService>();
            builder.Services.AddHostedService(serviceProvider =>
            {
                try
                {
                    return Worker.CreateAsync(serviceProvider, MQ_Queue, MQ_HostName).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Worker: {ex.Message}");
                    throw;
                }
            });

            var host = builder.Build();
            await host.RunAsync();
        }
    }
}