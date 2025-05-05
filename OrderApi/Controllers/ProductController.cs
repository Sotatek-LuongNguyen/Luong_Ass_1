using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Dto;
using OrderApi.Service.ServiceProduct;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateProduct([FromBody] ProductDto productDto)
        {
            var result = await _productService.CreateProductAsync(productDto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetAllProduct()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        // GET: api/product/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound($"Không tìm thấy sản phẩm với Id {id}");
            return Ok(product);
        }

        [HttpGet("by-category/{categoryName}")]
        public async Task<ActionResult<List<ProductDto>>> GetProductByCategoryName(string categoryName)
        {
            var products = await _productService.GetProductsByCategoryNameAsync(categoryName);
            return Ok(products);
        }


        [HttpPut]
        public async Task<ActionResult> Update([FromBody] ProductDto productDto)
        {
            var updated = await _productService.UpdateProductAsync(productDto);
            if (!updated)
                return NotFound($"Không tìm thấy sản phẩm với Id {productDto.IdProduct}");
            return Ok("Cập nhật sản phẩm thành công");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _productService.DeleteProductAsync(id);
            if (!deleted)
                return NotFound($"Không tìm thấy sản phẩm với Id {id}");
            return Ok("Xóa sản phẩm thành công");
        }
    }
}
