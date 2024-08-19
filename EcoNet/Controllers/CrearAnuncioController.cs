using EcoNet.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcoNet.Controllers
{
    public class CrearAnuncioController : Controller
    {
        private readonly DalAnuncio Dalanuncio;
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CrearAnuncio()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CrearAnuncio(int idAnuncio,string titulo,byte[]? imagen,string descripcion,decimal precio,int? fkborradoPor, int fkusuario,bool estaVendido)
        {
            Anuncio nuevoAnuncio = new Anuncio
            {
                IdAnuncio = idAnuncio,
                Titulo = titulo,
                Imagen = imagen, 
                Descripcion = descripcion,
                Precio = precio,
                FkborradoPor = fkborradoPor, 
                Fkusuario = fkusuario,
                EstaVendido = estaVendido
            };
            Dalanuncio.Add(nuevoAnuncio);
            return RedirectToAction("Index", "Home");
        }
    }
}
