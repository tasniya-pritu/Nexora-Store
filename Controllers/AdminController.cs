using Microsoft.AspNetCore.Mvc;
using GadgetShop.Models;
using GadgetShop.Helpers;

[Route("admin")]
public class AdminController : Controller
{
    private readonly ProductService _ps;
    private readonly OrderService _os;
    private readonly IWebHostEnvironment _env;
    private readonly IConfiguration _configuration;

    public AdminController(ProductService ps, OrderService os, IWebHostEnvironment env, IConfiguration configuration)
    {
        _ps = ps;
        _os = os;
        _env = env;
        _configuration = configuration;
    }

    // ===============================
    // ADMIN LOGIN PAGE (GET)
    // ===============================
    [HttpGet("login")]
    public IActionResult Login()
    {
        // Already logged in → go to dashboard
        if (HttpContext.Session.GetString("admin") == "true")
            return RedirectToAction("Dashboard");

        return View();
    }

    // ===============================
    // ADMIN LOGIN SUBMIT (POST)
    // ===============================
    [HttpPost("login")]
    public IActionResult Login(string username, string password)
    {
        var adminUsername = _configuration["Admin:Username"];
        var adminPassword = _configuration["Admin:Password"];

        if (string.IsNullOrWhiteSpace(adminUsername) || string.IsNullOrWhiteSpace(adminPassword))
        {
            ViewBag.Error = "Admin login is not configured. Set the Admin__Username and Admin__Password environment variables.";
            return View();
        }

        if (username == adminUsername && password == adminPassword)
        {
            HttpContext.Session.SetString("admin", "true");
            return RedirectToAction("Dashboard");
        }

        ViewBag.Error = "Invalid username or password!";
        return View();
    }

    // ===============================
    // ADMIN LOGOUT
    // ===============================
    [HttpGet("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("admin");
        return RedirectToAction("Login");
    }

    // ===============================
    // CHECK ADMIN AUTH (PRIVATE FUNCTION)
    // ===============================
    private bool IsAdmin()
    {
        return HttpContext.Session.GetString("admin") == "true";
    }

    // ===============================
    // DASHBOARD (SECURE)
    // ===============================
    [HttpGet("")]
    public IActionResult Dashboard()
    {
        if (!IsAdmin()) return RedirectToAction("Login");

        var lowStock = _ps.GetAll().Where(p => p.Stock < 5).ToList();
        var allProducts = _ps.GetAll().ToList();

        ViewBag.TotalSales = _os.GetAll().Sum(o => o.Total);
        ViewBag.TotalOrders = _os.GetAll().Count();
        ViewBag.LowStock = lowStock;

        return View(allProducts);  // <-- SEND PRODUCTS TO DASHBOARD VIEW
    }


    // ===============================
    // PRODUCT LIST (SECURE)
    // ===============================
    [HttpGet("products")]
    public IActionResult Products()
    {
        if (!IsAdmin()) return RedirectToAction("Login");
        return View(_ps.GetAll());
    }

    // ===============================
    // CREATE PRODUCT (SECURE)
    // ===============================
    [HttpGet("create")]
    public IActionResult Create()
    {
        if (!IsAdmin()) return RedirectToAction("Login");
        return View();
    }

    [HttpPost("create")]
    public IActionResult Create(Product p, IFormFile img)
    {
        if (!IsAdmin()) return RedirectToAction("Login");

        if (img != null)
        {
            string fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
            string filePath = Path.Combine(_env.WebRootPath, "images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                img.CopyTo(stream);
            }

            p.Image = "/images/" + fileName;
        }

        _ps.Add(p);
        return RedirectToAction("Products");
    }

    // ===============================
    // EDIT PRODUCT (SECURE)
    // ===============================
    [HttpGet("edit/{id}")]
    public IActionResult Edit(int id)
    {
        if (!IsAdmin()) return RedirectToAction("Login");

        var p = _ps.Get(id);
        return View(p);
    }

    [HttpPost("edit")]
    public IActionResult Edit(Product p, IFormFile img)
    {
        if (!IsAdmin()) return RedirectToAction("Login");

        if (img != null)
        {
            string fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
            string filePath = Path.Combine(_env.WebRootPath, "images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                img.CopyTo(stream);
            }

            p.Image = "/images/" + fileName;
        }

        _ps.Update(p);
        return RedirectToAction("Products");
    }

    // ===============================
    // DELETE PRODUCT (SECURE)
    // ===============================
    [HttpPost("delete")]
    public IActionResult Delete(int id)
    {
        if (!IsAdmin()) return RedirectToAction("Login");

        _ps.Delete(id);
        return RedirectToAction("Products");
    }
}
