namespace MarketApp.Models
{
    public class CartItem
    {
        public string CartItemId { get; set; } = Guid.NewGuid().ToString();
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
