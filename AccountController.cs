using Microsoft.AspNetCore.Mvc;
using Web.Models;

public class AccountController : Controller
{
    private readonly UserDbContext _context;

    public AccountController(UserDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        string hashedPassword = HashPassword(password);

        var user = _context.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == hashedPassword);
        if (user == null)
        {
            ModelState.AddModelError("", "Sai tên tài khoản hoặc mật khẩu");
            return View();
        }

        HttpContext.Session.SetString("Username", username);
        return RedirectToAction("Profile");
    }

    public IActionResult Profile()
    {
        var username = HttpContext.Session.GetString("Username");
        if (string.IsNullOrEmpty(username))
        {
            return RedirectToAction("Login");
        }

        var user = _context.Users.FirstOrDefault(u => u.Username == username);
        return View(user);
    }
    [HttpGet]
    public IActionResult EditProfile()
    {
        var username = HttpContext.Session.GetString("Username");
        var user = _context.Users.FirstOrDefault(u => u.Username == username);
        return View(user);
    }

    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> EditProfile(User updatedUser, IFormFile AvatarImage)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == updatedUser.Id);
        if (user != null)
        {
            user.FullName = updatedUser.FullName;
            user.DateOfBirth = updatedUser.DateOfBirth;
            user.Hometown = updatedUser.Hometown;
            user.PhoneNumber = updatedUser.PhoneNumber;
            user.Bio = updatedUser.Bio;
            user.Email = updatedUser.Email;
            user.Username = updatedUser.Username;
            
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Profile");
    }


    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(string username, string email, string password)
    {
        if (_context.Users.Any(u => u.Username == username || u.Email == email))
        {
            ModelState.AddModelError("", "Tên đăng nhập hoặc email đã tồn tại");
            return View();
        }

        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = HashPassword(password),
            CreatedAt = DateTime.Now
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        HttpContext.Session.SetString("Username", username);
        return RedirectToAction("Profile");
    }

    private string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("Username");
        return RedirectToAction("Login");
    }

}
