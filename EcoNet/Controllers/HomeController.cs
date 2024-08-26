using EcoNet.DAL;
using EcoNet.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Globalization;
using EcoNet.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

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
            var dalAnuncio = new DalAnuncio();  // Asegúrate de instanciar correctamente el DAL
            var anunciosFiltrados = dalAnuncio.Search(gsearch);

            var model = new EtiquetaAnuncio
            {
                ArticulosFiltrados = anunciosFiltrados
            };

            return PartialView("_AnunciosParTial", model);
        }

        public IActionResult Nosotros()
        {
            return View();
        }

        public IActionResult Profile()
        {
            //SubirImagen();
            var nameClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            DalUsuario dalUsuario = new DalUsuario();
            if (nameClaim != null)
            {
                Usuario? usuario = dalUsuario.SelectById(int.Parse(nameClaim));
                byte[] imageBytes = null;
                Debug.WriteLine("Usuario " + usuario + " " + nameClaim);
                return View(usuario);
            }
            return RedirectToAction("Index", "Home");

        }

        public void SubirImagen(string rutaImagenPC)
        {
            IFormFile imagen = CreateFormFileFromPath(rutaImagenPC);

            byte[] imageBytes = null;
            if (imagen != null && imagen.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    imagen.CopyTo(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }
            }

            var nameClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            DalUsuario dalUsuario = new DalUsuario();

            Usuario? usuario = dalUsuario.SelectById(int.Parse(nameClaim));
            usuario.FotoPerfil = imageBytes;

            dalUsuario.UpdateImagen(usuario);

        }

        public static IFormFile CreateFormFileFromPath(string filePath)
        {
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var fileName = Path.GetFileName(filePath);
            var contentType = "application/octet-stream"; // Puedes ajustar el tipo de contenido según el archivo

            var formFile = new FormFile(fileStream, 0, fileStream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };

            return formFile;
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

            Anuncio productoActual = dalAnuncio.SelectById(id);

            if (productoActual == null)
            {
                return NotFound();
            }

            List<string> descripciones = dalEtiquetaAnuncio.SelectDescriptionsByFKAnuncio(id);

            List<int> etiquetaIds = dalEtiqueta.SelectIdsByDescriptions(descripciones);

            List<Anuncio> productosRelacionados = dalAnuncio.SelectByEtiquetas(etiquetaIds);

            IndexViewModel vm = new()
            {
                ProductoActual = productoActual,
                EtiquetaAnuncioVM = new EtiquetaAnuncioViewModel
                {
                    ArticulosFiltrados = productosRelacionados ?? new List<Anuncio>(),
                    EtiquetaAnuncios = descripciones.Select(d => new Etiqueta { DescripcionEtiqueta = d }).ToList()
                }
            };

            return View(vm);
        }

        public IActionResult AgregarProducto()
        {
            try
            {
                if (!User.IsInRole("Logged"))
                {
                    TempData["MostrarModal"] = true;
                    return RedirectToAction("Index", "Home");
                }

                IndexViewModel vm = new();
                DalEtiqueta dalEtiqueta = new();

                try
                {
                    List<Etiqueta> listaE = dalEtiqueta.Select();
                    vm.ListaEtiquetas = listaE;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"SQL Error: {ex.Message}");
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"General Error: {ex.Message}");
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }

                return View(vm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AgregarProducto: {ex.Message}");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }


        public IActionResult MostrarAnuncio(int id, DalAnuncio dalAnuncio)
        {
            Anuncio a = dalAnuncio.SelectById(id);

            if (a == null) return NotFound();

            return View(a);
        }

        public IActionResult Politicas()
        {
            IndexViewModel vm = new();
            return View(vm);
        }

        [HttpPost]
        public IActionResult AgregarProducto(string title, string description, string price, List<int> selectedEtiquetas, IFormFile imagen)
        {
            string usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            byte[] imageBytes = null;
            if (imagen != null && imagen.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    imagen.CopyTo(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }
            }

            using var transaction = new System.Transactions.TransactionScope();
            try
            {
                if (!decimal.TryParse(price, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal precio))
                {
                    ModelState.AddModelError("price", "El precio no es válido.");
                    return View();
                }

                Anuncio nuevoAnuncio = new()
                {
                    Titulo = title,
                    Descripcion = description,
                    Precio = precio,
                    Fkusuario = int.Parse(usuarioId),
                    EstaVendido = false,
                    Imagen = imageBytes
                };

                DalAnuncio dalAnuncio = new();

                try
                {
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
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"SQL Error: {ex.Message}");
                    transaction.Dispose();
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"General Error: {ex.Message}");
                    transaction.Dispose();
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                transaction.Dispose();
                Console.WriteLine($"Error en AgregarProducto: {ex.Message}");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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

        public IActionResult ProductosPorEtiqueta(int etiquetaId)
        {
            DalAnuncio dalAnuncio = new();
            DalEtiqueta dalEtiqueta = new();
            List<Anuncio> productosRelacionados = dalAnuncio.SelectByEtiqueta(etiquetaId);
            Etiqueta etiquetaSeleccionada = dalEtiqueta.SelectById(etiquetaId);

            if (etiquetaSeleccionada == null)
            {
                return NotFound();
            }

            IndexViewModel viewModel = new IndexViewModel
            {
                EtiquetaAnuncioVM = new EtiquetaAnuncioViewModel
                {
                    ArticulosFiltrados = productosRelacionados,
                    EtiquetaAnuncios = new List<Etiqueta> { etiquetaSeleccionada }
                }
            };

            return View(viewModel);
        }

    }
}