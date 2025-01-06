
using Tokens;

namespace Master.API.Entity
{
    public class CountryMaster
    {
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public Guid? co_id { get; set; }

        public string? co_country_name { get; set; }
        public string? co_country_code { get; set; }
        public string? co_currency_name { get; set; }
        public string? co_currency_id { get; set; }
        public string? co_timezone { get; set; }
        public string? co_isactive { get; set; }
        public DateTime? co_createddate { get; set; }
        public DateTime? co_updateddate { get; set; }
    }
}
