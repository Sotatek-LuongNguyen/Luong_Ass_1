using OrderApi.Dto;
using OrderApi.Model;

namespace OrderApi.Service.ServiceProduct
{
    public interface IProductService
    {
        Task<string> CreateProductAsync(ProductDto productDto);
        Task<List<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductByIdAsync(int productId);
        Task<bool> UpdateProductAsync(ProductDto productDto);
        Task<bool> DeleteProductAsync(int productId);
        Task<List<ProductDto>> GetProductsByCategoryNameAsync(string categoryName);

    }
}
