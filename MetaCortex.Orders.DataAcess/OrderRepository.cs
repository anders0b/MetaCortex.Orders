﻿using MetaCortex.Orders.DataAccess.Entities;
using MetaCortex.Orders.DataAcess.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MetaCortex.Orders.DataAcess;

public class OrderRepository : IOrderRepository
{
    private readonly IMongoCollection<Order> _orders;
    public OrderRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> mongoDbSettings)
    {
        var settings = mongoDbSettings.Value;
        var database = mongoClient.GetDatabase(settings.DatabaseName);
        _orders = database.GetCollection<Order>(settings.CollectionName, new MongoCollectionSettings { AssignIdOnInsert = true});
    }
    public async Task CreateOrder(Order order)
    {
        await _orders.InsertOneAsync(order);
    }

    public async Task DeleteOrder(string orderId)
    {
        var filter = Builders<Order>.Filter.Eq("OrderId", ObjectId.Parse(orderId));
        await _orders.DeleteOneAsync(filter);
    }

    public async Task<IEnumerable<Order>> GetAllOrders()
    {
        return await _orders.Find(new BsonDocument()).ToListAsync();
    }

    public async Task<Order> GetOrderById(string orderId)
    {
        var filter = Builders<Order>.Filter.Eq("OrderId", ObjectId.Parse(orderId));
        return await _orders.Find(filter).FirstOrDefaultAsync();
    }

    public async Task UpdateOrder(Order order)
    {
        await _orders.ReplaceOneAsync(o => o.Id == order.Id, order);
    }
}
