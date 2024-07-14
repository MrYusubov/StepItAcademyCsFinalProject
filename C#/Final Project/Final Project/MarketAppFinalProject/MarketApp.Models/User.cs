namespace MarketApp.Models
{
    public class User
    {
        public string UserId { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
