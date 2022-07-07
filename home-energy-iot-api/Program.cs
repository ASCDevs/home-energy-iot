using home_energy_iot_core;
using home_energy_iot_core.Helpers;
using home_energy_iot_core.Helpers.Interfaces;
using home_energy_iot_core.Interfaces;
using home_energy_iot_entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataBaseContext>();

builder.Services.AddTransient<IUserManager, UserManager>();
builder.Services.AddTransient<IHasher, Hasher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();