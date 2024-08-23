using EcoNet.DAL;
using EcoNet.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Globalization;
using EcoNet.ViewModels;

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

        [HttpGet]
        public IActionResult BuscarAnuncios(string gsearch)
        {
            if (string.IsNullOrEmpty(gsearch))
            {
                return RedirectToAction("Index");
            }

            DalAnuncio dalAnuncio = new();
            List<Anuncio> anuncios = dalAnuncio.Search(gsearch);
            TempData["AnunciosBuscados"] = anuncios;
            TempData["TextoBusqueda"] = gsearch;

            return RedirectToAction("Index");
        }

        public IActionResult Nosotros()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Producto(int id)
        {
            DalAnuncio dalAnuncio = new();
            DalEtiqueta dalEtiqueta = new();
            DalEtiquetaAnuncio dalEtiquetaAnuncio = new();

            // Obtener el producto actual
            Anuncio productoActual = dalAnuncio.SelectById(id);

            if (productoActual == null)
            {
                return NotFound();
            }

            List<EtiquetaAnuncio> relacionesEtiquetaAnuncio = dalEtiquetaAnuncio.SelectByFKAnuncio(id);

            // Crear una lista para almacenar las descripciones de las etiquetas
            List<Etiqueta> etiquetasDelAnuncio = new List<Etiqueta>();

            foreach (var relacion in relacionesEtiquetaAnuncio)
            {
                // Obtener la etiqueta completa usando el ID de la relación
                var etiqueta = dalEtiqueta.SelectById(relacion.Fketiqueta);
                if (etiqueta != null)
                {
                    etiquetasDelAnuncio.Add(etiqueta); // Agregar la etiqueta a la lista
                }
            }

            // Obtener productos relacionados usando las etiquetas
            List<int> etiquetaIds = etiquetasDelAnuncio.Select(e => e.IdEtiqueta).ToList();
            List<Anuncio> productosRelacionados = dalAnuncio.SelectByEtiquetas(etiquetaIds);

            // Crear el ViewModel
            IndexViewModel vm = new()
            {
                ProductoActual = productoActual,
                EtiquetaAnuncioVM = new EtiquetaAnuncioViewModel
                {
                    ArticulosFiltrados = productosRelacionados ?? new List<Anuncio>(),
                    EtiquetaAnuncios = etiquetasDelAnuncio // Pasar las etiquetas con la descripción
                }
            };

            return View(vm);
        }


        public IActionResult AgregarProducto()
        {
            IndexViewModel vm = new IndexViewModel();
            DalEtiqueta dalEtiqueta = new DalEtiqueta();
            List<Etiqueta> listaE = dalEtiqueta.Select();
            vm.ListaEtiquetas = listaE;
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
        public IActionResult AgregarProducto(string title, string description, string price, List<int> selectedEtiquetas)
        {
            string usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!decimal.TryParse(price, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal precio))
            {
                ModelState.AddModelError("price", "El precio no es válido.");
                return View();
            }

            using var transaction = new System.Transactions.TransactionScope();
            try
            {
                Anuncio nuevoAnuncio = new()
                {
                    Titulo = title,
                    Descripcion = description,
                    Precio = precio,
                    Fkusuario = int.Parse(usuarioId),
                    EstaVendido = false
                };

                DalAnuncio dalAnuncio = new();
                nuevoAnuncio.IdAnuncio = dalAnuncio.Add(nuevoAnuncio);

                if (nuevoAnuncio.IdAnuncio <= 0)
                {
                    throw new Exception("Error al insertar el anuncio.");
                }

                DalEtiquetaAnuncio dalEtiquetaAnuncio = new();
                foreach (int etiquetaId in selectedEtiquetas)
                {
                    dalEtiquetaAnuncio.AsignarEtiquetaAnuncio(nuevoAnuncio.IdAnuncio, etiquetaId);
                }

                transaction.Complete();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                transaction.Dispose();
                ModelState.AddModelError(string.Empty, "Error al agregar el producto: " + ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult ProductosRelacionados(int id)
        {
            DalAnuncio dalAnuncio = new();
            DalEtiqueta dalEtiqueta = new();

            Anuncio productoActual = dalAnuncio.SelectById(id);

            if (productoActual == null)
            {
                return NotFound();
            }

            List<Etiqueta> etiquetas = dalEtiqueta.SelectEtiquetasByProductoId(id);
            List<Anuncio> productosRelacionados = dalAnuncio.SelectByEtiquetas(etiquetas);

            IndexViewModel viewModel = new IndexViewModel
            {
                ProductoActual = productoActual,
                EtiquetaAnuncioVM = new EtiquetaAnuncioViewModel
                {
                    ArticulosFiltrados = productosRelacionados
                }
            };

            return View(viewModel);
        }
    }
}