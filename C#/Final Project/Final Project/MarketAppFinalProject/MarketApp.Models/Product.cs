namespace MarketApp.Models
{
    public class Product
    {
        public string ProductId { get; set; } = Guid.NewGuid().ToString();
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
    }
}
