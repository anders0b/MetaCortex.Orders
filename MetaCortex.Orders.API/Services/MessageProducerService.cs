using MetaCortex.Orders.DataAcess.MessageBroker;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MetaCortex.Orders.API.Services
{
    public class MessageProducerService(RabbitMqConfiguration config) : IMessageProducerService
    {
        private readonly ConnectionFactory _connectionFactory = new ConnectionFactory
        {
            HostName = config.HostName,
            UserName = config.UserName,
            Password = config.Password
        };
        public async Task SendMessageAsync<T>(T message)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: "orders",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);


            var jsonString = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonString);

            await channel.BasicPublishAsync(exchange: "",
                                 routingKey: "orders",
                                 body: body);
        }
    }
}
