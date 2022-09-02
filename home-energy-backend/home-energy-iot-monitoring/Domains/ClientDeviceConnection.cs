using System.Net.WebSockets;

namespace home_energy_iot_monitoring.Domains
{
    public class ClientDeviceConnection
    {
        public WebSocket web_socket { get; set; }
        public string device_id { get; set; }
        public string conn_id { get; set; }
        public string dateconn { get; set; }
        public bool current_sate { get; set; }
        //Pode-se implementar uma variavel para guardar o token autenticado do dispositivo

        public ClientDeviceConnection(WebSocket webSocket, string connId)
        {
            web_socket = webSocket;
            conn_id = connId;
            dateconn = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            current_sate = true;
        }

        public void AddDeviceId(string deviceID)
        {
            device_id = deviceID;
        }

        public void ChangeCurrentState(string action)
        {
            if(action == "stopenergy") this.current_sate = false;
            if(action == "continueenergy") this.current_sate = true;
            if(action == "timerenergy") this.current_sate =false;
        }
    }
}