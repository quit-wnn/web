namespace Web.Models;
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? AvatarUrl { get; set; } // Đường dẫn ảnh đại diện
    public string? Bio { get; set; }       // Mô tả ngắn
    public string? FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Hometown { get; set; }
    public string? PhoneNumber { get; set; }

}