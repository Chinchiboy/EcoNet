﻿using Microsoft.AspNetCore.Mvc;

namespace EcoNet.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
