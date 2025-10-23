using System.ComponentModel.DataAnnotations;

namespace InventorySystem_webapi.DTOs;

public class ProductDto
{
    [Required(ErrorMessage = "Code is required.")]
    [StringLength(20, ErrorMessage = "Code cannot exceed 20 characters.")]
    public required string Code { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters.")]
    public required string Category { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be greater than or equal to 0.")]
    public required int Quantity { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public required decimal Price { get; set; }
}