using EcoNet.DAL;
using EcoNet.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Diagnostics;

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
            Console.WriteLine();
            if (!string.IsNullOrEmpty(userName))
            {
                // Crear los claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Email, Email)
                };

                // Crear la identidad del usuario
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Crear el principal del usuario
                var principal = new ClaimsPrincipal(identity);

                // Autenticar al usuario
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal).Wait();

                // Almacenar el nombre de usuario en TempData
                TempData["NombreUsuario"] = userName;
                Debug.WriteLine(" funcion login " + userName);

                return RedirectToAction("Index", "Home");
            }

            // Si la autenticación falla
            ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
            return RedirectToAction("Index", "Home");
        }
    }
}
