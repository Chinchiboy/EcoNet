using EcoNet.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcoNet.Controllers
{
    public class CrearAnuncioController : Controller
    {
        private readonly DalAnuncio Dalanuncio;

        public CrearAnuncioController()
        {
            Dalanuncio = new DalAnuncio();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CrearAnuncio()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CrearAnuncio(string titulo, string descripcion, decimal precio)
        {
            string usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            Anuncio nuevoAnuncio = new()
            {
                Titulo = titulo,
                Descripcion = descripcion,
                Precio = precio,
                Fkusuario = int.Parse(usuarioId), 
                EstaVendido = false
            };
            Dalanuncio.Add(nuevoAnuncio);

            return RedirectToAction("Index", "Home");
        }
    }
}
