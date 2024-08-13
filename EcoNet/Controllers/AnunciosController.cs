using Microsoft.AspNetCore.Mvc;
using EcoNet.Models;
using System.Collections.Generic;

namespace EcoNet.Controllers
{
    public class AnunciosController : Controller
    {
        private readonly DalAnuncio _dalAnuncio;

        public AnunciosController(DalAnuncio dalAnuncio)
        {
            _dalAnuncio = dalAnuncio;
        }

        public IActionResult Index(string? descripcionEtiqueta, [FromQuery] string? titulo)
        {
            List<Anuncio> anuncios;

            if (!string.IsNullOrEmpty(titulo))
            {
                anuncios = _dalAnuncio.SelectByTitle(titulo);
            }
            else if (!string.IsNullOrEmpty(descripcionEtiqueta))
            {
                anuncios = _dalAnuncio.SelectByTag(descripcionEtiqueta);
            }
            else
            {
                anuncios = _dalAnuncio.Select();
            }

            if (anuncios == null || anuncios.Count == 0)
            {
                return View("NoResults");
            }

            return View(anuncios);
        }
    }
}