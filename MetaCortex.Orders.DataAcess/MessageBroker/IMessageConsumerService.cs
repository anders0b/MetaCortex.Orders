namespace MetaCortex.Orders.DataAcess.MessageBroker;

public interface IMessageConsumerService
{
    Task ReadMessageAsync(string queueName, Func<string, Task> messageHandler);
}
