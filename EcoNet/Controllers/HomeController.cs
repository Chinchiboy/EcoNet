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
            string usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!decimal.TryParse(price, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal precio))
            {
                ModelState.AddModelError("price", "El precio no es válido.");
            }

            Anuncio nuevoAnuncio = new()
            {
                Titulo = title,
                Descripcion = description,
                Precio = precio,
                Fkusuario = int.Parse(usuarioId),
                EstaVendido = false
            };

            DalAnuncio dalAnuncio = new();
            dalAnuncio.Add(nuevoAnuncio);

            return RedirectToAction("Index", "Home");
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