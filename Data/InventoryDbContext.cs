using InventorySystem_webapi.Data.model;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem_webapi.Data;

public class InventoryDbContext(DbContextOptions<InventoryDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
}