using EcoNet.DAL;
using Microsoft.AspNetCore.Mvc;

namespace EcoNet.Controllers
{
    public class OfertaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public void UpdateOferta(int idOferta, Int16 aceptada)
        {
            DalOferta aux = new();
            if(!aux.UpdateStatus(idOferta, aceptada))
            {
                Console.WriteLine("Something went wrong while trying to update an offert status");
            }

        }
    }
}
