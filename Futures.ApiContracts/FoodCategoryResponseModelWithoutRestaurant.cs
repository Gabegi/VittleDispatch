using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Futures.ApiContracts
{
    public class FoodCategoryResponseModelWithoutRestaurant
    {
        [JsonProperty("CategoryName")]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; } = null!;

        [JsonProperty("CategoryDescription")]
        [Display(Name = "Category Description")]
        public string? CategoryDescription { get; set; }
    }
}
