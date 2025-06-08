using System.Diagnostics;
using System.Text.Json;
using Cineverse.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Cineverse.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            string apiKey = "bda97661";
            string searchTerm = "Barbie"; // ili bilo koji drugi pojam iz tvoje liste
            string url = $"http://www.omdbapi.com/?apikey={apiKey}&s={searchTerm}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        var jsonDoc = JsonDocument.Parse(responseData);
                        ViewBag.MovieData = jsonDoc.RootElement.GetProperty("Search").EnumerateArray().Take(8).ToList();
                        return View();
                    }
                    else
                    {
                        ViewBag.ErrorMessage = $"Greška: {response.StatusCode}";
                        return View("Error");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"Došlo je do greške: {ex.Message}";
                    return View("Error");
                }
            }
        }





        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
