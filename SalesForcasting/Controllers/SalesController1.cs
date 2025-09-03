using Microsoft.AspNetCore.Mvc;
using SalesForcasting.Models; // make sure this matches your namespace
using SalesForcasting.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesForcasting.Controllers
{
    public class SalesController : Controller
    {
        private readonly SalesForecastService _forecastService;

        public SalesController(SalesForecastService forecastService)
        {
            _forecastService = forecastService;
        }

        // Redirects to Forecast action directly
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Forecast");
        }

        [HttpGet]
        public async Task<IActionResult> Forecast()
        {
            var forecasts = await _forecastService.ForecastSalesAsync();
            return View(forecasts); // Looks for Views/Sales/Forecast.cshtml
        }
    }
}
