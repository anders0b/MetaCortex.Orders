using System.Threading.Tasks;

namespace MetaCortex.Orders.API.Interface
{
    public interface IOrderUpdaterService
    {
        Task UpdateOrderVipStatus(string order);
        Task UpdateOrderPaymentStatus(string order);
    }
}
