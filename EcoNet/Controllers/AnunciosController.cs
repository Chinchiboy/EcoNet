using Microsoft.AspNetCore.Mvc;
using EcoNet.Models;
using System.Collections.Generic;
using System.Linq;

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

        [HttpGet("todos")]
        public IActionResult ObtenerTodosLosAnuncios(EtiquetaAnuncio obj)
        {
            List<Anuncio> anuncios = _dalAnuncio.Select();

            if (anuncios == null || anuncios.Count == 0)
            {
                return NotFound("No se encontraron anuncios.");
            }

            return Ok(anuncios);
        }

        // Método para filtrar anuncios por título o etiqueta
        [HttpGet("filtrar")]
        public IActionResult FiltrarAnuncios([FromQuery] string? descripcionEtiqueta, [FromQuery] string? titulo)
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
                return BadRequest("Debe proporcionar al menos un título o una etiqueta para filtrar.");
            }

            if (anuncios == null || anuncios.Count == 0)
            {
                return NotFound("No se encontraron anuncios con los criterios especificados.");
            }

            return Ok(anuncios);
        }

        [HttpGet("{anuncioId}")]
        public IActionResult ObtenerAnuncioPorId(int id)
        {
            Anuncio anuncio = _dalAnuncio.SelectById(id);

            if (anuncio == null)
            {
                return NotFound($"No se encontró un anuncio con la ID {id}.");
            }

            return Ok(anuncio);
        }
    }
}
