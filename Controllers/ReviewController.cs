using Microsoft.AspNetCore.Mvc;
using GadgetShop.Models;

[Route("review")]
public class ReviewController : Controller
{
    private readonly ReviewService _rs;

    public ReviewController(ReviewService rs)
    {
        _rs = rs;
    }

    [HttpGet("list/{productId}")]
    public IActionResult List(int productId)
    {
        var items = _rs.GetByProduct(productId);
        return Json(items);
    }

    [HttpPost("add")]
    public IActionResult Add([FromBody] Review review)
    {
        _rs.Add(review);
        return Json(new { success = true });
    }
}
