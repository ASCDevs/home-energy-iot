using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

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
                    if(clients.TryRemove(clients.First(w => w.Value == webSocket))) Console.WriteLine("[disconnected] id-connection: " + client.Key);
                    webSocket.Dispose();
                    break;
                }
                
                string? msgReceive = Encoding.UTF8.GetString(new ArraySegment<byte>(buffer, 0, result.Count));
                //if(msgReceive.Contains(">")) await HandleAction(msgReceive);
                string idWs = clients.First(x => x.Value == webSocket).Key;
                if (msgReceive.Contains("server>")) Console.WriteLine("[From " + idWs + "]" + msgReceive);

                //Enviar para todos os clients
                foreach (var c in clients)
                {
                    if (!msgReceive.Contains("server>"))
                    {
                        await c.Value.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                    }
                }
            }
        }

        public int CountClients()
        {
            return clients.Count(); 
        }

        public async Task SendActionToClient(string idConnection, string Action)
        {
            var client = clients.First(x => x.Key == idConnection);
            byte[] bytes = Encoding.ASCII.GetBytes(Action);
            
            await client.Value.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text , true, CancellationToken.None) ;
        }

        private async Task HandleAction(string txtCommand, WebSocket ws)
        {
            //{para-quem}>{acao}>{valor}
            //ex: server>updateid>1239
            //{para-quem}>{acao}>{idclient,acao}
            //ex: client>sendaction>122,desligarsensor

            string para_quem = txtCommand.Split(">")[0];
            string acao = txtCommand.Split(">")[1];
            string valor = txtCommand.Split(">")[2];

            
            

        }
    }
}
