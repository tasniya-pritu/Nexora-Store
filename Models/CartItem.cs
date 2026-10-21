using System.ComponentModel.DataAnnotations;

namespace GadgetShop.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public decimal UnitPrice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        public decimal Total => UnitPrice * Quantity;
    }
}
