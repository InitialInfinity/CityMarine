using Tokens;

namespace Master.Entity
{
    public class ParameterMaster
    { 
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public Guid? p_id { get; set; }
        public string? p_parametername { get; set; }
        public string? p_code { get; set; }
        public string? p_isactive { get; set; }
        public DateTime? p_createddate { get; set; }
        public DateTime? p_updateddate { get; set; }

    }
}
