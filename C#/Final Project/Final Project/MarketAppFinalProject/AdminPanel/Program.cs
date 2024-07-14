using AdminPanel.Services;
using MarketApp.Models;
using System.Text.Json;

public class Program
{
    public static void AdminLogin()
    {
    AdminLogin:
        Console.Clear();
        Console.WriteLine("*********Admin Sign In*********");
        Console.Write("Email: ");
        var email = Console.ReadLine();
        Console.Write("Password: ");
        var password = Console.ReadLine();

        if (email == "admin" && password == "admin")
        {
            Console.WriteLine("Admin login successful!");
        }
        else
        {
            Console.WriteLine("Invalid email or password");
            Thread.Sleep(2000);
            goto AdminLogin;
        }
    }

    public static void AdminMainPage()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Add Category");
            Console.WriteLine("2. Add Product");
            Console.WriteLine("3. Update Product Detail");
            Console.WriteLine("4. Update Stocks");
            Console.WriteLine("5. View Products");
            Console.WriteLine("6. View Report");
            Console.WriteLine("7. Logout");

            Console.Write("Enter your choice: ");
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddCategory();
                    break;
                case "2":
                    AddProduct();
                    break;
                case "3":
                    UpdateProduct();
                    break;
                case "4":
                    UpdateStock();
                    break;
                case "5":
                    ViewProducts();
                    break;
                case "6":

                    break;
                case "7":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    public static void AddCategory()
    {
        try
        {
            Console.Clear();
            Console.Write("Enter category name: ");
            var name = Console.ReadLine();
            CategoryManager.AddCategory(name!);
            Console.WriteLine("Category added successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            Thread.Sleep(2000);
        }
    }

    public static void AddProduct()
    {
        try
        {
            Console.Clear();
            Console.Write("Enter product name: ");
            var name = Console.ReadLine();
            Console.Write("Enter product price: ");
            var price = double.Parse(Console.ReadLine()!);
            Console.Write("Enter product description: ");
            var description = Console.ReadLine();
            Console.Write("Enter the Stock: ");
            var stock = Convert.ToInt32(Console.ReadLine());
            var Categories = new List<Category>();
            if (File.Exists("categories.json"))
            {
                var json = File.ReadAllText("categories.json");
                var listOfCategories = JsonSerializer.Deserialize<List<Category>>(json);
                if (listOfCategories is not null) Categories = listOfCategories;
            }
            Console.Write("Enter category name: ");
            var categoryName = Console.ReadLine();
            var selectedCategory = Categories.FirstOrDefault(p => p.CategoryName.ToLower().Trim() == categoryName!.ToLower().Trim());
            var categoryId = selectedCategory!.CategoryId;
            ProductManager.AddProduct(name!, price, description!, categoryId, stock);
            Console.WriteLine("Product added successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            Thread.Sleep(2000);
        }
    }

    public static void UpdateProduct()
    {
        try
        {
            Console.Clear();
            var Products = new List<Product>();
            if (File.Exists("products.json"))
            {
                var json = File.ReadAllText("products.json");
                var listOfProducts = JsonSerializer.Deserialize<List<Product>>(json);
                if (listOfProducts is not null) Products = listOfProducts;
            }
            Console.Write("Enter product name to update: ");
            var productName = Console.ReadLine();
            var selectedProduct = Products.FirstOrDefault(p => p.ProductName.ToLower().Trim() == productName!.ToLower().Trim());
            var productId = selectedProduct!.ProductId;
            Console.Write("Enter new product name: ");
            var name = Console.ReadLine();
            Console.Write("Enter new product price: ");
            var price = double.Parse(Console.ReadLine()!);
            Console.Write("Enter new product description: ");
            var description = Console.ReadLine();
            ProductManager.UpdateProduct(productId!, name!, price, description!);
            Console.WriteLine("Product updated successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            Thread.Sleep(2000);
        }
    }

    public static void UpdateStock()
    {
        try
        {
            var Products = new List<Product>();
            if (File.Exists("products.json"))
            {
                var json = File.ReadAllText("products.json");
                var listOfProducts = JsonSerializer.Deserialize<List<Product>>(json);
                if (listOfProducts is not null) Products = listOfProducts;
            }
            Console.Write("Enter product name to update stock: ");
            var productName = Console.ReadLine();
            Console.Write("Enter the new stock: ");
            var productStock = Convert.ToInt32(Console.ReadLine()!);
            var selectedProduct = Products.FirstOrDefault(p => p.ProductName.ToLower().Trim() == productName!.ToLower().Trim());
            var productId = selectedProduct!.ProductId;
            ProductManager.UpdateStock(productId!, productStock!);
            Console.WriteLine("Product stock updated successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            Thread.Sleep(2000);
        }
    }

    public static void ViewProducts()
    {
        try
        {
            Console.Clear();
            var products = ProductManager.Products;
            foreach (var product in products)
            {
                Console.WriteLine($"ID: {product.ProductId}");
                Console.WriteLine($"Name: {product.ProductName}");
                Console.WriteLine($"Price: {product.Price}");
                Console.WriteLine($"Description: {product.Description}");
                Console.WriteLine($"Stock: {product.Stock}");
                Console.WriteLine("-------------------------------");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }
    }

    public static void Main(string[] args)
    {
        try
        {
            AdminLogin();
            AdminMainPage();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Critical Error: {ex.Message}");
        }
    }
}
