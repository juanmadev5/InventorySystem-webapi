using System.Text;
using dotenv.net;
using InventorySystem_webapi.Data;
using InventorySystem_webapi.domain.@interface;
using InventorySystem_webapi.domain.repository;
using InventorySystem_webapi.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

const string apiPolicy = "AllowAll";

builder.Services.AddCors(options =>
{
    options.AddPolicy(apiPolicy, policy => SetPolicies.SetAppPolicies(policy));
});

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        AppExtensions.SetJwtOptions(options, jwtSettings, key);
    });

AppDatabaseConfig.ConfigureDatabase(builder.Services, builder.Environment, builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
    if(dbContext.Database.IsRelational())
    {
        dbContext.Database.Migrate();
    }
}

app.UseCors(apiPolicy);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
