using Microsoft.AspNetCore.Mvc;
using SportsStore.Data;
using SportsStore.Models;
using System.Text.Json;

namespace SportsStore.Controllers
{
    public class CartController : Controller
    {
        private readonly IStoreRepository _repo;
        private readonly SportsStoreDbContext _context;

        public CartController(IStoreRepository repo, SportsStoreDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        private Cart GetCart()
        {
            var json = HttpContext.Session.GetString("Cart");
            if (json == null) return new Cart();

            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return System.Text.Json.JsonSerializer.Deserialize<Cart>(json, options) ?? new Cart();
        }

        private void SaveCart(Cart cart) =>
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult AddToCart(int productId)
        {
            var product = _repo.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                var cart = GetCart();
                cart.AddItem(product, 1);
                SaveCart(cart);
            }
            return Json(new { success = true });
        }

        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (!cart.Lines.Any())
                return RedirectToAction("List", "Product");

            ViewBag.Cart = cart;
            return View(new Order());
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Checkout(Order order)
        {
            var cart = GetCart();
            if (!cart.Lines.Any())
                ModelState.AddModelError("", "Your cart is empty");

            if (ModelState.IsValid)
            {
                try
                {
                    order.OrderDate = DateTime.UtcNow;
                    order.Lines = cart.Lines.Select(l => new OrderLine
                    {
                        ProductId = (int)l.Product.ProductId,
                        ProductName = l.Product.Name,
                        Price = l.Product.Price,
                        Quantity = l.Quantity
                    }).ToList();

                    _context.Orders.Add(order);
                    _context.SaveChanges();

                    int orderId = (int)order.OrderId;
                    cart.Clear();
                    SaveCart(cart);

                    return RedirectToAction("Completed", new { orderId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message} | Inner: {ex.InnerException?.Message}");
                }
            }

            ViewBag.Cart = cart;
            return View(order);
        }

        public IActionResult Completed(int orderId)
        {
            return View(orderId);
        }
    }
}