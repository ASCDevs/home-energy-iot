using home_energy_iot_core;
using home_energy_iot_core.Helpers;
using home_energy_iot_core.Helpers.Interfaces;
using home_energy_iot_core.Interfaces;
using home_energy_iot_core.Login;
using home_energy_iot_entities;
using home_energy_iot_repository;
using home_energy_iot_repository.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using home_energy_iot_core.Login.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddDbContext<DataBaseContext>();

//services
builder.Services.AddTransient<IUserManager, UserManager>();
builder.Services.AddTransient<IHouseManager, HouseManager>();
builder.Services.AddTransient<IDeviceManager, DeviceManager>();
builder.Services.AddTransient<IDeviceReporter, DeviceReporter>();
builder.Services.AddTransient<IDeviceConsumptionReader, DeviceConsumptionReader>();

//repositories
builder.Services.AddTransient<IDeviceManagerRepository, DeviceManagerRepository>();
builder.Services.AddTransient<IDeviceReporterRepository, DeviceReporterRepository>();
builder.Services.AddTransient<IHouseManagerRepository, HouseManagerRepository>();
builder.Services.AddTransient<IUserManagerRepository, UserManagerRepository>();
builder.Services.AddTransient<IDeviceConsumptionReaderRepository, DeviceConsumptionReaderRepository>();

//helpers
builder.Services.AddTransient<IHasher, Hasher>();
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddTransient<ILoginService, LoginService>();

builder.Logging.AddLog4Net();

var key = Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("SecretJwtKey"));

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseCors(p =>
{
    p.AllowAnyHeader();
    p.AllowAnyMethod();
    p.AllowAnyOrigin();
});

app.UseAuthorization();

app.MapControllers();

app.Run();