using EcoNet.Models;
using EcoNet.ViewModels;

namespace EcoNet.Models
{
    public class IndexViewModel
    {
        public EtiquetaAnuncio EtiquetaAnuncio { get; set; } = new EtiquetaAnuncio();

        public EtiquetaFiltros EtiquetaFiltros { get; set; } = new EtiquetaFiltros();
        public Anuncio ProductoActual { get; set; }
        public EtiquetaAnuncioViewModel EtiquetaAnuncioVM { get; set; }

    }
}
