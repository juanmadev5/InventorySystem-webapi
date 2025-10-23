using InventorySystem_webapi.Data;
using InventorySystem_webapi.Data.model;
using InventorySystem_webapi.domain.@interface;
using InventorySystem_webapi.DTOs;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem_webapi.domain.repository;

public class ProductRepository(InventoryDbContext context) : IProductRepository
{
    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        return await context.Products
            .Select(p => new ProductDto
            {
                Code = p.Id,
                Name = p.Name,
                Category = p.Category,
                Quantity = p.Quantity,
                Price = p.Price
            })
            .ToListAsync();
    }

    public async Task<ProductDto?> GetProductByCodeAsync(string code)
    {
        var product = await context.Products
            .FirstOrDefaultAsync(p => p.Id == code);

        if (product == null) return null;

        return new ProductDto
        {
            Code = product.Id,
            Name = product.Name,
            Category = product.Category,
            Quantity = product.Quantity,
            Price = product.Price
        };
    }


    public async Task<List<ProductDto>> GetProductsByCategoryAsync(string category)
    {
        return await context.Products
            .Where(p => p.Category.ToLower() == category.ToLower())
            .Select(p => new ProductDto
            {
                Code = p.Id,
                Name = p.Name,
                Category = p.Category,
                Quantity = p.Quantity,
                Price = p.Price
            })
            .ToListAsync();
    }


    public async Task AddProductAsync(ProductDto productDto)
    {
        
        if (await context.Products.AnyAsync(p => p.Id == productDto.Code))
            throw new InvalidOperationException($"Product with code '{productDto.Code}' already exists.");
        
        var product = new Product
        {
            Id = productDto.Code,
            Name = productDto.Name,
            Category = productDto.Category,
            Quantity = productDto.Quantity,
            Price = productDto.Price
        };

        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
    }


    public async Task UpdateProductAsync(ProductDto productDto)
    {
        var product = await context.Products
            .FirstOrDefaultAsync(p => p.Id == productDto.Code);

        product.Name = productDto.Name;
        product.Category = productDto.Category;
        product.Quantity = productDto.Quantity;
        product.Price = productDto.Price;

        await context.SaveChangesAsync();
    }


    public async Task DeleteProductAsync(string code)
    {
        var product = await context.Products
            .FirstOrDefaultAsync(p => p.Id == code);

        context.Products.Remove(product);
        await context.SaveChangesAsync();
    }
}