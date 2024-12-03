using MetaCortex.Orders.DataAcess.Interface;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MetaCortex.Orders.DataAcess.Entities;

public class EntityBase : IEntity<string>
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
}
