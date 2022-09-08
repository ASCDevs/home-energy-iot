namespace home_energy_iot_monitoring.Sockets
{
    public static class DeviceSocketHolderMapper
    {
        public static IApplicationBuilder MapDeviceSocketHolder(this IApplicationBuilder app, PathString path)
        {
            return app.Map(path, (app) => app.UseMiddleware<DeviceSocketMiddleware>());
        }
    }
}