using MetaCortex.Orders.DataAcess.MessageBroker;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading.Tasks;

namespace MetaCortex.Orders.API.Services
{
    public class MessageConsumerService : IMessageConsumerService
    {
        public async Task ReadMessageAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Password = "guest",
                UserName = "guest"
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "orders",
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = System.Text.Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
            };

            await channel.BasicConsumeAsync(queue: "orders",
                                 autoAck: true,
                                 consumer: consumer);

            await Task.CompletedTask; //kanske ta bort
        }
    }
}
