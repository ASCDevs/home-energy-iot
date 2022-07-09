using System.Net.WebSockets;
using System.Net;
using System.Text;

namespace home_energy_iot_monitoring.Sockets
{
    public class SocketClient
    {
        public WebSocket? _websocket { get; set; }
        public string NameSocket { get; set; }

        public SocketClient(WebSocket? websocket)
        {
            _websocket = websocket;
            this.LoopInfo();
        }

        public async Task LoopInfo()
        {
            
                while (true)
                {
                    await _websocket.SendAsync(
                        Encoding.ASCII.GetBytes($"Info -> {DateTime.Now} - "),//+SocketsHandler._connectedClients.Count+" online"
                        WebSocketMessageType.Text,
                        true, CancellationToken.None);
                    await Task.Delay(1000);
                }
            
        }
    }
}
