using MetaCortex.Orders.DataAcess.Entities;
using System.Collections.Generic;
using System;

namespace MetaCortex.Orders.API.DTOs
{
    public class OrderDTO
    {
        public required string Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string? CustomerId { get; set; }
        public required PaymentDTO? PaymentPlan { get; set; } 
        public bool VIPStatus { get; set; }
        public List<Product>? Products { get; set; }

    }
    public class PaymentDTO
    {
        public required string PaymentMethod { get; set; } //fånga upp denna
        public required bool IsPaid { get; set; } //fånga upp denna
    }
}
