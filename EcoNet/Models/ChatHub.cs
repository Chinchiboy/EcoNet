using EcoNet.DAL;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace EcoNet.Models
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(int user, string message, int chatId)
        {
            try
            {
                
                var mensaje = new Mensaje
                {
                    Texto = message,
                    Fkchat = chatId,
                    Creador = user,
                    HoraMensaje = DateTime.UtcNow
                };

                DalMensaje mensajeDal = new();
                mensajeDal.Add(mensaje);
                await Clients.All.SendAsync("ReceiveMessage", user, message);
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.Message);
            }
        }

        public async Task SendOffer(float precioOferta, int user, int chatId)
        {
            try
            {
                Oferta oferta = new()
                {
                    Precio = (decimal)precioOferta,
                    Fkchat = chatId,
                    Aceptada = 0,
                    CreadoPor = user,
                    FechaCreacion = DateTime.UtcNow
                };
                DalOferta d = new();
                var item = d.Add(oferta);

                await Clients.All.SendAsync("ReciveOffer", chatId, item);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }
    }
}
