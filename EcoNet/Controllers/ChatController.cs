using Microsoft.AspNetCore.Mvc;
using EcoNet.Models;
using System.Collections.Generic;
using EcoNet.DAL;
using System.Security.Claims;

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

        [HttpGet]
        public IActionResult GetOffers(int chatId) 
        { 
            DalOferta aux = new();
            List<Oferta>? ofertas = aux.SelectOfertaByChat(chatId);

            return Json(ofertas);
        }

        [HttpGet]
        public IActionResult GetChats()
        {
            DalChat aux = new();

            List<ChatViewModel>? chats = aux.SelectUserChats(Int32.Parse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value));
            return Json(chats);
        }
    }
}
