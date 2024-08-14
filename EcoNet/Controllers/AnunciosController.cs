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

        [HttpGet("filtrar")]
        public IActionResult ObtenerAnuncios([FromQuery] string? descripcionEtiqueta, [FromQuery] string? titulo)
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
                return NotFound("No se encontraron anuncios.");
            }

            var articulosFiltrados = anuncios.Select(a => a.Titulo).ToList(); // Ajusta esto según el campo que desees mostrar

            return Ok(articulosFiltrados);
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
