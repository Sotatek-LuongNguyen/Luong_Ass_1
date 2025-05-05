using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Dto;
using OrderApi.Model;
using OrderApi.Service.ServiceCategory;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] CategoryDto categoryDto)
        {
            var result = await _categoryService.CreateCategoryAsync(categoryDto);
            _logger.LogInformation("Danh mục mới đã được thêm: {@Category}", categoryDto);
            return Ok(result);
        }


        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> GetAll()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            return Ok(category);
        }


        [HttpPut]
        public async Task<ActionResult<string>> Update([FromBody] CategoryDto categoryDto)
        {
            var updated = await _categoryService.UpdateCategoryAsync(categoryDto);
            _logger.LogInformation("Danh mục đã được cập nhật: {@Category}", categoryDto);
            return Ok("Cập nhật danh mục thành công");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> Delete(int id)
        {
            var deleted = await _categoryService.DeleteCategoryAsync(id);

            if (!deleted)
            {
                return NotFound($"Không tìm thấy danh mục với ID: {id}");
            }

            _logger.LogWarning("Danh mục đã bị xóa với ID: {CategoryId}", id);
            return Ok("Xóa danh mục thành công");
        }
    }
}
