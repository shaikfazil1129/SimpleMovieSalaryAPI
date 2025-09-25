namespace SimpleMovieSalaryAPI.Models
{
    public class UserModel
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = "User"; // or "Admin"

        // ✅ Add these for refresh token
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
