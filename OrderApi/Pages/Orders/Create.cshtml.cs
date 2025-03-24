using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderApi.Data;
using OrderApi.Model;

namespace OrderApi.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly OrderApi.Data.OrderDbContext _context;
        private readonly OrderApi.Service.IOrderService _orderService;
        private readonly Microsoft.Extensions.Logging.ILogger<CreateModel> _logger;

        public CreateModel(OrderApi.Data.OrderDbContext context, Service.IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Order Order { get; set; } = default!;

        
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page(); // Quay lại trang Create với thông báo lỗi
                }

                var isCreated = await _orderService.CreateOrderAsync(Order);

                //if (!isCreated)
                //{
                //    ModelState.AddModelError("", "Không thể tạo đơn hàng.");
                //    return Page();
                //}

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo đơn hàng.");
                ModelState.AddModelError("", "Đã xảy ra lỗi, vui lòng thử lại.");
                return Page();
            }
        }

    }
}
