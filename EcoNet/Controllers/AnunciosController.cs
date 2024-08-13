using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using EcoNet.Models;

namespace EcoNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnunciosController : ControllerBase
    {
        private readonly DalAnuncio _dalAnuncio;

        public AnunciosController(DalAnuncio dalAnuncio)
        {
            _dalAnuncio = dalAnuncio;
        }

        [HttpGet]
        public IActionResult ObtenerAnuncios([FromQuery] string? descripcionEtiqueta)
        {
            List<Anuncio> anuncios;

            if (string.IsNullOrEmpty(descripcionEtiqueta))
            {
                anuncios = _dalAnuncio.Select();
            }
            else
            {
                anuncios = _dalAnuncio.SelectByTag(descripcionEtiqueta);
            }

            if (anuncios == null || anuncios.Count == 0)
            {
                return NotFound("No se encontraron anuncios.");
            }

            return Ok(anuncios);
        }
    }
}
