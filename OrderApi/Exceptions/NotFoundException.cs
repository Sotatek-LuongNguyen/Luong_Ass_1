using Microsoft.EntityFrameworkCore.Infrastructure;

namespace OrderApi.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Sai dữ liệu") { }
        public NotFoundException(string message) : base(message) { }  
    }
    public class BadRequestException : Exception
    {
        public BadRequestException() : base("Yêu cầu không hợp lệ hoặc giá trị không được để trống.") { }
        public BadRequestException(string message) : base(message) { }
    }

    public class ModelvalidationException : Exception
    {
        public IEnumerable<ModelValidator> Errors { get; }

        public ModelvalidationException() : base("Dữ liệu không hợp lệ") { }

        public ModelvalidationException(string message) : base(message) { }

        public ModelvalidationException(IEnumerable<ModelValidator> errors)
            : base("Dữ liệu không hợp lệ")
        {
            Errors = errors;
        }

        public ModelvalidationException(string message, IEnumerable<ModelValidator> errors)
            : base(message)
        {
            Errors = errors;
        }
    }
}
