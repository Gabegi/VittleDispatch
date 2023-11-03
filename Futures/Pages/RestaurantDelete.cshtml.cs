using Futures.ApiContracts;
using Futures.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Futures.Pages
{
    public class RestaurantDeleteModel : PageModel
    {
        [BindProperty]
        public GetRestaurantResponseModel restaurant { get; set; }

        private IRestaurantService _services;

        public RestaurantDeleteModel(IRestaurantService services)
        {
            _services = services;
        }

        [BindProperty(SupportsGet = true)]
        public int id { get; set; }
        public async Task<IActionResult> OnGet()
        {

           restaurant = await _services.GetbyIdAsync(id);

            if (id == 0)
            {
                return NotFound();
            }

            return Page();
        }



        public async Task<IActionResult> OnDeletetAsync(int RestaurantId)
        {
            

            _services.DeleteByIdAsync(RestaurantId);

            return RedirectToPage("./Restaurants");
        }
    }
}
