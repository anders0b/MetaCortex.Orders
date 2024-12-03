using MongoDB.Bson;

namespace MetaCortex.Orders.DataAcess.Entities
{
    public class Order : EntityBase
    {
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
        public bool PaymentStatus { get; set; }
        public bool ShippingStatus { get; set; }
        public List<string> Products { get; set; }
    }
}
