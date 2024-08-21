using EcoNet.Models;

public class IndexViewModel
{
    public List<Etiqueta> ListaEtiquetas { get; set; } = new List<Etiqueta>();

    // Otras propiedades que la vista necesita
    public EtiquetaAnuncio EtiquetaAnuncio { get; set; } = new EtiquetaAnuncio();
    public EtiquetaFiltros EtiquetaFiltros { get; set; } = new EtiquetaFiltros();
    public Etiqueta Etiqueta { get; set; } = new Etiqueta();
}
