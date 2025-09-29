using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SimpleMovieSalaryWebApp.Models;
using System.Text;
using System.Text.Json;

namespace SimpleMovieSalaryWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5096/"); // Your API base URL here
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var loginData = new
            {
                Username = model.Username,
                Password = model.Password
            };

            var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsync("api/auth/login", content);
            }
            catch (Exception ex)
            {
                model.ErrorMessage = "Login request failed: " + ex.Message;
                return View(model);
            }

            if (!response.IsSuccessStatusCode)
            {
                model.ErrorMessage = "Invalid credentials or server error.";
                return View(model);
            }

            var responseString = await response.Content.ReadAsStringAsync();

            TokenResponse tokenResponse;
            try
            {
                tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseString);
            }
            catch (Exception ex)
            {
                model.ErrorMessage = "Failed to read token: " + ex.Message;
                return View(model);
            }

            if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.Token))
            {
                model.ErrorMessage = "Login failed: Empty token response.";
                return View(model);
            }

            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(tokenResponse.Token);

            var roleClaim = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

            var role = roleClaim?.Value ?? "User";

            HttpContext.Session.SetString("JWToken", tokenResponse.Token);
            HttpContext.Session.SetString("UserRole", role);

            return RedirectToAction("Index", "CastMembers");
        }

    }
}

