using Microsoft.AspNetCore.Mvc;

namespace FoodShoppingCartMvc.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IReportRepository _reportRepository;
        public ReportsController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        // GET: ReportsController
        public async Task<ActionResult> TopFiveSellingItems(DateTime? sDate = null, DateTime? eDate = null)
        {
            try
            {
                // by default, get last 7 days record
                DateTime startDate = sDate ?? DateTime.UtcNow.AddDays(-7);
                DateTime endDate = eDate ?? DateTime.UtcNow;
                var topFiveSellingItems = await _reportRepository.GetTopNSellingItemsByDate(startDate, endDate);
                var vm = new TopNSoldItemsVm(startDate, endDate, topFiveSellingItems);
                return View(vm);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Something went wrong";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
