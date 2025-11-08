using Printstream.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Printstream.Services
{
    public interface IQueueService
    {
        Task<bool> AddTaskToQueue(UserSession UserData);
    }

    public class MQService : IQueueService, IDisposable
    {
        private string _queueName { get; }
        private IChannel _channel { get; }
        private IConnection _connection { get; }

        private MQService(IConnection connection, IChannel channel, string queueName)
        {
            _channel = channel;
            _queueName = queueName;
            _connection = connection;
        }

        public async static Task<IQueueService> CreateAsync(string queueName, string HostName)
        {
            var factory = new ConnectionFactory { HostName = HostName };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: queueName,
                                            durable: true,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);

            return new MQService(connection, channel, queueName);
        }

        public async Task<bool> AddTaskToQueue(UserSession UserData)
        {
            try
            {
                var SerializeData = JsonSerializer.Serialize(UserData);
                var body = Encoding.UTF8.GetBytes(SerializeData);
                var properties = new BasicProperties
                {
                    Persistent = true
                };

                await _channel.BasicPublishAsync(exchange: string.Empty,
                                                 routingKey: _queueName,
                                                 mandatory: false,
                                                 basicProperties: properties,
                                                 body: body);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MQService Error: {ex.Message}");
                return false;
            }
        }

        public void Dispose()
        {
            _channel?.DisposeAsync();
            _connection?.DisposeAsync();
        }
    }
}
