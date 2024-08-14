using EcoNet.DAL;
using EcoNet.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcoNet.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string Email, string Password)
        {
            DalUsuario dalUserr = new DalUsuario();

            string? userName = dalUserr.AutenticationUserDal(Email, Password);

            if (!string.IsNullOrEmpty(userName))
            {
                ViewBag.Usuario = userName;
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
            return View();
        }
    }
}
