namespace Web.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PlantId { get; set; }
        public int Quantity { get; set; }
        public Tree Tree { get; set; }
    }
}
