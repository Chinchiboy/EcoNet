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

        public IActionResult Index(string? descripcionEtiqueta)
        {
            List<Anuncio> anuncios;

            if (string.IsNullOrEmpty(descripcionEtiqueta))
            {
                // Obtener todas las ofertas si no se proporciona una etiqueta
                anuncios = _dalAnuncio.ObtenerTodasLasOfertas();
            }
            else
            {
                // Filtrar ofertas por la etiqueta proporcionada
                anuncios = _dalAnuncio.FiltrarAnunciosPorEtiqueta(descripcionEtiqueta);
            }

            if (anuncios == null || anuncios.Count == 0)
            {
                return View("NoResults"); // Vista que puedes crear para cuando no haya resultados
            }

            return View(anuncios);
        }
    }
}