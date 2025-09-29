using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using SimpleMovieSalaryWebApp.Models; // Replace with your actual namespace

namespace SimpleMovieSalaryWebApp.Controllers
{
   
    public class CastMembersController : Controller
    {
        private readonly HttpClient _httpClient;

        public CastMembersController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5096/"); // Your API base URL
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var role = HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("api/castmembers");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Could not fetch data.";
                return View(new List<CastMember>());
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            var castList = JsonConvert.DeserializeObject<List<CastMember>>(jsonData);

            ViewBag.Role = role;
            return View(castList);
        }
    }

}
