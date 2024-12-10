using MetaCortex.Orders.API.DTOs;
using MetaCortex.Orders.DataAcess;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MetaCortex.Orders.API.Services;

public class ObjectConverterService
{
    private readonly IOrderRepository _repository;
    private readonly ILogger<ObjectConverterService> _logger;
    public ObjectConverterService(IOrderRepository repository, ILogger<ObjectConverterService> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    public async Task CheckVIP(string order)
    {
        if(string.IsNullOrEmpty(order)) throw new ArgumentException("Order Id cannot be null", nameof(order));

        var orderDto = JsonSerializer.Deserialize<VIPOrderDTO>(order) ?? throw new InvalidOperationException("Failed to deserialize order");

        var originalOrder = await _repository.GetOrderById(orderDto.Id) ?? throw new InvalidOperationException("Order not found");
            
        originalOrder.VIPStatus = orderDto.IsVIP;

        await _repository.UpdateOrder(originalOrder);

        _logger.LogInformation($"VIP status updated for order {orderDto.Id}");
    }

    public async Task SaveFinalOrderFromPayment(string order)
    {
        if(string.IsNullOrEmpty(order)) throw new ArgumentException("Order Id cannot be null", nameof(order));

        var finalOrderDto = JsonSerializer.Deserialize<OrderDTO>(order) ?? throw new InvalidOperationException("Failed to deserialize order");

        var originalOrder = await _repository.GetOrderById(finalOrderDto.Payment.OrderId) ?? throw new InvalidOperationException("Order not found");

        originalOrder.IsPaid = finalOrderDto.Payment.IsPaid;

        originalOrder.PaymentMethod = finalOrderDto.Payment.PaymentMethod;

        await _repository.UpdateOrder(originalOrder);

        _logger.LogInformation($"Final order saved {finalOrderDto.Payment.OrderId}");

    }
}
