﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebPizzaSite.Constants;

namespace WebPizzaSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    public class TablesController : Controller
    {
        public IActionResult GeneralTables()
        {
            return View();
        }
        public IActionResult DataTables()
        {
            return View();
        }
    }
}
