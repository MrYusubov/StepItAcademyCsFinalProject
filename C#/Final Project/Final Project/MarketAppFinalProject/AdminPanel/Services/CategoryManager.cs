using System.Text.Json;
using MarketApp.Models;
namespace AdminPanel.Services
{
    public static class CategoryManager
    {
        public static List<Category> Categories { get; set; }
        static CategoryManager()
        {
            try
            {
                if (File.Exists("categories.json"))
                {
                    var json = File.ReadAllText("categories.json");
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

        public static void AddCategory(string name)
        {
            try
            {
                var category = new Category
                {
                    CategoryId = Guid.NewGuid().ToString(),
                    CategoryName = name
                };
                Categories.Add(category);

                var json = JsonSerializer.Serialize(Categories);
                File.WriteAllText("categories.json", json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding category: {ex.Message}");
            }
        }

        public static List<Category> GetCategories()
        {
            return Categories;
        }
    }
}
