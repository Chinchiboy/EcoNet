using EcoNet.Models;

public partial class Etiqueta
{
    public Etiqueta()
    {
        EtiquetaAnuncios = new HashSet<EtiquetaAnuncio>();
    }

    public int IdEtiqueta { get; set; }
    public string DescripcionEtiqueta { get; set; } = null!;

    public virtual ICollection<EtiquetaAnuncio> EtiquetaAnuncios { get; set; }

    // Puedes agregar una lista aquí, pero esto sería redundante
    public List<Etiqueta> ListaEtiquetas { get; set; } = new List<Etiqueta>();
}
