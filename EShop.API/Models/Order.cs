using System;
using System.Collections.Generic;

namespace EShop.API.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending";
        public decimal TotalAmount { get; set; }

        public ICollection<OrderItem>? Items { get; set; }
    }
}