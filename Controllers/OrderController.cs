using Microsoft.AspNetCore.Mvc;
using GadgetShop.Models;

public class OrderController : Controller
{
    private readonly OrderService _os;

    public OrderController(OrderService os)
    {
        _os = os;
    }

    // ===============================
    // ORDER TRACK
    // ===============================
    public IActionResult Track(string id)
    {
        if (string.IsNullOrEmpty(id))
            return NotFound();

        var order = _os.Get(id);
        if (order == null)
            return NotFound();

        return View(order);
    }

    // ===============================
    // INVOICE
    // ===============================
    public IActionResult Invoice(string id)
    {
        if (string.IsNullOrEmpty(id))
            return NotFound();

        var order = _os.Get(id);
        if (order == null)
            return NotFound();

        return View(order);
    }
}
