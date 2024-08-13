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

        [HttpGet("filtrar")]
        public IActionResult FiltrarPorEtiqueta([FromQuery] string descripcionEtiqueta)
        {
            List<Anuncio> anunciosFiltrados = _dalAnuncio.SelectByTag(descripcionEtiqueta);

            if (anunciosFiltrados == null || anunciosFiltrados.Count == 0)
            {
                return NotFound("No se encontraron anuncios con la etiqueta especificada.");
            }

            return Ok(anunciosFiltrados);
        }
    }
}
