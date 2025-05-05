namespace OrderApi.Dto
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public string? Status { get; set; }
        public int IdUser { get; set; }
        public int IdEmployee { get; set; }
        public int IdProduct { get; set; }
    }
}
