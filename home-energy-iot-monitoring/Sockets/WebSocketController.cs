using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.WebSockets;
using System.Text;

namespace home_energy_iot_monitoring.Sockets
{
    public class WebSocketController : ControllerBase
    {
        [HttpGet("/consocket")]
        public async Task Get()
        {
            if (!HttpContext.WebSockets.IsWebSocketRequest)
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            else
            {
                Console.WriteLine("Cliente websokcet conectou: " + HttpContext.TraceIdentifier);
                //using var webSocket = 
                SocketsHandler._connectedClients.Add(new SocketClient(await HttpContext.WebSockets.AcceptWebSocketAsync()));
            }
        }
    }
}
