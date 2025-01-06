using System.ComponentModel;
using Tokens;

namespace ibillcraft.Models
{
    public class CityMasterModel
    {
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public Guid? ci_id { get; set; }
        [DisplayName("Country Name")]
        public string? ci_country_name { get; set; }

        [DisplayName("State Name")]
        public string? ci_state_name { get; set; }
        public string? ci_state_code { get; set; }
     
        public string? ci_country_id { get; set; }
        public string? ci_state_id { get; set; }
        [DisplayName("Code")]
        public string? ci_city_code { get; set; }
        [DisplayName("Name")]
        public string? ci_city_name { get; set; }
        [DisplayName("Status")]
        public string? ci_isactive { get; set; }
        public DateTime? ci_createddate { get; set; }
        public DateTime? ci_updateddate { get; set; }
    }
}
