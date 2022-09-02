namespace home_energy_iot_monitoring.Domains
{
    public class CostumerConnection
    {
        public string? costumer_id { get; set; }
        public string? device_id { get; set; }
        public string conn_id { get; set; }

        public CostumerConnection(string ConnId)
        {
            conn_id = ConnId;
        }

        public void AddInfoCostumer(string DeviceId, string CostumerId)
        {
            costumer_id = CostumerId;
            device_id = DeviceId;
        }
    }
}
