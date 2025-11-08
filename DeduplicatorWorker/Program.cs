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
            var queueService = await Worker.CreateAsync(MQ_Queue, MQ_HostName);
            builder.Services.AddDbContext<AppDbContext>(
                options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddSingleton<IHostedService>(queueService);
            var host = builder.Build();
            host.Run();
        }
    }
}