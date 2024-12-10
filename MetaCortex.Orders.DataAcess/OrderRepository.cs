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
    public async Task<Order> CreateOrder(Order order)
    {
        await _orders.InsertOneAsync(order);
        return order;
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
        return await _orders.Find(p => p.Id == orderId).FirstOrDefaultAsync();
    }

    public async Task UpdateOrder(Order order)
    {
        await _orders.ReplaceOneAsync(o => o.Id == order.Id, order);
    }
}
