using MarketApp.Models;
using System.Text.Json;

namespace UserPanel.Services
{
    public static class CartManager
    {
        public static List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public static List<CartItem> CartHistory { get; set; } = new List<CartItem>();

        static CartManager()
        {
            try
            {
                string fileDirectory = @"C:\Users\Yusub_sq38\Desktop\Final Project\Final Project\MarketAppFinalProject\AdminPanel\bin\Debug\net8.0"; //Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "C:\\Users\\Yusub_sq38\\Desktop\\Final Project\\Final Project\\MarketAppFinalProject\\AdminPanel\\bin\\Debug\\net8.0\\categories.json");
                string filePath = Path.Combine(fileDirectory, "cart.json");
                string fileCopyPath = Path.Combine(fileDirectory, "cartHistory.json");
                if (File.Exists(filePath))
                {
                    var json = File.ReadAllText(filePath);
                    var listOfCartItem = JsonSerializer.Deserialize<List<CartItem>>(json);
                    if (listOfCartItem is not null) CartItems = listOfCartItem;
                    File.WriteAllText(fileCopyPath,json);
                }
                CartItems ??= new List<CartItem>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while initializing the cart: {ex.Message}");
                CartItems = new List<CartItem>();
            }
        }

        public static void AddToCart(Product product, int quantity, User user)
        {
            try
            {
                string fileDirectory = @"C:\Users\Yusub_sq38\Desktop\Final Project\Final Project\MarketAppFinalProject\AdminPanel\bin\Debug\net8.0"; //Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "C:\\Users\\Yusub_sq38\\Desktop\\Final Project\\Final Project\\MarketAppFinalProject\\AdminPanel\\bin\\Debug\\net8.0\\categories.json");
                string filePath = Path.Combine(fileDirectory, "cart.json");
                string fileCopyPath = Path.Combine(fileDirectory, "cartHistory.json");
                var cartItem = CartItems.FirstOrDefault(ci => ci.Product.ProductId == product.ProductId);
                if (cartItem != null)
                {
                    cartItem.Quantity += quantity;
                }
                else
                {
                    CartItems.Add(new CartItem { Product = product, Quantity = quantity, CartItemId = user.UserId });
                }

                var json = JsonSerializer.Serialize(CartItems);
                File.WriteAllText(filePath, json);
                File.WriteAllText(fileCopyPath,json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding to cart: {ex.Message}");
            }
        }

        public static void RemoveFromCart(Product product)
        {
            try
            {
                string fileDirectory = @"C:\Users\Yusub_sq38\Desktop\Final Project\Final Project\MarketAppFinalProject\AdminPanel\bin\Debug\net8.0"; //Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "C:\\Users\\Yusub_sq38\\Desktop\\Final Project\\Final Project\\MarketAppFinalProject\\AdminPanel\\bin\\Debug\\net8.0\\categories.json");
                string filePath = Path.Combine(fileDirectory, "cart.json");
                var cartItem = CartItems.FirstOrDefault(ci => ci.Product.ProductId == product.ProductId);
                if (cartItem != null)
                {
                    CartItems.Remove(cartItem);

                    var json = JsonSerializer.Serialize(CartItems);
                    File.WriteAllText(filePath, json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while removing from cart: {ex.Message}");
            }
        }

        public static void OrderHistory()
        {
            try
            {
                string fileDirectory = @"C:\Users\Yusub_sq38\Desktop\Final Project\Final Project\MarketAppFinalProject\AdminPanel\bin\Debug\net8.0"; //Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "C:\\Users\\Yusub_sq38\\Desktop\\Final Project\\Final Project\\MarketAppFinalProject\\AdminPanel\\bin\\Debug\\net8.0\\categories.json");
                string filePath = Path.Combine(fileDirectory, "cart.json");
                string fileCopyPath = Path.Combine(fileDirectory, "cartHistory.json");
                var currentUser = UserManager.User;
                if (currentUser is null) throw new Exception("User not found");

                if (File.Exists(fileCopyPath))
                {
                    var json = File.ReadAllText(filePath);
                    var listOfHistory = JsonSerializer.Deserialize<List<CartItem>>(json);
                    if (listOfHistory is not null) CartHistory = listOfHistory;
                    File.WriteAllText(fileCopyPath, json);
                }

                Console.WriteLine("Your Shopping History: ");
                foreach (var cartItem in CartHistory)
                {
                    if (cartItem.CartItemId == currentUser.UserId)
                    {
                        Console.WriteLine("***********************************************");
                        Console.WriteLine($"Product: {cartItem.Product.ProductName}");
                        Console.WriteLine($"Description: {cartItem.Product.Description}");
                        Console.WriteLine($"Quantity: {cartItem.Quantity}");
                        Console.WriteLine("***********************************************");
                    }
                }
                Console.WriteLine("Press any button to return...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching order history: {ex.Message}");
            }
        }

        public static double Checkout(double payment)
        {
            try
            {
                string fileDirectory = @"C:\Users\Yusub_sq38\Desktop\Final Project\Final Project\MarketAppFinalProject\AdminPanel\bin\Debug\net8.0"; //Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "C:\\Users\\Yusub_sq38\\Desktop\\Final Project\\Final Project\\MarketAppFinalProject\\AdminPanel\\bin\\Debug\\net8.0\\categories.json");
                string filePath = Path.Combine(fileDirectory, "cart.json");
                string fileCopyPath = Path.Combine(fileDirectory, "cartHistory.json");
                double total = CartItems.Sum(ci => ci.Product.Price * ci.Quantity);
                if (payment >= total)
                {
                    //CartItems.Clear();
                    var json = JsonSerializer.Serialize(CartItems);
                    File.WriteAllText(filePath, json);
                    File.WriteAllText(fileCopyPath, json);
                    return payment - total;
                }
                throw new Exception("Insufficient funds");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during checkout: {ex.Message}");
                return -1; 
            }
        }
    }
}
