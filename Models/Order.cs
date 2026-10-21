using System;
using System.Collections.Generic;

namespace GadgetShop.Models
{
    public enum OrderStatus
    {
        Placed,
        Packed,
        Shipped,
        Delivered,
        Cancelled
    }

    public class OrderItem
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Total => UnitPrice * Quantity;
    }

    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString().Substring(0, 8);
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public OrderStatus Status { get; set; } = OrderStatus.Placed;

        public List<OrderItem> Items { get; set; } = new();

        public decimal Subtotal { get; set; }
        public decimal Vat { get; set; }
        public decimal Shipping { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
    }
}
