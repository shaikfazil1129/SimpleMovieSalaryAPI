namespace SimpleMovieSalaryAPI.Models
{
    public class UserModel
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = "User"; // or "Admin"
    }
}
