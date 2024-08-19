using System;
using System.Collections.Generic;

namespace EcoNet.Models
{
    public partial class Anuncio
    {
        public Anuncio()
        {
            Chats = new HashSet<Chat>();
            EtiquetaAnuncios = new HashSet<EtiquetaAnuncio>();
        }

        public int IdAnuncio { get; set; }
        public string Titulo { get; set; } = null!;
        public byte[] Imagen { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public decimal Precio { get; set; }
        public int? FkborradoPor { get; set; }
        public int Fkusuario { get; set; }
        public bool EstaVendido { get; set; }

        public virtual Usuario? FkborradoPorNavigation { get; set; }
        public virtual Usuario FkusuarioNavigation { get; set; }
        public virtual ICollection<Chat> Chats { get; set; }
        public virtual ICollection<EtiquetaAnuncio> EtiquetaAnuncios { get; set; }
    }
}
