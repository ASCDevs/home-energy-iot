namespace home_energy_iot_monitoring.Interfaces
{
    public interface IReportAPI
    {
        Task SaveEnergyValue(string energyValue, string deviceId);
    }
}
