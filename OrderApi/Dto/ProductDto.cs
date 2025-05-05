namespace OrderApi.Dto
{
    public class ProductDto
    {
        public int IdProduct { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string? Status { get; set; }
        public string? Image { get; set; }
        public int Quantity { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public string? ImageUrl { get; set; }
        public int IdCategory { get; set; }
        public string CategoryName { get; set; }
    }
}
