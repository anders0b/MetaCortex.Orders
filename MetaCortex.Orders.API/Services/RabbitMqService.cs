using MetaCortex.Orders.API.InterfaceM;
using MetaCortex.Orders.DataAcess.MessageBroker;
using RabbitMQ.Client;
using System.Threading.Tasks;

namespace MetaCortex.Orders.API.Services;

public class RabbitMqService(RabbitMqConfiguration config) : IRabbitMqService
{
    public Task<IConnection> CreateConnection()
    {
        var connectionFactory = new ConnectionFactory
        {
            HostName = config.HostName,
            UserName = config.UserName,
            Password = config.Password
        };
        var connection = connectionFactory.CreateConnectionAsync();
        return connection;
    }
    
}
