using GadgetShop.Models;

public class ProductService
{
    private List<Product> _products;

    public ProductService()
    {
        _products = new List<Product>()
        {
            // 5 Laptops (Mac & Windows)
            new Product { Id = 1, Name = "MacBook Air M2", Category = "Laptop", Price = 135000, Stock = 7, Image = "/images/macbook_air_m2.jpg" },
            new Product { Id = 2, Name = "MacBook Pro 14\"", Category = "Laptop", Price = 210000, Stock = 5, Image = "/images/MacBook Pro 14.jpg" },
            new Product { Id = 3, Name = "Dell XPS 13", Category = "Laptop", Price = 145000, Stock = 6, Image = "/images/dell_xps_13.jpg" },
            new Product { Id = 4, Name = "HP Envy 15", Category = "Laptop", Price = 125000, Stock = 8, Image = "/images/hp_envy_15.jpg" },
            new Product { Id = 5, Name = "Windows", Category = "Laptop", Price = 98000, Stock = 10, Image = "/images/windows.jpg" },

            // 5 iPhones
            new Product { Id = 6, Name = "iPhone 17", Category = "iPhone", Price = 78000, Stock = 12, Image = "/images/Iphone 17.jpg" },
            new Product { Id = 7, Name = "iPhone 16 Pro", Category = "iPhone", Price = 105000, Stock = 6, Image = "/images/iphone16_pro.jpg" },
            new Product { Id = 8, Name = "iPhone 16", Category = "iPhone", Price = 115000, Stock = 9, Image = "/images/iphone16.jpg" },
            new Product { Id = 9, Name = "iPhone 14 Pro", Category = "iPhone", Price = 145000, Stock = 4, Image = "/images/iphone14_pro.jpg" },
            new Product { Id = 10, Name = "iPhone 17 Air", Category = "iPhone", Price = 55000, Stock = 15, Image = "/images/apple-iphone-air-2.jpg" },

            // 5 Android Phones
            new Product { Id = 11, Name = "Samsung Galaxy S25 Ultra", Category = "Android", Price = 95000, Stock = 10, Image = "/images/galaxy_s25.jpg" },
            new Product { Id = 12, Name = "Realme C55", Category = "Android", Price = 4500, Stock = 18, Image = "/images/Realme C55.jpg" },
            new Product { Id = 13, Name = "Honor 200 5G", Category = "Android", Price = 3299, Stock = 20, Image = "/images/Honor 200 5G.jpg" },
            new Product { Id = 14, Name = "Vivo x300", Category = "Android", Price = 98000, Stock = 14, Image = "/images/vivo x300.jpg" },
            new Product { Id = 15, Name = "OnePlus 15", Category = "Android", Price = 152000, Stock = 9, Image = "/images/oneplus-15.jpg" },

            // 5 Accessories
            new Product { Id = 16, Name = "Soundcore P30i", Category = "Accessory", Price = 28000, Stock = 20, Image = "/images/soundcore P30i.jpg" },
            new Product { Id = 17, Name = "Apple Magic Mouse", Category = "Accessory", Price = 15999, Stock = 40, Image = "/images/Apple Magic Mouse.jpg" },
            new Product { Id = 18, Name = "Anker PowerBank 20,000mAh", Category = "Accessory", Price = 8500, Stock = 25, Image = "/images/joyroom.jpg" },
            new Product { Id = 19, Name = "USB-C Fast Charger 25W", Category = "Accessory", Price = 1200, Stock = 30, Image = "/images/AC.jpg" },
            new Product { Id = 20, Name = "Laptop Backpack (Waterproof)", Category = "Accessory", Price = 2500, Stock = 16, Image = "/images/Bange-BG.jpg" },
        };
    }

    public IEnumerable<Product> GetAll() => _products;

    public Product Get(int id) =>
        _products.FirstOrDefault(p => p.Id == id);

    public IEnumerable<Product> GetRelatedByCategory(string category, int count = 3)
    {
        return _products
            .Where(p => p.Category == category)
            .Take(count);
    }

    public void Add(Product p)
    {
        p.Id = _products.Max(x => x.Id) + 1;
        _products.Add(p);
    }

    public void Update(Product p)
    {
        var existing = _products.FirstOrDefault(x => x.Id == p.Id);
        if (existing == null) return;

        existing.Name = p.Name;
        existing.Price = p.Price;
        existing.Category = p.Category;
        existing.Stock = p.Stock;

        if (!string.IsNullOrEmpty(p.Image))
            existing.Image = p.Image;
    }

    public void Delete(int id)
    {
        _products.RemoveAll(x => x.Id == id);
    }
}
