using MetaCortex.Orders.DataAcess;
using MetaCortex.Orders.DataAcess.Entities;
using MetaCortex.Orders.DataAcess.MessageBroker;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
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
            await producerService.SendMessageAsync(order, "order-to-payment");
            await producerService.SendMessageAsync(order, "order-to-customer");
            await repository.CreateOrder(order);
            return Results.Created($"/api/orders/{order.Id}", order);
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
            if(order.IsPaid == true)
            {
                await producerService.SendMessageAsync(order, "order-to-products");
            }
            order.Id = existingOrder.Id;
            await repository.UpdateOrder(order);
            return Results.Ok(order);
        }
    }
}
