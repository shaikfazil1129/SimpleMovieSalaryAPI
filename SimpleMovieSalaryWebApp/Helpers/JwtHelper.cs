using System.IdentityModel.Tokens.Jwt;

namespace SimpleMovieSalaryWebApp.Helpers
{
    public class JwtHelper
    {
        public static string GetRoleFromToken(string? token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Adjust the claim type depending on your backend token
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

            return roleClaim?.Value;
        }
    }
}
