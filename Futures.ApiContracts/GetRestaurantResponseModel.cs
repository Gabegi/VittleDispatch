using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Futures.ApiContracts
{
    public class GetRestaurantResponseModel
    {
        [JsonProperty("RestaurantId")]
        [Display(Name = "Restaurant Id")]
        public int RestaurantId { get; set; }

        [JsonProperty("RestaurantName")]
        [Display(Name = "Restaurant Name")]
        public string RestaurantName { get; set; } = null!;

        [JsonProperty("CategoryName")]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; } = null!;

        [JsonProperty("RestaurantAddress")]
        [Display(Name = "Restaurant Address")]
        public string RestaurantAddress { get; set; } = null!;

        [JsonProperty("ZoneId")]
        [Display(Name = "Zone Id")]
        public int? ZoneId { get; set; }

        public FoodCategoryResponseModelWithoutRestaurant? CategoryNameNavigation { get; set; }
    }
}
