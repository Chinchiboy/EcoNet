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

            vm.EtiquetaFiltros = new EtiquetaFiltros();

            vm.EtiquetaAnuncio = new EtiquetaAnuncio();

            List<Anuncio> ListAn = new List<Anuncio>();
            
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Producto()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
