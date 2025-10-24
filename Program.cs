using System.Text;
using dotenv.net;
using InventorySystem_webapi.Constants;
using InventorySystem_webapi.Data;
using InventorySystem_webapi.domain.@interface;
using InventorySystem_webapi.domain.repository;
using InventorySystem_webapi.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(AppPolicy.apiPolicy, policy => SetPolicies.SetAppPolicies(policy));
});

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        AppExtensions.SetJwtOptions(options, jwtSettings, key);
    });

AppDatabaseConfig.ConfigureDatabase(
    builder.Services,
    builder.Environment,
    builder.Configuration["INVENTORY_DB_CONNECTION"]
);

builder.Services.AddControllers();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

if (args.Length == 1 && args[0].Equals("--migrate", StringComparison.OrdinalIgnoreCase))
{
    Console.WriteLine("Ejecutando proceso de migración forzada...");

    using (var scope = app.Services.CreateScope())
    {
        AutoMigration.ApplyMigrations(scope);
        Console.WriteLine("Migraciones de base de datos aplicadas con éxito.");
    }

    return;
}

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    using var scope = app.Services.CreateScope();
    AutoMigration.ApplyMigrations(scope);
    Console.WriteLine("Migraciones automáticas aplicadas en desarrollo/staging.");
}
else
{
    Console.WriteLine("Entorno de Producción: Las migraciones se ejecutan mediante el entrypoint.sh.");
}


app.UseCors(AppPolicy.apiPolicy);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
