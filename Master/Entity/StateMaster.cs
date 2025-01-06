using Tokens;

namespace Master.API.Entity
{
    public class StateMaster
    {
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public Guid? s_id { get; set; }

        public string? s_country_name { get; set; }
        public string? s_country_id { get; set; }
        public string? s_state_name { get; set; }
        public string? s_state_code { get; set; }
        public string? s_isactive { get; set; }
        public string? s_updatedby { get; set; }
    }
}
