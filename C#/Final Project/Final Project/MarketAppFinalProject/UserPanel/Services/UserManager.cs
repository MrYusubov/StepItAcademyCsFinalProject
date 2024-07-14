using MarketApp.Models;
using System.Text.Json;

namespace UserPanel.Services
{
    public static class UserManager
    {
        public static List<User> Users { get; set; }
        public static User? User { get; set; }

        static UserManager()
        {
            try
            {
                if (File.Exists("users.json"))
                {
                    var json = File.ReadAllText("users.json");
                    var listOfUser = JsonSerializer.Deserialize<List<User>>(json);
                    if (listOfUser is not null) Users = listOfUser;
                }
                Users ??= new List<User>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while initializing the UserManager: {ex.Message}");
                Users = new List<User>();
            }
        }

        public static void Register(string name, string surname, string email, string pass, string date)
        {
            try
            {
                var user = Users.FirstOrDefault(u => u.Email == email);
                if (user is null)
                {
                    user = new User
                    {
                        Email = email,
                        Surname = surname,
                        Name = name,
                        Password = pass,
                        DateOfBirth = DateOnly.ParseExact(date!, "dd.MM.yyyy")
                    };
                    Users.Add(user);

                    var json = JsonSerializer.Serialize(Users);
                    File.WriteAllText("users.json", json);

                    return;
                }
                throw new Exception("User already exists");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while registering the user: {ex.Message}");
            }
        }

        public static void Login(string email, string password)
        {
            try
            {
                User = Users.FirstOrDefault(u => u.Email == email.ToLower().Trim() && u.Password == password);
                if (User is null) throw new Exception("Invalid email or password");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while logging in: {ex.Message}");
            }
        }

        public static void Logout()
        {
            try
            {
                User = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while logging out: {ex.Message}");
            }
        }

        public static void UpdateProfile(string name, string surname, DateOnly dateOfBirth)
        {
            try
            {
                if (User is not null)
                {
                    User.Name = name;
                    User.Surname = surname;
                    User.DateOfBirth = dateOfBirth;

                    var json = JsonSerializer.Serialize(Users);
                    File.WriteAllText("users.json", json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the profile: {ex.Message}");
            }
        }

        public static void ChangePassword(string newPassword)
        {
            try
            {
                if (User is not null)
                {
                    User.Password = newPassword;

                    var json = JsonSerializer.Serialize(Users);
                    File.WriteAllText("users.json", json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while changing the password: {ex.Message}");
            }
        }
    }
}
