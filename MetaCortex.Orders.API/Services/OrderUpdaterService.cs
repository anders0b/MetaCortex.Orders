using MetaCortex.Orders.API.DTOs;
using MetaCortex.Orders.API.Interface;
using MetaCortex.Orders.DataAcess;
using MetaCortex.Orders.DataAcess.Entities;
using MetaCortex.Orders.DataAcess.MessageBroker;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MetaCortex.Orders.API.Services;

public class OrderUpdaterService : IOrderUpdaterService
{
    private readonly IOrderRepository _repository;
    private readonly ILogger<OrderUpdaterService> _logger;
    private readonly IMessageProducerService _producerService;
    public OrderUpdaterService(IOrderRepository repository, ILogger<OrderUpdaterService> logger, IMessageProducerService producerService)
    {
        _repository = repository;
        _logger = logger;
        _producerService = producerService;
    }
    public async Task UpdateOrderVipStatus(string order)
    {
        if(string.IsNullOrEmpty(order)) throw new ArgumentException("Order Id cannot be null", nameof(order));

        var orderDto = JsonSerializer.Deserialize<Order>(order) ?? throw new InvalidOperationException("Failed to deserialize order");

        var originalOrder = await _repository.GetOrderById(orderDto.Id) ?? throw new InvalidOperationException("Order not found");
            
        originalOrder.VIPStatus = orderDto.VIPStatus;

        await _repository.UpdateOrder(originalOrder);

        _logger.LogInformation($"[INFO] VIP status is: {orderDto.VIPStatus}. Updated for order {orderDto.Id}");

        await _producerService.SendMessageAsync(originalOrder, "order-to-payment");
        _logger.LogInformation($"[OUTGOING] Order {originalOrder.Id} sent to Payment-channel");
    }

    public async Task UpdateOrderPaymentStatus(string order)
    {
        if(string.IsNullOrEmpty(order)) throw new ArgumentException("Order Id cannot be null", nameof(order));

        var finalOrderDto = JsonSerializer.Deserialize<OrderDTO>(order) ?? throw new InvalidOperationException("Failed to deserialize order");

        var originalOrder = await _repository.GetOrderById(finalOrderDto.Id) ?? throw new InvalidOperationException("Order not found");

        originalOrder.IsPaid = finalOrderDto.PaymentPlan.IsPaid;

        originalOrder.PaymentMethod = finalOrderDto.PaymentPlan.PaymentMethod;

        await _repository.UpdateOrder(originalOrder);

        _logger.LogInformation($"[INFO] Payment completed. Order: {finalOrderDto.Id} is to be sent out for delivery. Sending {originalOrder.Id} to Product-queue");


        await _producerService.SendMessageAsync(originalOrder.Products, "order-to-products");
        _logger.LogInformation($"[OUTGOING] Sending order products from order: {originalOrder.Id} to Products-queue");
    }
}
