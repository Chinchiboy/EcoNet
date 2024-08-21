using EcoNet.DAL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Diagnostics;
using EcoNet.Models;

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
            DalUsuario dalUserr = new();
            Usuario? usuario = dalUserr.AutenticationUserDal(Email, Password);

            if (usuario != null)
            {
                List<Claim> claims = new()
                {
                    new(ClaimTypes.Name, usuario.NombreUsuario),
                    new(ClaimTypes.Email, usuario.Email),
                    new(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString())
                };

                ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new(identity);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal).Wait();

                TempData["NombreUsuario"] = usuario.NombreUsuario;
                Debug.WriteLine("función login " + usuario.NombreUsuario);

                return RedirectToAction("Index", "Home");
            }

            TempData["LoginError"] = "Usuario o contraseña incorrectos.";
            return RedirectToAction("Index", "Home");
        }
    }
}
