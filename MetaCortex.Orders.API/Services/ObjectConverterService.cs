using MetaCortex.Orders.API.DTOs;
using MetaCortex.Orders.DataAcess;
using MetaCortex.Orders.DataAcess.Entities;
using System.Text.Json;
using System.Threading.Tasks;

namespace MetaCortex.Orders.API.Services;

public class ObjectConverterService
{
    private readonly IOrderRepository _repository;
    public ObjectConverterService(IOrderRepository repository)
    {
        _repository = repository;
    }
    public async Task CheckVIP(string order)
    {
        var orderDto = JsonSerializer.Deserialize<VIPOrderDTO>(order);

        if (orderDto != null)
        {
            var originalOrder = await _repository.GetOrderById(orderDto.Id);
            if (originalOrder != null)
            {
                originalOrder.VIPStatus = orderDto.IsVIP;
                await _repository.UpdateOrder(originalOrder);
            }
            else
            {
                throw new System.Exception("Order not found");
            }
        }
        else
        {
            throw new System.Exception("Order cannot be null");
        }
    }
    public async Task SaveFinalOrderFromPayment(string order)
    {
        var finalOrderDto = JsonSerializer.Deserialize<OrderDTO>(order);
        if (finalOrderDto != null)
        {
            var originalOrder = await _repository.GetOrderById(finalOrderDto.Payment.OrderId);
            if (originalOrder != null)
            {
                originalOrder.IsPaid = finalOrderDto.Payment.IsPaid;
                originalOrder.PaymentMethod = finalOrderDto.Payment.PaymentMethod;
                await _repository.UpdateOrder(originalOrder);
            }
            else
            {
                throw new System.Exception("Order not found");
            }
        }
    }
    public async Task UpdateVIPStatus(string order)
    {
        var updatedOrderDto = JsonSerializer.Deserialize<VIPOrderDTO>(order);
        if (updatedOrderDto != null)
        {
            var originalOrder = await _repository.GetOrderById(updatedOrderDto.Id);
            if (originalOrder != null)
            {
                originalOrder.VIPStatus = updatedOrderDto.IsVIP;
                await _repository.UpdateOrder(originalOrder);
            }
            else
            {
                throw new System.Exception("Order not found");
            }
        }
    }
}
