using MarketApp.Models;
using System.Text.Json;

namespace AdminPanel.Services
{
    public static class ProductManager
    {
        public static List<Product> Products { get; set; }

        static ProductManager()
        {
            try
            {
                if (File.Exists("products.json"))
                {
                    var json = File.ReadAllText("products.json");
                    var listOfProducts = JsonSerializer.Deserialize<List<Product>>(json);
                    if (listOfProducts is not null) Products = listOfProducts;
                }
                Products ??= new List<Product>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing ProductManager: {ex.Message}");
                Products = new List<Product>();
            }
        }

        public static void AddProduct(string name, double price, string description, string categoryId, int stock)
        {
            try
            {
                var product = new Product
                {
                    ProductName = name,
                    Price = price,
                    Description = description,
                    ProductId = categoryId,
                    Stock = stock
                };
                Products.Add(product);

                var json = JsonSerializer.Serialize(Products);
                File.WriteAllText("products.json", json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding product: {ex.Message}");
            }
        }

        public static void UpdateProduct(string productId, string name, double price, string description)
        {
            try
            {
                var product = Products.FirstOrDefault(p => p.ProductId == productId);
                if (product != null)
                {
                    product.ProductName = name;
                    product.Price = price;
                    product.Description = description;

                    var json = JsonSerializer.Serialize(Products);
                    File.WriteAllText("products.json", json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product: {ex.Message}");
            }
        }

        public static void UpdateStock(string productId, int stock)
        {
            try
            {
                var product = Products.FirstOrDefault(p => p.ProductId == productId);
                if (product != null)
                {
                    product.Stock = stock;

                    var json = JsonSerializer.Serialize(Products);
                    File.WriteAllText("products.json", json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating stock: {ex.Message}");
            }
        }

        public static List<Product> GetProductsByCategory(string categoryId)
        {
            try
            {
                return Products.Where(p => p.ProductId == categoryId && p.Stock > 0).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting products by category: {ex.Message}");
                return new List<Product>();
            }
        }
    }
}
