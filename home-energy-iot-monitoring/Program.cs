using home_energy_iot_monitoring.Hubs;
using Microsoft.AspNetCore.ResponseCompression;
using System.Net;
using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
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
app.Map("/consocket", async context =>
{
    if (!context.WebSockets.IsWebSocketRequest)
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    else
    {
        Console.WriteLine("Cliente websokcet conectou: "+context.TraceIdentifier);
        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        while (true)
        {
            await webSocket.SendAsync(
                Encoding.ASCII.GetBytes($"Info -> {DateTime.Now}"),
                WebSocketMessageType.Text,
                true, CancellationToken.None);
            await Task.Delay(1000);
        }
    }
});


//app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<DevicesHub>("/devicehub");

app.Run();
