using System.Net.WebSockets;

namespace home_energy_iot_monitoring.Sockets
{
    public class ClientDeviceConnection
    {
        public WebSocket web_socket { get; set; }
        public string device_id { get; set; }
        public string conn_id { get; set; }
        public string dateconn { get; set; }
        //Pode-se implementar uma variavel para guardar o token autenticado do dispositivo

        public ClientDeviceConnection(WebSocket webSocket, string deviceId, string connId)
        {
            web_socket = webSocket;
            device_id = deviceId;
            conn_id = connId;
            dateconn = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }
    }
}