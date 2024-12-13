using MetaCortex.Orders.API.DTOs;
using MetaCortex.Orders.DataAcess;
using MetaCortex.Orders.DataAcess.Entities;
using MetaCortex.Orders.DataAcess.MessageBroker;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace MetaCortex.Orders.API.Extensions
{
    public static class OrderEndpointExtensions
    {
        public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/orders");
            group.MapPost("", CreateOrder);
            group.MapGet("{orderId}", GetOrderById);
            group.MapGet("", GetAllOrders);
            group.MapDelete("{orderId}", DeleteOrder);
            group.MapPut("{orderId}", UpdateOrder);
            return app;
        }
        public static async Task<IResult> CreateOrder(IOrderRepository repository, IMessageProducerService producerService, Order order)
        {
            if(order == null)
            {
                return Results.BadRequest("Order cannot be null");
            }

            var addedOrder = await repository.CreateOrder(order);
            Console.WriteLine($"[INFO] Order: {addedOrder.Id} created and saved to database");

            await producerService.SendMessageAsync(addedOrder, "order-to-customer");
            Console.WriteLine($"[OUTGOING] Order: {addedOrder.Id} with Customer ID: {addedOrder.CustomerId} sent to Customer-channel for VIP-status check");

            return Results.Created($"/api/orders/{addedOrder.Id}", addedOrder);
        }

        public static async Task<IResult> GetOrderById(IOrderRepository repository, string orderId)
        {
            var order = await repository.GetOrderById(orderId);
            if (order == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(order);
        }
        public static async Task<IResult> GetAllOrders(IOrderRepository repository, IMessageProducerService messageProducerService)
        {
            var orders = await repository.GetAllOrders();
            return Results.Ok(orders);
        }
        public static async Task<IResult> DeleteOrder(IOrderRepository repository, string orderId)
        {
            await repository.DeleteOrder(orderId);
            return Results.NoContent();
        }
        public static async Task<IResult> UpdateOrder(IOrderRepository repository, IMessageProducerService producerService, string orderId, Order order)
        {
            if (order == null)
            {
                return Results.BadRequest("Order cannot be null");
            }
            var existingOrder = await repository.GetOrderById(orderId);

            if (existingOrder == null)
            {
                return Results.NotFound();
            }
            order.Id = existingOrder.Id;
            await repository.UpdateOrder(order);
            return Results.Ok(order);
        }
    }
}
