using Microsoft.AspNetCore.Mvc;
using GadgetShop.Models;

[Route("shop")]
public class ShopController : Controller
{
    private readonly ProductService _ps;

    public ShopController(ProductService ps)
    {
        _ps = ps;
    }

    [HttpGet("product/{id}")]
    public IActionResult Product(int id)
    {
        var product = _ps.Get(id);
        return Json(product);
    }

    [HttpGet("recommend/{id}")]
    public IActionResult Recommend(int id)
    {
        var p = _ps.Get(id);
        if (p == null)
            return Json(new { items = Array.Empty<Product>() });

        var related = _ps.GetRelatedByCategory(p.Category, 3);

        return Json(new { items = related });
    }

    [HttpPost("calculate")]
    public IActionResult Calculate([FromBody] PriceCalcRequest req)
    {
        var product = _ps.Get(req.ProductId);
        if (product == null) return BadRequest();

        decimal subtotal = product.Price * req.Quantity;
        decimal shipping = req.Shipping == "Express" ? 300 : 100;
        decimal discount = req.Coupon == "SAVE10" ? subtotal * 0.10m : 0;
        decimal vat = subtotal * 0.05m;

        decimal total = subtotal + shipping + vat - discount;

        return Json(new
        {
            subtotal,
            shipping,
            discount,
            vat,
            total
        });
    }
}

public class PriceCalcRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string Shipping { get; set; }
    public string Coupon { get; set; }
}
