using Microsoft.EntityFrameworkCore.Infrastructure;

namespace OrderApi.Exceptions
{
    public class ModelvalidationException : Exception
    {
        public IEnumerable<ModelValidator> Errors { get; }

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

        public ModelvalidationException(string? message) : base(message)
        {

        }
    }
}
