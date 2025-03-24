using Microsoft.EntityFrameworkCore.Infrastructure;

namespace OrderApi.Exceptions
{

    public class BadRequestException : Exception
    {
        public BadRequestException() : base("Yêu cầu không hợp lệ hoặc giá trị không được để trống.") { }
        public BadRequestException(string message) : base(message) { }
    } 
}
