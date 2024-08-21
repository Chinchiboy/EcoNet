using EcoNet.DAL;
using EcoNet.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace EcoNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController()
        {
           
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

        public IActionResult ObtenerAnuncios(string filtro)
        {
            TempData["Filtro"] = filtro;
            return RedirectToAction("Index");
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

        public IActionResult AgregarProducto()
        {
            IndexViewModel vm = new IndexViewModel();
            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult MostrarAnuncio(int id, DalAnuncio dalAnuncio)
        {
            Anuncio a = dalAnuncio.SelectById(id);

            if (a == null) return NotFound();

            return View(a);
        }

        [HttpPost]
        public IActionResult AgregarProducto(string title, string description, decimal price)
        {
            try
            {
                string usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(usuarioIdClaim))
                {
                    return RedirectToAction("Error", "Home");
                }

                if (!int.TryParse(usuarioIdClaim, out int usuarioId))
                {
                    return RedirectToAction("Error", "Home");
                }

                Anuncio nuevoAnuncio = new()
                {
                    Titulo = title,
                    Descripcion = description,
                    Precio = price,
                    Fkusuario = usuarioId,
                    EstaVendido = false
                };

                DalAnuncio dalAnuncio = new();
                dalAnuncio.Add(nuevoAnuncio);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Manejo de errores, registrar el error y redirigir a una página de error
                Debug.WriteLine($"Error en AgregarProducto: {ex.Message}");
                return RedirectToAction("Error", "Home");
            }
        }

    }
}