using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public string UserId { get; set; } = string.Empty; // nếu có đăng nhập
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public decimal TotalAmount { get; set; }

        public List<Order> Orders { get; set; } = new();
    }
}
