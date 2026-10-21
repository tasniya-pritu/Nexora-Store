using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly ProductService _ps;

    public HomeController(ProductService ps)
    {
        _ps = ps;
    }

    public IActionResult Index(string search, string category, string sort, decimal? minPrice, decimal? maxPrice)
    {
        var products = _ps.GetAll();

        if (!string.IsNullOrWhiteSpace(search))
            products = products.Where(p =>
               p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
               p.Category.Contains(search, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(category) && category != "All")
            products = products.Where(p => p.Category == category);

        if (minPrice.HasValue)
            products = products.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            products = products.Where(p => p.Price <= maxPrice.Value);

        products = sort switch
        {
            "priceAsc" => products.OrderBy(p => p.Price),
            "priceDesc" => products.OrderByDescending(p => p.Price),
            _ => products
        };

        return View(products);
    }
}
