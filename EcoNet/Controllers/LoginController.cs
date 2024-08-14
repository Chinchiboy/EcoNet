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

            // Verifica si el nombre de usuario no es nulo o vacío
            if (!string.IsNullOrEmpty(userName))
            {
                ViewBag.Usuario = userName;
                // Autenticación exitosa, redirige al Index
                return RedirectToAction("Index", "Home");
            }

            // Si la autenticación falla, muestra un mensaje de error
            ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
            return View();
        }
    }
}
