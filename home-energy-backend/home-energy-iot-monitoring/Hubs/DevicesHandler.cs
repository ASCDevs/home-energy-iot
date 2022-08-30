using home_energy_iot_monitoring.Sockets;

namespace home_energy_iot_monitoring.Hubs
{
    public static class DevicesHandler
    {
        public static HashSet<string> _connectedDevices = new HashSet<string>();

    }
}