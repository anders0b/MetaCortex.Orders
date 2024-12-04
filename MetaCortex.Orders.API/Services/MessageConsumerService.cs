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
        private const string _queueName = "customer-to-order";
        private readonly IConnection _connection;
        private IChannel _channel;
        private readonly ObjectConverterService _objectConverterService;

        public MessageConsumerService(IRabbitMqService rabbitMqService, IOrderRepository repository)
        {
            _connection = rabbitMqService.CreateConnection().Result;
            _channel = _connection.CreateChannelAsync().Result;
            _objectConverterService = new ObjectConverterService(repository);
        }

        public async Task ReadCustomerOrderAsync()
        {
            await _channel.QueueDeclareAsync(queue: _queueName,
              durable: false,
              exclusive: false,
              autoDelete: false
              );

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = System.Text.Encoding.UTF8.GetString(body);
                await _objectConverterService.CheckVIP(message);
                Console.WriteLine($"recieved {message}");
            };

            await _channel.BasicConsumeAsync(queue: _queueName,
                     autoAck: true,
                     consumer: consumer);

            await Task.CompletedTask;
        }

        public async Task ReadMessageAsync()
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = System.Text.Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
            };

            await _channel.BasicConsumeAsync(queue: _queueName,
                                 autoAck: true,
                                 consumer: consumer);

            await Task.CompletedTask;
        }
    }
}
