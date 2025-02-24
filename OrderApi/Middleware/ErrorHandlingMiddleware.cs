using System.Net;
using System.Text.Json;

namespace OrderApi.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Chuyển tiếp xử lý request cho middleware/controller phía sau
                await _next(context);
            }
            catch (Exception ex)
            {
                // Middleware bắt lỗi và xử lý
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Xác định mã lỗi và thông báo dựa trên loại exception
            HttpStatusCode status;
            string message;

            switch (exception)
            {
                case ArgumentNullException _:
                    status = HttpStatusCode.BadRequest;
                    message = "Giá trị không được để trống.";
                    break;
                case InvalidOperationException _:
                    status = HttpStatusCode.BadRequest;
                    message = "Yêu cầu không hợp lệ.";
                    break;
                default:
                    status = HttpStatusCode.InternalServerError;
                    message = "Đã xảy ra lỗi không xác định.";
                    break;
            }

            // Ghi log lỗi với ILogger
            _logger.LogError(exception, "Đã xảy ra lỗi: {Message}", exception.Message);

            // Tạo phản hồi JSON lỗi
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            var errorResponse = new { error = message, detail = exception.Message };

            return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    } 
}
