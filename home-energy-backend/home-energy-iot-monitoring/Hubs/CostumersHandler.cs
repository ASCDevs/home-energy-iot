using home_energy_iot_monitoring.Domains;

namespace home_energy_iot_monitoring.Hubs
{
    public static class CostumersHandler
    {
        public static HashSet<CostumerConnection> _connectedCostumers = new HashSet<CostumerConnection>();

        public static List<CostumerConnection> GetCostumerByDevice(string DeviceId)
        {
           return _connectedCostumers.Where(x => x.device_id == DeviceId).ToList();
        }

        public static CostumerConnection GetCostumerByConnection(string ConnId)
        {
            return _connectedCostumers.First(x => x.conn_id == ConnId);
        }
    }
    
}
