using EcoNet.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EcoNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            IndexViewModel vm = new IndexViewModel();
            DalAnuncio dalAnuncio = new DalAnuncio();

            vm.EtiquetaFiltros = new EtiquetaFiltros();

            vm.EtiquetaAnuncio = new EtiquetaAnuncio();
            vm.EtiquetaAnuncio.ArticulosFiltrados = TempData["Filtro"] == null ? dalAnuncio.Select() : dalAnuncio.SelectByTag(TempData["Filtro"].ToString());

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Producto()
        {
            IndexViewModel vm = new IndexViewModel();
            DalAnuncio dalAnuncio = new DalAnuncio();

            vm.EtiquetaFiltros = new EtiquetaFiltros();

            vm.EtiquetaAnuncio = new EtiquetaAnuncio();
            vm.EtiquetaAnuncio.ArticulosFiltrados = TempData["Filtro"] == null ? dalAnuncio.Select() : dalAnuncio.SelectByTag(TempData["Filtro"].ToString());
            return View(vm);
        }

        public IActionResult ObtenerAnuncios(string filtro)
        {
            TempData["Filtro"] = filtro;
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}