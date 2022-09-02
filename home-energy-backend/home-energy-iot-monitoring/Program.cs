using home_energy_iot_monitoring.Domains;
using home_energy_iot_monitoring.Hubs;
using home_energy_iot_monitoring.Interfaces;
using home_energy_iot_monitoring.Sockets;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR().AddJsonProtocol(options =>
{
    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddSingleton<IWebSocketHolder, WebSocketHolder>();
builder.Services.AddSingleton<IReportAPI, ReportAPI>();
builder.Services.AddResponseCompression(options =>
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" })
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseResponseCompression();
app.UseStaticFiles();
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();

app.UseWebSockets();
app.MapWebSocketHolder("/consocket");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<PanelsHub>("/panelhub");
app.MapHub<DevicesHub>("/devicehub");
app.MapHub<CostumersHub>("/costumerhub");

app.Run();