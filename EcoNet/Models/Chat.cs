using System;
using System.Collections.Generic;

namespace EcoNet.Models
{
    public partial class Chat
    {
        public Chat()
        {
            Mensajes = new HashSet<Mensaje>();
            Oferta = new HashSet<Oferta>();
        }

        public int IdChat { get; set; }
        public int Fkanuncio { get; set; }
        public int Fkvendedor { get; set; }
        public int Fkcomprador { get; set; }

        public virtual Anuncio FkanuncioNavigation { get; set; }
        public virtual Usuario FkcompradorNavigation { get; set; }
        public virtual Usuario FkvendedorNavigation { get; set; }
        public virtual ICollection<Mensaje> Mensajes { get; set; }
        public virtual ICollection<Oferta> Oferta { get; set; }

        public static implicit operator Chat(List<Chat> v)
        {
            throw new NotImplementedException();
        }
    }
}
