using Futures.ApiContracts;
using Futures.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Futures.Pages
{
    public class RestaurantEditModel : PageModel
    {
        [BindProperty]
        public GetRestaurantResponseModel restaurant { get; set; }

        private IRestaurantService _services;

        public RestaurantEditModel(IRestaurantService services)
        {
            _services = services;
        }

        [BindProperty(SupportsGet =true)]
        public int id { get; set; }
        public async Task<IActionResult> OnGet()
        {

            if (id == 0)
            {
                return NotFound();
            }
           var result = await _services.GetbyIdAsync(id);

            restaurant = result;


            return Page();
        }

        public async Task<IActionResult> OnPostAsync(GetRestaurantResponseModel restaurant)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

             _services.UpdateAsync(restaurant);

            return Page();
            // return RedirectToPage("./Restaurants");
        }
    }
}
