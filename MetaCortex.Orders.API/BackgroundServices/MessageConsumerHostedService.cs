using DnsClient.Internal;
using MetaCortex.Orders.API.Services;
using MetaCortex.Orders.DataAcess;
using MetaCortex.Orders.DataAcess.MessageBroker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace MetaCortex.Orders.API.BackgroundServices
{
    public class MessageConsumerHostedService : BackgroundService
    {
        private readonly IMessageConsumerService _messageConsumerService;
        private readonly ObjectConverterService _objectConverterService;
        private readonly IOrderRepository _repository;
        private readonly ILogger<ObjectConverterService> _logger;
        public MessageConsumerHostedService(IMessageConsumerService messageConsumerService, IOrderRepository repository, ILogger<ObjectConverterService> logger)
        {
            _messageConsumerService = messageConsumerService;
            _repository = repository;
            _logger = logger;
            _objectConverterService = new ObjectConverterService(repository, logger);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queueNames = new string[] { "customer-to-order", "payment-to-order" };
            _logger.LogInformation("Currently listening for messages. Service is running.");
            foreach (var queueName in queueNames)
            {
                await _messageConsumerService.ReadMessageAsync(queueName, async (message) =>
                {
                    if(queueName == "customer-to-order")
                    {
                        _logger.LogInformation($"Message from {queueName}. Delivering message: {message} to CheckVIP");
                        await _objectConverterService.CheckVIP(message);
                    }
                    else if (queueName == "payment-to-order")
                    {
                        _logger.LogInformation($"Message from {queueName}. Delivering message: {message} to SaveFinalOrder");
                        await _objectConverterService.SaveFinalOrderFromPayment(message);
                    }
                });
            }
        }
    }
}
