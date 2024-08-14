using System;
using System.Collections.Generic;

namespace EcoNet.Models
{
    public partial class EtiquetaAnuncio
    {
        public int IdEtiquetaAnuncio { get; set; }
        public int? Fketiqueta { get; set; }
        public int? Fkanuncio { get; set; }

        public virtual Anuncio? FkanuncioNavigation { get; set; }
        public virtual Etiqueta? FketiquetaNavigation { get; set; }

        public List<Anuncio> ArticulosFiltrados { get; set; } = new List<Anuncio>();
    }
}
