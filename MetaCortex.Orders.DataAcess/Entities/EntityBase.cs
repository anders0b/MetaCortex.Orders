using MetaCortex.Orders.DataAcess.Interface;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MetaCortex.Orders.DataAcess.Entities;

public class EntityBase : IEntity<ObjectId>
{
    [BsonId]
    public ObjectId Id { get; set; }
}
