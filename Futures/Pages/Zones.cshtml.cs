using Futures.ApiContracts;
using Futures.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Futures.Pages
{
    public class ZonesModel : PageModel
    {
        [BindProperty]
        public IEnumerable<GetZoneResponseModel> Zones { get; set; }


        [BindProperty]
        public GetZoneResponseModel Zone { get; set; }

        private IZoneService _services;

        public ZonesModel(IZoneService services)
        {
            _services = services;
        }

        public async Task<ActionResult>  OnGet()
        {
            Zones = await _services.GetAllAsync();

            return Page();
        }
    }
}
