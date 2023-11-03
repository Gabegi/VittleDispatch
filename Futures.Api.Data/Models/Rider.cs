using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Futures.Api.Data.Models
{
    public partial class Rider
    {
        public Rider()
        {
            Orders = new HashSet<Order>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RiderId { get; set; }
        public string RiderName { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string RiderAvailability { get; set; } = null!;
        public int? ZoneId { get; set; }

        public virtual Zone? Zone { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
