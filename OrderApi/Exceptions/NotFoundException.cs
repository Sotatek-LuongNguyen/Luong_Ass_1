using Microsoft.EntityFrameworkCore.Infrastructure;

namespace OrderApi.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Sai dữ liệu") { }
        public NotFoundException(string message) : base(message) { }  
    }
}
