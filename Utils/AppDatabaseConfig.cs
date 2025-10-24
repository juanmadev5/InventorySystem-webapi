using InventorySystem_webapi.Data;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem_webapi.Utils
{
    public static class AppDatabaseConfig
    {
        public static void ConfigureDatabase(
            IServiceCollection services,
            IWebHostEnvironment env,
            string? connectionString
        )
        {
            if (env.IsDevelopment())
            {
                services.AddDbContext<InventoryDbContext>(o =>
                    o.UseInMemoryDatabase("InventoryDbDev")
                );
            }
            else
            {
                var serverVersion = new MySqlServerVersion(new Version(9, 0, 0));

                services.AddDbContext<InventoryDbContext>(o =>
                    o.UseMySql(
                        connectionString,
                        serverVersion,
                        mySqlOptions =>
                            mySqlOptions.EnableRetryOnFailure(
                                maxRetryCount: 10,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null
                            )
                    )
                );
            }
        }
    }
}
