using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Futures.Api.Data.Models;

namespace Futures.Pages
{
    public class OrdersModel : PageModel
    {
        // fetch the class order
        [BindProperty]
        public Order order { get; set; }

        // when page loading display all orders
        public IActionResult OnGet()
        {

            return Page();
        }
    }
}
