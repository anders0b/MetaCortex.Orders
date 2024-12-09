﻿using MetaCortex.Orders.DataAcess.Entities;
using System.Collections.Generic;
using System;

namespace MetaCortex.Orders.API.DTOs
{
    public class OrderDTO
    {
        public string Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
        public PaymentDTO PaymentPlan { get; set; } 
        public bool VIPStatus { get; set; }
        public List<Product> Products { get; set; }

    }
    public class PaymentDTO
    {
        public string PaymentMethod { get; set; } //fånga upp denna
        public bool IsPaid { get; set; } //fånga upp denna
    }
}
