
using MetaCortex.Orders.DataAcess.Entities;

namespace MetaCortex.Orders.DataAcess
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrder(Order order);
        Task<Order> GetOrderById(string orderId);
        Task<IEnumerable<Order>> GetAllOrders();
        Task UpdateOrder(Order order);
        Task DeleteOrder(string orderId);
    }
}
