using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderApi.Dto;
using OrderApi.Exceptions;
using OrderApi.Model;

namespace OrderApi.Service.ServiceCategory
{
    public class CategoryService : ICategoryService
    {
        private readonly OrderDbContext _context;

        public CategoryService(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreateCategoryAsync(CategoryDto categoryDto)
        {
            var entity = new Category
            {
                CategoryName = categoryDto.CategoryName,
                Status = categoryDto.Status
            };

            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
            return "Thêm danh mục thành công";
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                Status = c.Status
            }).ToList();
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
            {
                throw new NotFoundException("Không tìm thấy danh mục");
            }

            return new CategoryDto
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                Status = category.Status
            };
        }

        public async Task<bool> UpdateCategoryAsync(CategoryDto categoryDto)
        {
            var existing = await _context.Categories.FindAsync(categoryDto.Id);
            if (existing == null)
            {
                throw new NotFoundException("Không tìm thấy danh mục");
            }

            existing.CategoryName = categoryDto.CategoryName;
            existing.Status = categoryDto.Status;

            _context.Categories.Update(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
            {
                throw new NotFoundException("Không tìm thấy danh mục");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
