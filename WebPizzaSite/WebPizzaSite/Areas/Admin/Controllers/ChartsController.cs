using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebPizzaSite.Constants;

namespace WebPizzaSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    public class ChartsController : Controller
    {
        public IActionResult Chart()
        {
            return View();
        }
        public IActionResult ApexCharts()
        {
            return View();
        }
        public IActionResult ECharts()
        {
            return View();
        }
    }
}
