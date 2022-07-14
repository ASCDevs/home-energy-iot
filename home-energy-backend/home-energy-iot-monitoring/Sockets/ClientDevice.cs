using System.Net.WebSockets;

namespace home_energy_iot_monitoring.Sockets
{
    public class ClientDeviceConnection
    {
        public WebSocket web_socket { get; set; }
        public string device_id { get; set; }
        //Pode-se implementar uma variavel para guardar o token autenticado do dispositivo

        public ClientDeviceConnection(WebSocket webSocket, string deviceId)
        {
            web_socket = webSocket;
            device_id = deviceId;
        }
    }
}
