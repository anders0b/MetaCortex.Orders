using MetaCortex.Orders.DataAcess.MessageBroker;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MetaCortex.Orders.API.Services
{
    public class MessageProducerService : IMessageProducerService
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IChannel _channel;
        public MessageProducerService(RabbitMqConfiguration config)
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = config.HostName,
                UserName = config.UserName,
                Password = config.Password

            };
            _connection = _connectionFactory.CreateConnectionAsync().Result;
            _channel = _connection.CreateChannelAsync().Result;
        }

        public async Task SendMessageAsync<T>(T message, string routingKey)
        {
            await _channel.QueueDeclareAsync(queue: routingKey,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var jsonString = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonString);

            await _channel.BasicPublishAsync(exchange: "",
                                 routingKey: routingKey,
                                 body: body);

            Console.WriteLine($"Sent {message} to {routingKey}");

        }
    }
}
