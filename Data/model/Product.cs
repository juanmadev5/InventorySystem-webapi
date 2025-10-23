using System.ComponentModel.DataAnnotations;

namespace InventorySystem_webapi.Data.model;

public class Product
{
    [Key]
    public required string Id { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Category { get; set; }

    [Required]
    public required int Quantity { get; set; }

    [Required]
    public required decimal Price { get; set; }
}