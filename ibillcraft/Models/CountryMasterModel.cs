using System.ComponentModel;


namespace ibillcraft.Models
{
    public class CountryMasterModel
    {
        public Guid? UserId { get; set; }
        public Guid? co_id { get; set; }
        [DisplayName("Country Name")]
        public string? co_country_name { get; set; }
        [DisplayName("Country Code")]
        public string? co_country_code { get; set; }
        [DisplayName("Currency Name")]
        public string? co_currency_name { get; set; }
        [DisplayName("Currency Id")]
        public string? co_currency_id { get; set; }
        [DisplayName("Country Timezone")]
        public string? co_timezone { get; set; }
        [DisplayName("Status")]
        public string? co_isactive { get; set; }
        public DateTime? co_createddate { get; set; }
        public DateTime? co_updateddate { get; set; }
    }
    public class RootObject
    {
        public CountryMasterModel? data { get; set; }
    }
}