using InventorySystem_webapi.Constants;
using InventorySystem_webapi.domain.@interface;
using InventorySystem_webapi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem_webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // JWT Required
public class InventoryController(IProductRepository repository) : ControllerBase
{
    // READY!
    // GET: /api/Inventory/products
    [HttpGet("products")]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await repository.GetAllProductsAsync();
        return Ok(products);
    }

    // READY!
    // GET: /api/Inventory/products/{code}
    [HttpGet("products/{code}")]
    public async Task<IActionResult> GetProductByCode(string code)
    {
        var product = await repository.GetProductByCodeAsync(code);
        if (product == null)
        {
            return NotFound(new { message = InventoryMessages.ProductNotFound });
        }

        return Ok(product);
    }

    // READY!
    // GET /api/Inventory/categories?category={name}
    [HttpGet("categories")]
    public async Task<IActionResult> GetProductsByCategory([FromQuery] string category)
    {
        if (string.IsNullOrWhiteSpace(category))
            return BadRequest(new { message = InventoryMessages.CategoryNameRequired });

        var products = await repository.GetProductsByCategoryAsync(category);

        if (products.Count.Equals(0))
            return NotFound(new { message = string.Format(InventoryMessages.NoProductsInCategoryFormat, category) });

        return Ok(products);
    }

    // READY!
    // POST: /api/Inventory/products/add
    [HttpPost("products/add")]
    public async Task<IActionResult> AddProduct([FromBody] ProductDto product)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await repository.AddProductAsync(product);
            return CreatedAtAction(nameof(GetAllProducts), new { code = product.Code }, product);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // READY!
    // PUT /api/Inventory/products/{code}
    [HttpPut("products/{code}")]
    public async Task<IActionResult> UpdateProduct(string code, [FromBody] ProductDto updatedProduct)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var product = await repository.GetProductByCodeAsync(code);
        if (product == null)
            return NotFound(new { message = string.Format(InventoryMessages.ProductWithCodeNotFoundFormat, code) });

        product.Name = updatedProduct.Name;
        product.Category = updatedProduct.Category;
        product.Quantity = updatedProduct.Quantity;
        product.Price = updatedProduct.Price;

        await repository.UpdateProductAsync(product);
        return Ok(new { message = string.Format(InventoryMessages.ProductUpdatedSuccessfullyFormat, code), product });
    }


    // READY!
    // DELETE /api/products/{code}
    [HttpDelete("products/{code}")]
    public async Task<IActionResult> DeleteProduct(string code)
    {
        var product = await repository.GetProductByCodeAsync(code);
        if (product == null)
            return NotFound(new { message = string.Format(InventoryMessages.ProductWithCodeNotFoundFormat, code) });

        await repository.DeleteProductAsync(product.Code);
        return Ok(new { message = string.Format(InventoryMessages.ProductDeletedSuccessfullyFormat, code) });
    }
}