using Printstream.Services;
using Printstream.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Printstream
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var MQ_Queue = Environment.GetEnvironmentVariable("MQ_Queue")!;
            var MQ_HostName = Environment.GetEnvironmentVariable("MQ_HostName")!;
            var queueService = await MQService.CreateAsync(MQ_Queue, MQ_HostName);

            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSingleton<IQueueService>(queueService);
            builder.Services.AddDbContext<AppDbContext>(
                options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.ApplyMigrations();
            }

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}