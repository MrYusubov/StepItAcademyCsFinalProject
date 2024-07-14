namespace MarketApp.Models
{
    public class Cart
    {
        public string CartId { get; set; } = Guid.NewGuid().ToString();
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
