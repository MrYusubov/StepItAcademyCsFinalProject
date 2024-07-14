using MarketApp.Models;
using System.Text.Json;

namespace UserPanel.Services
{
    public static class ProductManager
    {
        public static List<Product> Products { get; set; }
       static string fileDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "C:\\Users\\Yusub_sq38\\Desktop\\Final Project\\Final Project\\MarketAppFinalProject\\AdminPanel\\bin\\Debug\\net8.0");
       static string filePath = Path.Combine(fileDirectory, "products.json");
        static ProductManager()
        {
            try
            {

                if (File.Exists(filePath))
                {
                    var json = File.ReadAllText(filePath);
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

        public static void UpdateStock(string productId, int stock)
        {
            try
            {
                var product = Products.FirstOrDefault(p => p.ProductId == productId);
                if (product != null)
                {
                    product.Stock = stock;

                    var json = JsonSerializer.Serialize(Products);
                    File.WriteAllText(filePath,json);
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