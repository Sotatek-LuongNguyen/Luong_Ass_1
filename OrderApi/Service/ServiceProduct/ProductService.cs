using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderApi.Dto;
using OrderApi.Exceptions;
using OrderApi.Model;

namespace OrderApi.Service.ServiceProduct
{
    public class ProductService : IProductService
    {
        private readonly OrderDbContext _context;
        public ProductService(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreateProductAsync(ProductDto productDto)
        {
            var role = await _context.Categories.FindAsync(productDto.IdCategory);
            if (role == null)
            {
                return $"Lỗi: Category với Id {productDto.IdCategory} không tồn tại!";
            }
            var product = new Product
            {
                ProductName = productDto.ProductName,
                Price = productDto.Price,
                Description = productDto.Description,
                Image = productDto.Image,
                Quantity = productDto.Quantity,
                Created = productDto.Created,
                ImageUrl = productDto.ImageUrl,
                Status = productDto.Status,
                IdCategory = productDto.IdCategory,
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return "Thêm người dùng thành công!";
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(u => u.Category)
                .Select(u => new ProductDto
                {
                    ProductName = u.ProductName,
                    Description = u.Description,
                    Price = u.Price,
                    Status = u.Status,
                    Image = u.Image,
                    Quantity = u.Quantity,
                    Created = u.Created,
                    IdCategory = u.IdCategory,
                    CategoryName = u.Category.CategoryName,
                })
                .ToListAsync();
        }

        public async Task<ProductDto> GetProductByIdAsync(int productId)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.IdProduct == productId);

            if (product == null)
                return null;

            return new ProductDto
            {
                IdProduct = product.IdProduct,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                Status = product.Status,
                Image = product.Image,
                Quantity = product.Quantity,
                Created = product.Created,
                ImageUrl = product.ImageUrl,
                IdCategory = product.IdCategory,
                CategoryName = product.Category.CategoryName
            };
        }

        public async Task<bool> UpdateProductAsync(ProductDto productDto)
        {
            var product = await _context.Products.FindAsync(productDto.IdProduct);
            if (product == null)
                return false;

            product.ProductName = productDto.ProductName;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.Status = productDto.Status;
            product.Image = productDto.Image;
            product.Quantity = productDto.Quantity;
            product.Created = productDto.Created;
            product.ImageUrl = productDto.ImageUrl;
            product.IdCategory = productDto.IdCategory;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<ProductDto>> GetProductsByCategoryNameAsync(string categoryName)
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Category.CategoryName.ToLower().Contains(categoryName.ToLower()))
                .Select(p => new ProductDto
                {
                    IdProduct = p.IdProduct,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    Status = p.Status,
                    Image = p.Image,
                    Quantity = p.Quantity,
                    Created = p.Created,
                    ImageUrl = p.ImageUrl,
                    IdCategory = p.IdCategory,
                    CategoryName = p.Category.CategoryName
                })
                .ToListAsync();

            return products;
        }

    }
}
