using Microsoft.AspNetCore.Mvc;
using SimpleMovieSalaryWebApp.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SimpleMovieSalaryWebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public IActionResult Login()
        {
            var token = Request.Cookies["token"];
            if (!string.IsNullOrEmpty(token))
            {
                // Already logged in, redirect to Home
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Paste the full API URL here (like http://localhost:5007/api/auth/login)
            var response = await _httpClient.PostAsync("http://localhost:5096/api/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseString);
                var token = doc.RootElement.GetProperty("token").GetString();
                var refreshToken = doc.RootElement.GetProperty("refreshToken").GetString();

                // Save in cookies
                Response.Cookies.Append("token", token);
                Response.Cookies.Append("refreshToken", refreshToken);

                // Redirect to Home page
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid login.";
            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("token");
            Response.Cookies.Delete("refreshToken");

            return RedirectToAction("Login");
        }

    }
}
