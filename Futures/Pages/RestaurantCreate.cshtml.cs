using Futures.ApiContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Futures.Services.ServiceInterfaces;

namespace Futures.Pages
{
    public class RestaurantCreateModel : PageModel
    {
        private IRestaurantService _services;

        public RestaurantCreateModel(IRestaurantService services)
        {
            _services = services;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

       
        public GetRestaurantResponseModel restaurant { get; set; }

        public async Task<IActionResult> OnPostAsync(GetRestaurantResponseModel restaurant)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _services.InsertNewAsync(restaurant);

            return RedirectToPage("./Restaurants");
        }
    }
}
