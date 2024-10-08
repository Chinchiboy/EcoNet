﻿using System;
using System.Collections.Generic;

namespace EcoNet.Models
{
    public partial class Oferta
    {
        public int IdOferta { get; set; }
        public decimal Precio { get; set; }
        public int Fkchat { get; set; }
        public Int16 Aceptada { get; set; }
        public int CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }

        public virtual Chat FkchatNavigation { get; set; }
    }
}
