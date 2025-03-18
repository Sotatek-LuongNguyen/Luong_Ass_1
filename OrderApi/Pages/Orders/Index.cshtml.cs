using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderApi.Model;

namespace OrderApi.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly OrderApi.Data.OrderDbContext _context;

        public IndexModel(OrderApi.Data.OrderDbContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Order = await _context.Orders.ToListAsync();
        }
    }
}
