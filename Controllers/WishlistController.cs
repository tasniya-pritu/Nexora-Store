using Microsoft.AspNetCore.Mvc;
using GadgetShop.Helpers;

public class WishlistController : Controller
{
    private readonly ProductService _ps;
    private const string KEY = "WISHLIST";

    public WishlistController(ProductService ps)
    {
        _ps = ps;
    }

    private List<int> GetList()
    {
        return HttpContext.Session.GetObject<List<int>>(KEY) ?? new List<int>();
    }

    private void SaveList(List<int> ids)
    {
        HttpContext.Session.SetObject(KEY, ids);
    }

    public IActionResult Index()
    {
        var ids = GetList();
        var products = _ps.GetAll().Where(p => ids.Contains(p.Id));
        return View(products);
    }

    [HttpPost]
    public IActionResult Add(int productId)
    {
        var list = GetList();
        if (!list.Contains(productId))
            list.Add(productId);

        SaveList(list);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Remove(int productId)
    {
        var list = GetList();
        list.Remove(productId);
        SaveList(list);

        return RedirectToAction("Index");
    }
}
