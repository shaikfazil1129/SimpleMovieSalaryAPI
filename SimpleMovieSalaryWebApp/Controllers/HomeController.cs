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

namespace SimpleMovieSalaryWebApp.Controllers
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

        [AuthGuard]
        [HttpGet]
        public IActionResult Create()
        {
            SetRoleInViewBag();
            return View();
        }

        [AuthGuard]
        [HttpPost]
        public async Task<IActionResult> Create(CastMemberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                SetRoleInViewBag();
                return View(model);
            }

            var token = Request.Cookies["token"];
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var jsonContent = new StringContent(JsonSerializer.Serialize(model), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5096/api/castmembers", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Error creating cast member.");
                SetRoleInViewBag();
                return View(model);
            }
        }

        [AuthGuard]
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var token = Request.Cookies["token"];
            SetRoleInViewBag();

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"http://localhost:5096/api/castmembers/{id}");

            if (!response.IsSuccessStatusCode)
            {
                // Could not find the item or error, redirect back to Index or show error
                return RedirectToAction("Index");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var model = JsonSerializer.Deserialize<CastMemberViewModel>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(model);
        }

        [AuthGuard]
        [HttpPost]
        public async Task<IActionResult> Update(CastMemberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                SetRoleInViewBag();
                return View(model);
            }

            var token = Request.Cookies["token"];
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var jsonContent = new StringContent(JsonSerializer.Serialize(model), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"http://localhost:5096/api/castmembers/{model.Id}", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Error updating cast member.");
                SetRoleInViewBag();
                return View(model);
            }
        }

        [AuthGuard]
        [HttpPost("Home/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var token = Request.Cookies["token"];
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"http://localhost:5096/api/castmembers/{id}");

            if (!response.IsSuccessStatusCode)
            {
                // Log or handle individual deletion failure if needed
                return BadRequest();
            }

            return Ok();
        }


        private void SetRoleInViewBag()
        {
            var token = Request.Cookies["token"];
            var role = JwtHelper.GetRoleFromToken(token);
            ViewBag.Role = role;
        }
    }
}
