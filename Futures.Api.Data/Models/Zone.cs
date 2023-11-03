using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Futures.Api.Data.Models
{
    public partial class Zone
    {
        public Zone()
        {
            Restaurants = new HashSet<Restaurant>();
            Riders = new HashSet<Rider>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ZoneId { get; set; }
        public string? ZoneDescription { get; set; }

        public virtual ICollection<Restaurant> Restaurants { get; set; }
        public virtual ICollection<Rider> Riders { get; set; }
    }
}
