using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Futures.Api.Data.Models
{
    public partial class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public int? IdUser { get; set; }
        public int? RiderId { get; set; }
        public string OrderStatus { get; set; } = null!;
        public byte[] CreatedAt { get; set; } = null!;

        public virtual User? IdUserNavigation { get; set; }
        public virtual Rider? Rider { get; set; }
    }
}
