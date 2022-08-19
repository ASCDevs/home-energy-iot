namespace home_energy_iot_monitoring.Sockets
{
    public static class WebSocketHolderMapper
    {
        public static IApplicationBuilder MapWebSocketHolder(this IApplicationBuilder app, PathString path)
        {
            return app.Map(path, (app) => app.UseMiddleware<WebSocketMiddleware>());
        }
    }
}