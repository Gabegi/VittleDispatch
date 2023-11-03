using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Futures.ApiContracts
{
    public class GetZoneResponseModel
    {
        [JsonProperty("zone_id")]
        [Display(Name = "Zone Id")]
        public int zone_id { get; set; } 

        [JsonProperty("zone_description")]
        [Display(Name = "Zone Description")]
        public string zone_description { get; set; } = null!;
    }
}
