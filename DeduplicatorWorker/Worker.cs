using DeduplicatorWorker.Services;
using Microsoft.Extensions.DependencyInjection;
using Printstream.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace DeduplicatorWorker
{
    public class Worker : BackgroundService
    {
        private string _queueName { get; }
        private IChannel _channel { get; }
        private IConnection _connection { get; }
        private IServiceProvider _serviceProvider { get; }

        private Worker(IServiceProvider serviceProvider, IConnection connection, IChannel channel, string queueName)
        {
            _channel = channel;
            _queueName = queueName;
            _connection = connection;
            _serviceProvider = serviceProvider;
        }

        public async static Task<Worker> CreateAsync(IServiceProvider serviceProvider, string queueName, string HostName)
        {
            var factory = new ConnectionFactory { HostName = HostName };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.BasicQosAsync(prefetchSize: 0,
                                        prefetchCount: 1,
                                        global: false);

            await channel.QueueDeclareAsync(queue: queueName,
                                            durable: true,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);

            return new Worker(serviceProvider, connection, channel, queueName);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    try
                    {
                        var deduplicator = scope.ServiceProvider.GetRequiredService<IDeduplicator>();
                        var body = ea.Body.ToArray();
                        var json = Encoding.UTF8.GetString(body);
                        var unserializeData = JsonSerializer.Deserialize<UserSession>(json);
                        var Message = await deduplicator.TryAddUser(unserializeData!);

                        Console.WriteLine($" [x] {Message}");
                        await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Worker failed to process data: {ex.Message}");
                        await _channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);
                    }
                }
            };

            await _channel.BasicConsumeAsync(_queueName, autoAck: false, consumer: consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
        }

        public override void Dispose()
        {
            _channel?.DisposeAsync();
            _connection?.DisposeAsync();
            base.Dispose();
        }
    }
}