using MetaCortex.Orders.API.DTOs;
using MetaCortex.Orders.DataAcess;
using MetaCortex.Orders.DataAcess.Entities;
using MetaCortex.Orders.DataAcess.MessageBroker;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MetaCortex.Orders.API.Services;

public class ObjectConverterService
{
    private readonly IOrderRepository _repository;
    private readonly ILogger<ObjectConverterService> _logger;
    private readonly IMessageProducerService _producerService;
    public ObjectConverterService(IOrderRepository repository, ILogger<ObjectConverterService> logger, IMessageProducerService producerService)
    {
        _repository = repository;
        _logger = logger;
        _producerService = producerService;
    }
    public async Task CheckVIP(string order)
    {
        if(string.IsNullOrEmpty(order)) throw new ArgumentException("Order Id cannot be null", nameof(order));

        var orderDto = JsonSerializer.Deserialize<Order>(order) ?? throw new InvalidOperationException("Failed to deserialize order");

        var originalOrder = await _repository.GetOrderById(orderDto.Id) ?? throw new InvalidOperationException("Order not found");
            
        originalOrder.VIPStatus = orderDto.VIPStatus;

        await _repository.UpdateOrder(originalOrder);

        _logger.LogInformation($"VIP status is: {orderDto.VIPStatus}. Updated for order {orderDto.Id}");

        await _producerService.SendMessageAsync(originalOrder, "order-to-payment");
        _logger.LogInformation("Order sent to Payment-channel");
    }

    public async Task SaveFinalOrderFromPayment(string order)
    {
        if(string.IsNullOrEmpty(order)) throw new ArgumentException("Order Id cannot be null", nameof(order));

        var finalOrderDto = JsonSerializer.Deserialize<OrderDTO>(order) ?? throw new InvalidOperationException("Failed to deserialize order");

        var originalOrder = await _repository.GetOrderById(finalOrderDto.Id) ?? throw new InvalidOperationException("Order not found");

        originalOrder.IsPaid = finalOrderDto.Payment.IsPaid;

        originalOrder.PaymentMethod = finalOrderDto.Payment.PaymentMethod;

        await _repository.UpdateOrder(originalOrder);

        _logger.LogInformation($"Final order saved {finalOrderDto.Id}. Sending {originalOrder.Id} to Product");

        await _producerService.SendMessageAsync(originalOrder.Products, "order-to-products");
        _logger.LogInformation($"Sent {originalOrder.Id} to Product-queue");
    }
}
