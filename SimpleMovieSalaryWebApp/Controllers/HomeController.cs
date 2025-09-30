//using Microsoft.AspNetCore.Mvc;
//using SimpleMovieSalaryWebApp.Models;
//using SimpleMovieSalaryWebApp.Filters;
//using System.Net.Http;
//using System.Text.Json;
//using System.Threading.Tasks;
//using System.Collections.Generic;

//namespace SimpleMovieSalaryWebApp.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly HttpClient _httpClient;

//        public HomeController(IHttpClientFactory httpClientFactory)
//        {
//            _httpClient = httpClientFactory.CreateClient();
//        }

//        [AuthGuard]
//        public IActionResult Index()
//        {
//            return View(new List<CastMemberViewModel>());
//        }

//        [AuthGuard]
//        [HttpPost]
//        public async Task<IActionResult> FetchAll()
//        {
//            var token = Request.Cookies["token"];
//            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

//            // Paste your GET ALL API URL here
//            var response = await _httpClient.GetAsync("http://localhost:5096/api/castmembers");

//            if (!response.IsSuccessStatusCode)
//                return View("Index", new List<CastMemberViewModel>());

//            var responseString = await response.Content.ReadAsStringAsync();
//            var data = JsonSerializer.Deserialize<List<CastMemberViewModel>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

//            return View("Index", data);
//        }

//        [AuthGuard]
//        [HttpPost]
//        public async Task<IActionResult> FetchById(int id)
//        {
//            var token = Request.Cookies["token"];
//            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

//            // Paste your GET BY ID API URL here
//            var response = await _httpClient.GetAsync($"http://localhost:5096/api/castmembers/{id}");

//            if (!response.IsSuccessStatusCode)
//                return View("Index", new List<CastMemberViewModel>());

//            var responseString = await response.Content.ReadAsStringAsync();
//            var singleItem = JsonSerializer.Deserialize<CastMemberViewModel>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

//            return View("Index", new List<CastMemberViewModel> { singleItem });
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using SimpleMovieSalaryWebApp.Filters;
using SimpleMovieSalaryWebApp.Helpers; // ✅ Add this
using SimpleMovieSalaryWebApp.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [AuthGuard]
        public IActionResult Index()
        {
            SetRoleInViewBag();
            return View(new List<CastMemberViewModel>());
        }

        [AuthGuard]
        [HttpPost]
        public async Task<IActionResult> FetchAll()
        {
            var token = Request.Cookies["token"];
            SetRoleInViewBag();

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("http://localhost:5096/api/castmembers");

            if (!response.IsSuccessStatusCode)
                return View("Index", new List<CastMemberViewModel>());

            var responseString = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<List<CastMemberViewModel>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View("Index", data);
        }

        [AuthGuard]
        [HttpPost]
        public async Task<IActionResult> FetchById(int id)
        {
            var token = Request.Cookies["token"];
            SetRoleInViewBag();

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"http://localhost:5096/api/castmembers/{id}");

            if (!response.IsSuccessStatusCode)
                return View("Index", new List<CastMemberViewModel>());

            var responseString = await response.Content.ReadAsStringAsync();
            var singleItem = JsonSerializer.Deserialize<CastMemberViewModel>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View("Index", new List<CastMemberViewModel> { singleItem });
        }

        private void SetRoleInViewBag()
        {
            var token = Request.Cookies["token"];
            var role = JwtHelper.GetRoleFromToken(token);
            ViewBag.Role = role;
        }
    }
}
