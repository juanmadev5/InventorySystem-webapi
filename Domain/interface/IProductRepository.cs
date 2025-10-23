using InventorySystem_webapi.DTOs;

namespace InventorySystem_webapi.domain.@interface
{
    public interface IProductRepository
    {
        Task<List<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByCodeAsync(string code);
        Task<List<ProductDto>> GetProductsByCategoryAsync(string category);
        Task AddProductAsync(ProductDto product);
        Task UpdateProductAsync(ProductDto product);
        Task DeleteProductAsync(string code);
    }
}