using home_energy_iot_monitoring.Interfaces;

namespace home_energy_iot_monitoring.Sockets
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IWebSocketHolder webSocket;

        public WebSocketMiddleware(RequestDelegate next, IWebSocketHolder webSocket)
        {
            this.next = next;
            this.webSocket = webSocket;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest == false)
            {
                context.Response.StatusCode = 400;
                return;
            }
            await webSocket.AddAsync(context);
        }
    }
}