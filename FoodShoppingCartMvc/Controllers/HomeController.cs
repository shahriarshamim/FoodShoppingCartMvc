using FoodShoppingCartMvc.Models;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FoodShoppingCartMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _logger = logger;
            _homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index(string sterm = "", int categoryId = 0)
        {
            IEnumerable<Item> items = await _homeRepository.GetItems(sterm, categoryId);
            IEnumerable<Category> categories = await _homeRepository.Categories();
            ItemDisplayModel itemModel = new ItemDisplayModel
            {
                Items = items,
                Categories = categories,
                STerm = sterm,
                CategoryId = categoryId
            };
            return View(itemModel);
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
