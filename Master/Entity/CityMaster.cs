using Tokens;

namespace Master.API.Entity
{
    public class CityMaster
    {
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public Guid? ci_id { get; set; }
        public string? ci_country_name { get; set; }
        public string? ci_state_name { get; set; }
        public string? ci_country_id { get; set; }
        public string? ci_state_id { get; set; }
        public string? ci_city_name { get; set; }
        public string? ci_city_code { get; set; }
        public string? ci_isactive { get; set; }
        public DateTime? ci_createddate { get; set; }
        public DateTime? ci_updateddate { get; set; }
    }

}
