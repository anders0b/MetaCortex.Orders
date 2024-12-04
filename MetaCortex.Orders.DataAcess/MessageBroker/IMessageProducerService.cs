namespace MetaCortex.Orders.DataAcess.MessageBroker;

public interface IMessageProducerService
{
    Task SendMessageAsync<T>(T message, string routingKey);
}
