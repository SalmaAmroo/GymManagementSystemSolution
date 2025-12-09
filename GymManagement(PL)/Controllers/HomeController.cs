using System.Diagnostics;
using GymManagement_PL_.Models;
using GymManagementBLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement_PL_.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnalyticService _analyticService;

        public HomeController(IAnalyticService analyticService)
        {
            _analyticService = analyticService;
        }
        public ActionResult Index()
        {
            var analyticsData = _analyticService.GetAnalyticsData();
            return View(analyticsData);
        }


    }
}
