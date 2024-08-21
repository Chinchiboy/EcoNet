using EcoNet.DAL;
using EcoNet.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Globalization;

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
        public IActionResult AgregarProducto(string title, string description, string price)
        {
            // Obtén el ID del usuario desde las claims
            string usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Intenta convertir el valor de price a decimal
            if (!decimal.TryParse(price, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal precio))
            {
                // Maneja el error si la conversión falla
                ModelState.AddModelError("price", "El precio no es válido.");
                return View(); // Regresa a la vista actual si la conversión falla
            }

            // Crea un nuevo anuncio con los datos proporcionados
            Anuncio nuevoAnuncio = new()
            {
                Titulo = title,
                Descripcion = description,
                Precio = precio,
                Fkusuario = int.Parse(usuarioId),
                EstaVendido = false
            };

            // Agrega el anuncio a la base de datos
            DalAnuncio dalAnuncio = new();
            dalAnuncio.Add(nuevoAnuncio);

            // Redirige al usuario a la página principal
            return RedirectToAction("Index", "Home");
        }

    }

}