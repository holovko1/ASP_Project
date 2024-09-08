using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebPizzaSite.Constants;

namespace WebPizzaSite.Areas.Admin.Controllers
{
    public class IconsController : Controller
    {
        [Area("Admin")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult BootstrapIcons()
        {
            return View();
        }
        public IActionResult RemixIcons()
        {
            return View();
        }
        public IActionResult Boxicons()
        {
            return View();
        }
    }
}
