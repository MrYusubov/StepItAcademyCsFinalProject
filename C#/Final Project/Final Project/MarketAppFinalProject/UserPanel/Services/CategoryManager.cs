using MarketApp.Models;
using System.Text.Json;
namespace UserPanel.Services
{
    public static class CategoryManager
    {
        public static List<Category> Categories { get; set; }

        static CategoryManager()
        {
            try
            {
                string fileDirectory = @"C:\Users\Yusub_sq38\Desktop\Final Project\Final Project\MarketAppFinalProject\AdminPanel\bin\Debug\net8.0"; //Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "C:\\Users\\Yusub_sq38\\Desktop\\Final Project\\Final Project\\MarketAppFinalProject\\AdminPanel\\bin\\Debug\\net8.0\\categories.json");
                string filePath = Path.Combine(fileDirectory, "categories.json");
                if (File.Exists(filePath))
                {
                    var json = File.ReadAllText(filePath);
                    var listOfCategories = JsonSerializer.Deserialize<List<Category>>(json);
                    if (listOfCategories is not null) Categories = listOfCategories;
                }
                Categories ??= new List<Category>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing CategoryManager: {ex.Message}");
                Categories = new List<Category>();
            }
        }
        public static List<Category> GetCategories()
        {
            return Categories;
        }
    }
}
