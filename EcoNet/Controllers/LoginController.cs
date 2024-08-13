using EcoNet.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcoNet.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            var model = new LoginViewModel();
            return PartialView("_LoginPartial", model);
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
