using RabbitMQ.Client;
using System.Threading.Tasks;

namespace MetaCortex.Orders.API.InterfaceM
{
    public interface IRabbitMqService
    {
        Task<IConnection> CreateConnection();
    }
}
