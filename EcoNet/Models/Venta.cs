using System;
using System.Collections.Generic;

namespace EcoNet.Models
{
    public partial class Venta
    {
        public int IdVenta { get; set; }
        public decimal Precio { get; set; }
        public int Fkoferta { get; set; }
    }
}
