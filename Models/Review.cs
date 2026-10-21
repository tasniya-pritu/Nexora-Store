namespace GadgetShop.Models
{
    public class Review
    {
        public int ProductId { get; set; }
        public string UserName { get; set; }
        public int Stars { get; set; }
        public string Comment { get; set; }
    }
}
