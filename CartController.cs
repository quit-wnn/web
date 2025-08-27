using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Models;

public class CartController : Controller
{
    private readonly UserDbContext _context;

    public CartController(UserDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        int userId = GetUserId(); // Giả lập ID người dùng

        var cartItems = _context.CartItems
            .Include(c => c.Tree)
            .Where(c => c.UserId == userId)
            .ToList();

        return View(cartItems); // Trả về View Cart.cshtml
    }

    public IActionResult AddToCart(int plantId)
    {
        int userId = GetUserId();

        var existingItem = _context.CartItems
            .FirstOrDefault(c => c.UserId == userId && c.PlantId == plantId);

        if (existingItem != null)
        {
            existingItem.Quantity += 1;
        }
        else
        {
            var newItem = new CartItem
            {
                UserId = userId,
                PlantId = plantId,
                Quantity = 1
            };
            _context.CartItems.Add(newItem);
        }

        _context.SaveChanges();
        return RedirectToAction("Index", "TreeShop");
    }

    private int GetUserId()
    {
        return 1; // Giả lập người dùng
    }
}
