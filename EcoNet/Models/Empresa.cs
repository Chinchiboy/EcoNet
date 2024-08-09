using System;
using System.Collections.Generic;

namespace EcoNet.Models
{
    public partial class Empresa
    {
        public int IdUsuario { get; set; }
        public string Cif { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public bool? EsRecicladora { get; set; }
        public string? Direccion { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
    }
}
