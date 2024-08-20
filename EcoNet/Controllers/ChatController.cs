using Microsoft.AspNetCore.Mvc;
using EcoNet.Models;
using System.Collections.Generic;
using EcoNet.DAL;

namespace EcoNet.Controllers
{
    public class ChatController : Controller
    {
        
        public ChatController()
        {
            
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetMessages(int chatId)
        {
            DalMensaje aux = new();
            List<Mensaje>? mensajes = aux.GetMessagesFromChat(chatId);

            return Json(mensajes);
        }
    }
}
