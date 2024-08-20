using Microsoft.AspNetCore.Mvc;
using EcoNet.Models;

namespace EcoNet.Controllers
{
    public class MostrarAnuncioController : Controller
    {
        private readonly DalAnuncio dalAnuncio;

        public MostrarAnuncioController(DalAnuncio dalAnuncio)
        {
            this.dalAnuncio = dalAnuncio;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MostrarAnuncio(int id)
        {
            Anuncio a = dalAnuncio.SelectById(id);

            if (a == null)      return NotFound();

            return View(a);
        }
    }
}
