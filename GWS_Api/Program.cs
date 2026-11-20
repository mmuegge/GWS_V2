using GWS_Api.Models;
using GWS_Api.Repositories;
using GWS_Api.Repositories.Electric;
using GWS_Api.Repositories.Gas;
using GWS_Api.Repositories.Water;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Microsoft.OpenApi;
using GWS_Api.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog direkt aus der appsettings.json konfigurieren
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();


// Standard-Logger durch Serilog ersetzen
builder.Host.UseSerilog();

var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<GWS_DbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// mit Newtonsoft Serialization
builder.Services.AddControllers().AddNewtonsoftJson(s =>
{
    s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
});

// Automapper
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(cfg => 
{
    cfg.LicenseKey = builder.Configuration["AutoMapper:LicenseKey"];
    cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
});  // Automapper für die gesamte Assembly

if (builder.Environment.IsDevelopment())
{
    // Mock-Repository
    builder.Services.AddSingleton<IGWSRepository, Mock_GWSRepository>();  // hier können noch mehr services eingefügt werden
    builder.Services.AddSingleton<IParameterRepository, Mock_ParameterRepository>();  // hier können noch mehr services eingefügt werden
    builder.Services.AddSingleton<IWaterRepository, Mock_WaterRepository>();  // hier können noch mehr services eingefügt werden
    builder.Services.AddSingleton<IGasRepository, Mock_GasRepository>();  // hier können noch mehr services eingefügt werden
    builder.Services.AddSingleton<IElectricRepository, Mock_ElectricRepository>();  // hier können noch mehr services eingefügt werden
}
else
{
    // MySql-Repository
    builder.Services.AddScoped<IGWSRepository, MySQL_GWSRepository>();
    builder.Services.AddScoped<IParameterRepository, MySQL_ParameterRepository>();
    builder.Services.AddScoped<IWaterRepository, MySQL_WaterRepository>();  // hier können noch mehr services eingefügt werden
    builder.Services.AddScoped<IGasRepository, MySQL_GasRepository>();  // hier können noch mehr services eingefügt werden
    builder.Services.AddScoped<IElectricRepository, MySQL_ElectricRepository>();  // hier können noch mehr services eingefügt werden
}

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GWS_Api", Version = "v1" });

    c.CustomOperationIds(apiDesc =>
    {
        var controller = apiDesc.ActionDescriptor.RouteValues["controller"];
        var action = apiDesc.ActionDescriptor.RouteValues["action"];
        return $"{controller}_{action}";
    });
});

// Globaler Exception Handler registrieren
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Globaler Exception Handler Middleware
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GWS_Api v1"));
}

//app.UseHttpsRedirection();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.Run();
}

if (app.Environment.IsProduction())
{
    //app.Run("http://*:5850"); // GWS Pi
    app.Run("http://*:5850");  // Proxmox LAMP
}

// Development: Starten über Konsole --> dotnet run --environment Development
// Vorher in Verzeichnis GWS_Api wechseln
// Swagger im Browser: http://localhost:5202/swagger/index.html
// oder über Properties/launchSettings.json
// unter GWS_Api/Properties/PublishedProfiles/launchSettings.json,
// zusätzlich muss appsettings.json vorhanden sein mit Eintrag:
//  "ConnectionStrings": {"MySqlConnection": "server=192.168.178.156;port=3306;database=gwsdb;user=dbuser;password=xxxx"},
// wenn nicht vorhanden siehe FREEZER_Api
// beim Debugger (Auswahl: debug) muss die Auswahl auf GWS_Api stehen dann wird auf local gestartet (