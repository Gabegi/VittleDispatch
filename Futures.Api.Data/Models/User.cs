using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Futures.Api.Data.Models
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUser { get; set; }
        public string FullName { get; set; } = null!;
        public byte[] CreatedAt { get; set; } = null!;
        public DateTime? LastUpdate { get; set; }
        public string UserAddress { get; set; } = null!;
        public bool IsOver18 { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
