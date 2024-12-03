namespace MetaCortex.Orders.DataAcess.MessageBroker;

public interface IMessageConsumerService
{
    Task ReadMessageAsync();
}
