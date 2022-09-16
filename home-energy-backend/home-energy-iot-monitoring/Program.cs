using home_energy_iot_monitoring.Domains;
using home_energy_iot_monitoring.Hubs;
using home_energy_iot_monitoring.Infrasctructure;
using home_energy_iot_monitoring.Interfaces;
using home_energy_iot_monitoring.Sockets;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR().AddJsonProtocol(options =>
{
    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddSingleton<IDeviceSocketHolder, DeviceSocketHolder>();
builder.Services.AddSingleton<IReportAPI, ReportAPI>();
builder.Services.AddSingleton<IPanelHubControl, PanelHubControl>();
builder.Services.AddSingleton<ICostumerHubControl, CostumerHubControl>();
builder.Services.AddResponseCompression(options =>
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" })
);
builder.Services.AddCors();

var app = builder.Build();

app.UseResponseCompression();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseCors(p =>
{
    p.AllowAnyHeader();
    p.AllowAnyMethod();
    p.AllowAnyOrigin();
});
app.UseAuthorization();

app.UseWebSockets();
app.MapDeviceSocketHolder("/consocket");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<PanelsHub>("/panelhub");
app.MapHub<CostumersHub>("/costumerhub");

app.Run();