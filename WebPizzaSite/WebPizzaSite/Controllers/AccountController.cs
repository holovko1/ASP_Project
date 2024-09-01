using Microsoft.AspNetCore.Mvc;

namespace WebPizzaSite.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
