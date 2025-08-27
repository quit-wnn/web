using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    public class TreeShopController : Controller
    {
        private readonly UserDbContext _context;
        private readonly CartService _cart;

        public TreeShopController(UserDbContext context, CartService cart)
        {
            _context = context;
            _cart = cart;
        }

        public IActionResult Index(string searchString)
        {
            var trees = _context.trees.Where(t => t.IsAvailable);

            if (!string.IsNullOrEmpty(searchString))
            {
                trees = trees.Where(t => t.Name.Contains(searchString));
            }

            return View(trees.ToList());
        }
        public IActionResult AddToCart(int plantId)
        {
            int userId = GetCurrentUserId();

            var existingItem = _context.CartItems
                .FirstOrDefault(c => c.UserId == userId && c.PlantId == plantId);

            if (existingItem != null)
            {
                existingItem.Quantity += 1;
            }
            else
            {
                _context.CartItems.Add(new CartItem
                {
                    UserId = userId,
                    PlantId = plantId,
                    Quantity = 1
                });
            }

            _context.SaveChanges();
            return RedirectToAction("Cart");
        }

        public IActionResult Cart()
        {
            int userId = GetCurrentUserId();
            var cartItems = _cart.GetCart(userId);
            return View(cartItems);
        }

        public IActionResult Remove(int id)
        {
            _cart.RemoveFromCart(id);
            return RedirectToAction("Cart");
        }

        public IActionResult Checkout()
        {
            int userId = GetCurrentUserId();
            var cartItems = _cart.GetCart(userId);

            if (!cartItems.Any())
            {
                TempData["Message"] = "Giỏ hàng của TRAN đang trống!";
                return RedirectToAction("Cart");
            }

            _cart.ClearCart(userId);
            ViewBag.Message = "TRAN đã thanh toán thành công!";
            return View(cartItems);
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 1; // fallback giả lập
        }
    }
}
