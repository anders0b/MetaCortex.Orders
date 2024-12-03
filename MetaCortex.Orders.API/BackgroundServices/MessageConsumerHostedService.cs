using MetaCortex.Orders.DataAcess.MessageBroker;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace MetaCortex.Orders.API.BackgroundServices
{
    public class MessageConsumerHostedService : BackgroundService
    {
        private readonly IMessageConsumerService _messageConsumerService;
        public MessageConsumerHostedService(IMessageConsumerService messageConsumerService)
        {
            _messageConsumerService = messageConsumerService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _messageConsumerService.ReadMessageAsync();
        }
    }
}
