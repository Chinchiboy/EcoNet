using System;
using System.Collections.Generic;

namespace EcoNet.Models
{
    public partial class Etiqueta
    {
        public Etiqueta()
        {
            EtiquetaAnuncios = new HashSet<EtiquetaAnuncio>();
        }

        public int IdEtiqueta { get; set; }
        public string? DescripcionEtiqueta { get; set; }

        public virtual ICollection<EtiquetaAnuncio> EtiquetaAnuncios { get; set; }



    }
}
