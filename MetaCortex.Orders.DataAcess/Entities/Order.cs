
namespace MetaCortex.Orders.DataAcess.Entities
{
    public class Order : EntityBase
    {
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
        public string PaymentMethod { get; set; }
        public bool IsPaid { get; set; }
        public bool VIPStatus { get; set; }
        public List<Product> Products { get; set; }
    }
}
