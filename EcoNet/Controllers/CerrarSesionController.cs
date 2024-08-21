using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace EcoNet.Controllers
{
    public class CerrarSesionController : Controller
    {
        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
