using System;
using System.Collections.Generic;

namespace EcoNet.Models
{
    public partial class Particular
    {
        public int IdUsuario { get; set; }
        public string Dni { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Apellidos { get; set; } = null!;

        public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
    }
}
