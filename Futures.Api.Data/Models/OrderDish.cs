using System;
using System.Collections.Generic;

namespace Futures.Api.Data.Models
{
    public partial class OrderDish
    {
        public int OrderId { get; set; }
        public int? DishId { get; set; }
        public int? Quantity { get; set; }

        public virtual Dish? Dish { get; set; }
        public virtual Order Order { get; set; } = null!;
    }
}
