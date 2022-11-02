using System.Net.WebSockets;

namespace home_energy_iot_monitoring.Domains
{
    public class ClientDeviceConnection
    {
        public WebSocket web_socket { get; set; }
        public string device_id { get; set; }
        public string device_ip { get; set; }
        public string conn_id { get; set; }
        public string dateconn { get; set; }
        public bool current_sate { get; set; }
        public DateTime last_ok_confirmation { get; set; }

        public ClientDeviceConnection(WebSocket webSocket, string connId)
        {
            web_socket = webSocket;
            conn_id = connId;
            dateconn = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            current_sate = true;
            last_ok_confirmation = DateTime.Now;
        }

        public void AddDeviceId(string deviceID)
        {
            device_id = deviceID;
        }

        public void AddDeviceIp(string deviceIP)
        {
            device_ip = deviceIP;
        }

        public void ChangeCurrentState(string action)
        {
            if(action == "stopenergy") this.current_sate = false;
            if(action == "continueenergy") this.current_sate = true;
            if(action == "timerenergy") this.current_sate =false;
        }

        public bool IsInactive()
        {
            int secTimeOut = 5;
            DateTime dtNow = DateTime.Now;
            int secondsDiff = (int)(dtNow - this.last_ok_confirmation).TotalSeconds;
            if (secondsDiff > secTimeOut) return true;
            return false;
        }

        public void UpdateLastConfirmation()
        {
            this.last_ok_confirmation = DateTime.Now;
        }
    }
}