﻿using EcoNet.DAL;
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
     
        public IActionResult BuscarAnuncios(string gsearch)
        {
            DalAnuncio dalAnuncio = new DalAnuncio();
            var anuncios = dalAnuncio.Search(gsearch); // Usar tu método DAL para buscar los anuncios
            var resultado = anuncios.Select(a => new {
                idAnuncio = a.IdAnuncio,
                titulo = a.Titulo,
                imagenBase64 = a.Imagen != null ? Convert.ToBase64String(a.Imagen) : string.Empty,
                precio = a.Precio
            });

            return Json(resultado);
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