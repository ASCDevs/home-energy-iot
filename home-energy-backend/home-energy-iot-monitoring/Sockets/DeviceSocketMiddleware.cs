using home_energy_iot_monitoring.Interfaces;

namespace home_energy_iot_monitoring.Sockets
{
    public class DeviceSocketMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IDeviceSocketHolder deviceSocket;

        public DeviceSocketMiddleware(RequestDelegate next, IDeviceSocketHolder deviceSocket)
        {
            this.next = next;
            this.deviceSocket = deviceSocket;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest == false)
            {
                context.Response.StatusCode = 400;
                return;
            }
            await deviceSocket.AddAsync(context);
        }
    }
}