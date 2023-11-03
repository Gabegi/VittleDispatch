namespace Futures.Api.Data.Models
{
    public partial class FoodCategory
    {
        public FoodCategory()
        {
            Restaurants = new HashSet<Restaurant>();
        }

        public string? CategoryName { get; set; }/* = null!;*/
        public string? CategoryDescription { get; set; }

        public virtual ICollection<Restaurant> Restaurants { get; set; } = default!;
    }
}
