using Futures.ApiContracts;
using Futures.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Futures.Pages
{
    public class RestaurantsModel : PageModel
    {

        [BindProperty]  
        GetRestaurantResponseModel restaurant { get; set; }

        [BindProperty]
        public IEnumerable<GetRestaurantResponseModel> RestaurantList { get; set; }

        private IRestaurantService _services;

        public RestaurantsModel(IRestaurantService services)
        {
            _services = services;
        }


        public async Task<IActionResult> OnGet()
        {
            RestaurantList = await _services.GetAllAsync(); 
            return Page();
        }

       

        [BindProperty]
        public int restaurant_id { get; set; }
        public IActionResult OnPostId()
        {
            if (restaurant_id != 0) // checking that the restaurant exists
            {

                // Use the service method that makes an API call to the Futures_API
                restaurant = _services.GetbyIdAsync(restaurant_id).Result;

                RestaurantList = RestaurantList.Append(restaurant);

                return Page();
            }
            if (restaurant_id == 0)
            {
                return NotFound();
            }

                return Page();

            // return RedirectToPage("Restaurants/" + restaurant.RestaurantId); // add something there
        }

        [BindProperty]
        public string restaurant_name { get; set; }
        public IActionResult OnPostName()
        {
            if (restaurant_name != null) // checking that the restaurant exists
            {

                // Use the service method that makes an API call to the Futures_API
                restaurant = _services.GetbyNameAsync(restaurant_name).Result;

                RestaurantList =  RestaurantList.Append(restaurant);

                return Page();
            }


            if (restaurant_name is null) //if name = null, then it's a new entry (for INSERT Method)
            {
                return NotFound();
            }

            return Page();
        }
    }
}

