using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace home_energy_iot_monitoring.Sockets
{
    public class WebSocketHolder : IWebSocketHolder
    {
        private readonly ILogger<WebSocketHolder> logger;
        private readonly ConcurrentDictionary<string, WebSocket> clients = new();

        public WebSocketHolder(ILogger<WebSocketHolder> logger)
        {
            this.logger = logger;
        }

        public async Task AddAsync(HttpContext context)
        {
            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
            string idConnection = CreateId();
            if (clients.TryAdd(idConnection, webSocket))
            {
                Console.WriteLine("[connected] id-connection: " + idConnection);
                await EchoAsycn(webSocket);
            }
        }

        private string CreateId()
        {
            return Guid.NewGuid().ToString(); 
        }

        private async Task EchoAsycn(WebSocket webSocket)
        {
            //Para enviar dados
            byte[] buffer = new byte[1024*4];
            //Receber e enviar mensagens até as conexões serem fechadas.
            while (true)
            {
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.CloseStatus.HasValue)
                {
                    await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                    var client = clients.First(x => x.Value == webSocket);
                    if(clients.TryRemove(clients.First(w => w.Value == webSocket))) Console.WriteLine("[disconnected] id-connection: " + client.Key); ;
                    webSocket.Dispose();
                    break;
                }

                //Enviar para todos os clients
                foreach(var c in clients)
                {
                    await c.Value.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                }
            }
        }
    }
}
