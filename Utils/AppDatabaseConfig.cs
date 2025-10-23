using InventorySystem_webapi.Data;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem_webapi.Utils
{
    public static class AppDatabaseConfig
    {
        public static void ConfigureDatabase(IServiceCollection services, IWebHostEnvironment env, IConfiguration config)
        {
            if (env.IsDevelopment())
                services.AddDbContext<InventoryDbContext>(o => o.UseInMemoryDatabase("InventoryDbDev"));
            else
                services.AddDbContext<InventoryDbContext>(o => o.UseSqlServer(config["INVENTORY_DB_CONNECTION"]));
        }
    }
}