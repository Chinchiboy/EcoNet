using System;
using System.Collections.Generic;

namespace EcoNet.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            AnuncioFkborradoPorNavigations = new HashSet<Anuncio>();
            AnuncioFkusuarioNavigations = new HashSet<Anuncio>();
            ChatFkcompradorNavigations = new HashSet<Chat>();
            ChatFkvendedorNavigations = new HashSet<Chat>();
        }

        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = null!;
        public string Contraseña { get; set; } = null!;
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaBaja { get; set; }
        public string Telefono { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Municipio { get; set; } = null!;
        public bool EsAdmin { get; set; }
        public byte[]? FotoPerfil { get; set; }

        public virtual Empresa Empresa { get; set; } = null!;
        public virtual Particular Particular { get; set; } = null!;
        public virtual ICollection<Anuncio> AnuncioFkborradoPorNavigations { get; set; }
        public virtual ICollection<Anuncio> AnuncioFkusuarioNavigations { get; set; }
        public virtual ICollection<Chat> ChatFkcompradorNavigations { get; set; }
        public virtual ICollection<Chat> ChatFkvendedorNavigations { get; set; }
    }
}
