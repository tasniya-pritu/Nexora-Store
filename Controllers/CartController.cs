using Microsoft.AspNetCore.Mvc;
using GadgetShop.Models;
using GadgetShop.Helpers;

public class CartController : Controller
{
    private readonly ProductService _ps;
    private readonly OrderService _os;
    private const string CART_KEY = "CART";

    public CartController(ProductService ps, OrderService os)
    {
        _ps = ps;
        _os = os;
    }

    private List<CartItem> GetCart()
    {
        return HttpContext.Session.GetObject<List<CartItem>>(CART_KEY)
               ?? new List<CartItem>();
    }

    private void SaveCart(List<CartItem> cart)
    {
        HttpContext.Session.SetObject(CART_KEY, cart);
    }

    // ================================
    // SHOW CART
    // ================================
    public IActionResult Index()
    {
        return View(GetCart());
    }

    // ================================
    // ADD TO CART (VALIDATED)
    // ================================
    [HttpPost]
    public IActionResult Add(int productId, int qty = 1)
    {
        var product = _ps.Get(productId);
        if (product == null) return NotFound();

        // ❌ Invalid quantity
        if (qty < 1)
        {
            TempData["Error"] = "Quantity must be at least 1";
            return RedirectToAction("Index");
        }

        var cart = GetCart();
        var existing = cart.FirstOrDefault(c => c.ProductId == productId);

        int totalQty = qty;
        if (existing != null)
            totalQty = existing.Quantity + qty;

        // ❌ Stock exceeded
        if (totalQty > product.Stock)
        {
            TempData["Error"] =
                $"Only {product.Stock} items available for {product.Name}";
            return RedirectToAction("Index");
        }

        if (existing == null)
        {
            cart.Add(new CartItem
            {
                ProductId = product.Id,
                Name = product.Name,
                UnitPrice = product.Price,
                Quantity = qty
            });
        }
        else
        {
            existing.Quantity += qty;
        }

        SaveCart(cart);
        return RedirectToAction("Index");
    }

    // ================================
    // UPDATE CART ITEM (VALIDATED)
    // ================================
    [HttpPost]
    public IActionResult Update(int productId, int qty)
    {
        var product = _ps.Get(productId);
        if (product == null) return NotFound();

        // ❌ Invalid quantity
        if (qty < 1)
        {
            TempData["Error"] = "Quantity must be at least 1";
            return RedirectToAction("Index");
        }

        // ❌ Stock exceeded
        if (qty > product.Stock)
        {
            TempData["Error"] =
                $"Only {product.Stock} items available for {product.Name}";
            return RedirectToAction("Index");
        }

        var cart = GetCart();
        var item = cart.FirstOrDefault(c => c.ProductId == productId);

        if (item != null)
            item.Quantity = qty;

        SaveCart(cart);
        return RedirectToAction("Index");
    }

    // ================================
    // REMOVE ITEM
    // ================================
    [HttpPost]
    public IActionResult Remove(int productId)
    {
        var cart = GetCart();
        cart.RemoveAll(c => c.ProductId == productId);

        SaveCart(cart);
        return RedirectToAction("Index");
    }

    // ================================
    // CLEAR CART
    // ================================
    [HttpPost]
    public IActionResult Clear()
    {
        SaveCart(new List<CartItem>());
        return RedirectToAction("Index");
    }

    // ================================
    // SHOW CHECKOUT PAGE
    // ================================
    public IActionResult Checkout()
    {
        var cart = GetCart();
        if (!cart.Any())
            return RedirectToAction("Index");

        return View(cart);
    }

    // ================================
    // CONFIRM CHECKOUT + STOCK UPDATE
    // ================================
    [HttpPost]
    public IActionResult Checkout(string customerName, string address, string paymentMethod)
    {
        var cart = GetCart();
        if (!cart.Any())
            return RedirectToAction("Index");

        // 🔒 FINAL STOCK VALIDATION
        foreach (var item in cart)
        {
            var product = _ps.Get(item.ProductId);

            if (product == null)
            {
                TempData["Error"] = "Invalid product in cart";
                return RedirectToAction("Index");
            }

            if (item.Quantity < 1)
            {
                TempData["Error"] = "Invalid quantity in cart";
                return RedirectToAction("Index");
            }

            if (item.Quantity > product.Stock)
            {
                TempData["Error"] =
                    $"Only {product.Stock} items available for {product.Name}";
                return RedirectToAction("Index");
            }
        }

        decimal subtotal = cart.Sum(c => c.Total);
        decimal vat = subtotal * 0.05m;
        decimal shipping = 100;
        decimal discount = 0;
        decimal total = subtotal + vat + shipping - discount;

        // CREATE ORDER
        var order = new Order
        {
            CustomerName = customerName,
            Address = address,
            PaymentMethod = paymentMethod,
            Subtotal = subtotal,
            Vat = vat,
            Shipping = shipping,
            Discount = discount,
            Total = total,
            Items = cart.Select(c => new OrderItem
            {
                ProductId = c.ProductId,
                Name = c.Name,
                UnitPrice = c.UnitPrice,
                Quantity = c.Quantity
            }).ToList()
        };

        _os.Add(order);

        // 🔥 STOCK REDUCE (SAFE)
        foreach (var item in cart)
        {
            var product = _ps.Get(item.ProductId);
            product.Stock -= item.Quantity;
            _ps.Update(product);
        }

        // CLEAR CART
        SaveCart(new List<CartItem>());

        return RedirectToAction("Track", "Order", new { id = order.Id });
    }
    [HttpPost]
    public IActionResult UpdateAjax(int productId, int qty)
    {
        var product = _ps.Get(productId);
        if (product == null)
            return Json(new { success = false, message = "Product not found" });

        if (qty < 1)
            return Json(new { success = false, message = "Quantity must be at least 1" });

        if (qty > product.Stock)
            return Json(new
            {
                success = false,
                message = $"Only {product.Stock} items available for {product.Name}"
            });

        var cart = GetCart();
        var item = cart.FirstOrDefault(c => c.ProductId == productId);

        if (item != null)
            item.Quantity = qty;

        SaveCart(cart);

        return Json(new { success = true });
    }

}
