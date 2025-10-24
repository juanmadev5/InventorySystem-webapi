using InventorySystem_webapi.Data;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem_webapi.Utils
{
    public static class AutoMigration
    {
        public static void ApplyMigrations(IServiceScope? scope)
        {
            using (scope)
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
                if (dbContext.Database.IsRelational())
                {
                    try
                    {
                        dbContext.Database.Migrate();
                        Console.WriteLine("Migraciones aplicadas correctamente.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al aplicar migraciones: {ex.Message}");
                    }
                    dbContext.Database.Migrate();
                }
            }
        }
    }
}
