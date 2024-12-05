using MetaCortex.Orders.API.Services;
using MetaCortex.Orders.DataAcess;
using MetaCortex.Orders.DataAcess.MessageBroker;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MetaCortex.Orders.API.BackgroundServices
{
    public class MessageConsumerHostedService : BackgroundService
    {
        private readonly IMessageConsumerService _messageConsumerService;
        private readonly ObjectConverterService _objectConverterService;
        private readonly IOrderRepository _repository;
        public MessageConsumerHostedService(IMessageConsumerService messageConsumerService, IOrderRepository repository)
        {
            _messageConsumerService = messageConsumerService;
            _repository = repository;
            _objectConverterService = new ObjectConverterService(repository);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queueNames = new string[] { "order-to-payment", "payment-to-order" };
            Console.WriteLine("Message Consumer Hosted Service is running.");
            foreach (var queueName in queueNames)
            {
                await _messageConsumerService.ReadMessageAsync(queueName, async (message) =>
                {
                    if(queueName == "order-to-payment")
                    {
                        Console.WriteLine("Order to Payment");
                        await _objectConverterService.CheckVIP(message);
                    }
                    else if (queueName == "payment-to-order")
                    {
                        Console.WriteLine("Payment to Order");
                        await _objectConverterService.SaveFinalOrderFromPayment(message);
                    }
                });
            }
        }
    }
}
