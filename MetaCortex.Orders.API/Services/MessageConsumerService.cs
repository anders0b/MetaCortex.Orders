using DnsClient.Internal;
using MetaCortex.Orders.API.InterfaceM;
using MetaCortex.Orders.DataAcess;
using MetaCortex.Orders.DataAcess.MessageBroker;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading.Tasks;

namespace MetaCortex.Orders.API.Services
{
    public class MessageConsumerService : IMessageConsumerService
    {
        private readonly IConnection _connection;
        private IChannel _channel;
        private readonly ILogger<MessageConsumerService> _logger;

        public MessageConsumerService(IRabbitMqService rabbitMqService, IOrderRepository repository, ILogger<MessageConsumerService> logger)
        {
            _connection = rabbitMqService.CreateConnection().Result;
            _channel = _connection.CreateChannelAsync().Result;
            _logger = logger;
        }

        public async Task ReadMessageAsync(string queueName, Func<string, Task> messageHandler)
        {
            await _channel.QueueDeclareAsync(queue: queueName,
              durable: false,
              exclusive: false,
              autoDelete: false
              );

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = System.Text.Encoding.UTF8.GetString(body);
                _logger.LogInformation($"Recieved {message} from {queueName}");
                await messageHandler(message);
            };

            await _channel.BasicConsumeAsync(queue: queueName,
                     autoAck: true,
                     consumer: consumer);

            _logger.LogInformation($"Consumed message");

            await Task.CompletedTask;
        }
    }
}
