using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Futures.Api.Data.Models
{
    public partial class Dish
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DishId { get; set; }

        public string DishName { get; set; } = null!;
        public string? DishDescription { get; set; }
        public int? RestaurantId { get; set; }
        public decimal? Price { get; set; }
        public bool Require18 { get; set; }

        public virtual Restaurant Restaurant { get; set; }
    }
}
