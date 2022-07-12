namespace home_energy_iot_monitoring.Sockets
{
    public interface IWebSocketHolder
    {
        Task AddAsync(HttpContext context);
    }
}
