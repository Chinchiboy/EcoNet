using System;
using System.Collections.Generic;

namespace EcoNet.Models
{
    public partial class Mensaje
    {
        public int IdMensaje { get; set; }
        public string Texto { get; set; } = null!;
        public int Fkchat { get; set; }
        public int Creador { get; set; }
        public DateTime HoraMensaje { get; set; }

        public virtual Chat? FkchatNavigation { get; set; }
    }
}
