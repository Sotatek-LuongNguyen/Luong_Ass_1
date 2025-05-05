using OrderApi.Dto;

namespace OrderApi.Service.ServiceCategory
{
    public interface ICategoryService
    {
        Task<string> CreateCategoryAsync(CategoryDto categoryDto);
        Task<List<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> GetCategoryByIdAsync(int categoryId);
        Task<bool> UpdateCategoryAsync(CategoryDto categoryDto);
        Task<bool> DeleteCategoryAsync(int categoryId);
    }
}
