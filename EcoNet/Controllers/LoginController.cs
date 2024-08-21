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
            DalUsuario dalUserr = new DalUsuario();
            string? userName = dalUserr.AutenticationUserDal(Email, Password);
            int userId = ;

            if (!string.IsNullOrEmpty(userName))
            {
                List<Claim> claims = new()
                {
                    new (ClaimTypes.Name, userName),
                    new (ClaimTypes.Email, Email),
                    new (ClaimTypes.NameIdentifier, userId)
                };

                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal).Wait();

                TempData["NombreUsuario"] = userName;
                Debug.WriteLine("funcion login " + userName);

                return RedirectToAction("Index", "Home");
            }
            TempData["LoginError"] = "Usuario o contraseña incorrectos.";
            return RedirectToAction("Index", "Home");
        }
    }
}
