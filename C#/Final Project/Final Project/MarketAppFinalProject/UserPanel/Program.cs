using System.ComponentModel.Design;
using UserPanel.Services;
internal class Program
{
    public static void RegisterPage()
    {
    Register:
        Console.Clear();
        Console.WriteLine("*********Sign Up*********");
        Console.Write("Name: ");
        var name = Console.ReadLine();
        Console.Write("Surname: ");
        var surname = Console.ReadLine();
        Console.Write("Date of birth (dd.MM.yyyy): ");
        var date = Console.ReadLine();
        Console.Write("Email: ");
        var email = Console.ReadLine();
        Console.Write("Password: ");
        var password = Console.ReadLine();

        try
        {
            UserManager.Register(name!, surname!, email!.ToLower().Trim(), password!, date!);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Thread.Sleep(2000);
            goto Register;
        }
    }

    public static void LoginPage()
    {
    Login:
        Console.Clear();
        Console.WriteLine("*********Sign In*********");
        Console.Write("Email: ");
        var email = Console.ReadLine();
        Console.Write("Password: ");
        var password = Console.ReadLine();
        try
        {
            UserManager.Login(email!, password!);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Thread.Sleep(2000);
            goto Login;
        }
    }

    public static void MainPage()
    {
        try
        {
            Console.Clear();
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static void CategoryPage()
    {
        try
        {
            Console.Clear();
            Console.WriteLine("*********Categories*********");
            var categories = CategoryManager.GetCategories();
            foreach (var category in categories)
            {
                Console.WriteLine(category.CategoryName);
            }
            Console.Write("Choose a category by name: ");
            var categoryName = Console.ReadLine();
            var selectedCategory = categories.FirstOrDefault(p => p.CategoryName.ToLower().Trim() == categoryName!.ToLower().Trim());
            ProductPage(selectedCategory!.CategoryId);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static void ProductPage(string categoryId)
    {
        try
        {
            Stock:
            Console.Clear();
            Console.WriteLine("*********Products*********");
            var products = ProductManager.GetProductsByCategory(categoryId);
            foreach (var product in products)
            {
                if (product.Stock != 0)
                {
                    Console.WriteLine("***********************************************");
                    Console.WriteLine($"Product: {product.ProductName}");
                    Console.WriteLine($"Price: {product.Price}");
                    Console.WriteLine($"Stock: {product.Stock}");
                    Console.WriteLine($"Description: {product.Description}");
                    Console.WriteLine("***********************************************");
                }

            }
            Console.Write("Choose a product by name to add to cart: ");
            var choice = Console.ReadLine()!;
            var selectedProduct = products.FirstOrDefault(p => p.ProductName.ToLower().Trim() == choice.ToLower().Trim());
            Console.Write("Enter quantity: ");
            var quantity = int.Parse(Console.ReadLine()!);
            if (quantity > selectedProduct!.Stock)
            {
                Console.WriteLine($"Error!We have only {selectedProduct.Stock} stock from this product.Please try again...");
                Thread.Sleep(100);
                goto Stock;
            }
            if (quantity < selectedProduct!.Stock && quantity>=0)
            {
                var newStock=selectedProduct!.Stock-quantity;
                ProductManager.UpdateStock(selectedProduct.ProductId, newStock);
            }
            if(selectedProduct!.Stock == 0)
            {
                Console.WriteLine("This Item is out of stock.Please choose another product...");
                goto Stock;
            }
            var currentUser = UserManager.User;
            CartManager.AddToCart(selectedProduct!, quantity, currentUser!);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static void CartPage()
    {
        try
        {
            Console.Clear();
            Console.WriteLine("*********Cart*********");
            var cartItems = CartManager.CartItems;
            foreach (var cartItem in cartItems)
            {
                Console.WriteLine($"Product: {cartItem.Product.ProductName}");
                Console.WriteLine($"Quantity: {cartItem.Quantity}");
            }
            double total = cartItems.Sum(ci => ci.Product.Price * ci.Quantity);
            Console.WriteLine($"Total: {total}");
            Console.WriteLine("1. Remove an item");
            Console.WriteLine("2. Checkout");
            var choice = Console.ReadLine()!;
            switch (choice)
            {
                case "1":
                    Console.Write("Enter the name of the item to remove: ");
                    var removeProduct = Console.ReadLine()!;
                    var products = ProductManager.Products;
                    var selectedProduct = products.FirstOrDefault(p => p.ProductName.ToLower().Trim() == removeProduct.ToLower().Trim());
                    CartManager.RemoveFromCart(selectedProduct!);
                    break;
                case "2":
                    Console.Write("Enter payment amount: ");
                    var payment = double.Parse(Console.ReadLine()!);
                    if (payment < total && payment < 0) throw new Exception("Payment Error try again!");

                        try
                        {
                            double change = payment - total;
                            Console.WriteLine($"Change: {change}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    foreach (var cartItem in cartItems)
                    {
                        ProductManager.UpdateStock(cartItem.Product.ProductId,cartItem.Product.Stock-cartItem.Quantity);
                    }
                    Console.WriteLine("Invalid choice. Please try again.");
                    foreach (var cartItem in cartItems)
                    {
                        CartManager.RemoveFromCart(cartItem.Product);
                    }
                    break;
                default:
                    Console.WriteLine( "Invalid Choice.Please Try Again...");
                    break;

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static void ProfilePage()
    {
        try
        {
            Console.Clear();
            var user = UserManager.User;
            Console.WriteLine("*********Profile*********");
            Console.WriteLine($"Name: {user!.Name}");
            Console.WriteLine($"Surname: {user.Surname}");
            Console.WriteLine($"Email: {user.Email}");
            Console.WriteLine($"Date of Birth: {user.DateOfBirth}");
            Console.WriteLine("1. Edit profile");
            Console.WriteLine("2. Change password");
            Console.WriteLine("3. View order history");
            Console.WriteLine("4. Logout");

            var choice = Console.ReadLine()!;
            switch (choice)
            {
                case "1":
                    Console.Write("Enter new name: ");
                    var name = Console.ReadLine();
                    Console.Write("Enter new surname: ");
                    var surname = Console.ReadLine();
                    Console.Write("Enter new date of birth (dd.MM.yyyy): ");
                    var date = Console.ReadLine();
                    UserManager.UpdateProfile(name!, surname!, DateOnly.ParseExact(date!, "dd.MM.yyyy"));
                    break;
                case "2":
                    Console.Write("Enter new password: ");
                    var newPassword = Console.ReadLine();
                    UserManager.ChangePassword(newPassword!);
                    break;
                case "3":
                    CartManager.OrderHistory();
                    break;
                case "4":
                    UserManager.Logout();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static void Main(string[] args)
    {
        while (true)
        {
            try
            {
                if (UserManager.User == null)
                {
                    MainPage();
                    var choice = Console.ReadLine();
                    if (choice == "1")
                    {
                        RegisterPage();
                    }
                    else if (choice == "2")
                    {
                        LoginPage();
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("1. View Categories");
                    Console.WriteLine("2. View Cart");
                    Console.WriteLine("3. View Profile");
                    Console.WriteLine("4. Logout");
                }
                var switch_on = Console.ReadLine()!;
                switch (switch_on)
                {
                    case "1":
                        CategoryPage();
                        break;
                    case "2":
                        CartPage();
                        break;
                    case "3":
                        ProfilePage();
                        break;
                    case "4":
                        UserManager.Logout();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
