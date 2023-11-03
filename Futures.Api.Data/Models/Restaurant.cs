using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Futures.Api.Data.Models
{
    public partial class Restaurant
    {
        public Restaurant()
        {
            Dishes = new HashSet<Dish>();
        }

        [Display(Name = "Restaurant Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RestaurantId { get; set; }

        [Display(Name = "Restaurant Name")]
        public string RestaurantName { get; set; } = null!;

        [Display(Name = "Category Name")]
        public string CategoryName { get; set; } = null!;

        [Display(Name = "Restaurant Address")]
        public string RestaurantAddress { get; set; } = null!;

        [Display(Name = "Zone Id")]
        public int? ZoneId { get; set; }

        public virtual FoodCategory? CategoryNameNavigation { get; set; } = default!; 
        public virtual Zone? Zone { get; set; } = default!;
        public virtual ICollection<Dish> Dishes { get; set; } = default!;
        public virtual Dish? Dish { get; set; } = default!;


    }
}
