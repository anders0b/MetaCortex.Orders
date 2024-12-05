using MetaCortex.Orders.API.InterfaceM;
using MetaCortex.Orders.DataAcess;
using MetaCortex.Orders.DataAcess.MessageBroker;
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

        public MessageConsumerService(IRabbitMqService rabbitMqService, IOrderRepository repository)
        {
            _connection = rabbitMqService.CreateConnection().Result;
            _channel = _connection.CreateChannelAsync().Result;
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
                await messageHandler(message);
                Console.WriteLine($"Recieved {message}");
            };

            await _channel.BasicConsumeAsync(queue: queueName,
                     autoAck: true,
                     consumer: consumer);

            await Task.CompletedTask;
        }
    }
}
