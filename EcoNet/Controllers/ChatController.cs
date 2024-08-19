using Microsoft.AspNetCore.Mvc;
using EcoNet.Models;
using System.Collections.Generic;
using EcoNet.DAL;

namespace EcoNet.Controllers
{
    public class ChatController : Controller
    {
        private readonly DalChat _dalChat;

        public ChatController(DalChat dalChat)
        {
            _dalChat = dalChat;
        }

        public IActionResult MisChats()
        {
            int userId = ObtenerUsuarioActivoId();

            List<Chat>? chats = _dalChat.SelectUserChats(userId);

            return RedirectToAction("Index", "Home");
            //return View(chats);
        }

        private int ObtenerUsuarioActivoId()
        {
            // Implementa la lógica para obtener el ID del usuario activo.
            // Esto podría depender de la autenticación que estés usando (por ejemplo, Claims, Session, etc.).
            // Aquí asumo que estás usando Claims.
            return int.Parse(User.FindFirst("UserId").Value);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
