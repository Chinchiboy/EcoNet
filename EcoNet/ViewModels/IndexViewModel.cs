using EcoNet.Models;

namespace EcoNet.Models
{
    public class IndexViewModel
    {

        //Aquí se añaden los modelos que vayamos a usar al Index
        public EtiquetaAnuncio EtiquetaAnuncio { get; set; } = new EtiquetaAnuncio();

        public EtiquetaFiltros EtiquetaFiltros { get; set; } = new EtiquetaFiltros();

    }
}
