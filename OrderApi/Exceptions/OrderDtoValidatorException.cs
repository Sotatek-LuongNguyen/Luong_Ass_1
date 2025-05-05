namespace OrderApi.Exceptions
{
    public class OrderDtoValidatorException : Exception
    {
        public IEnumerable<OrderDtoValidatorException> Errors { get; }

        public OrderDtoValidatorException(IEnumerable<OrderDtoValidatorException> errors)
            : base("Dữ liệu không hợp lệ")
        {
            Errors = errors;
        }

        public OrderDtoValidatorException(string message, IEnumerable<OrderDtoValidatorException> errors)
            : base(message)
        {
            Errors = errors;
        }

        public OrderDtoValidatorException(string? message) : base(message)
        {

        }
    }
}
