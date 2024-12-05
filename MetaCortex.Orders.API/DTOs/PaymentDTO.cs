using MetaCortex.Orders.DataAcess.Entities;
using System.Collections.Generic;
using System;

namespace MetaCortex.Orders.API.DTOs
{
    public class OrderDTO
    {
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
        public PaymentDTO Payment { get; set; }
        public bool VIPStatus { get; set; }
        public List<string> Products { get; set; }
        
    }
    public class PaymentDTO
    {
        public string OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public bool IsPaid { get; set; } //fånga upp denna

    }
}
