using Tokens;

namespace Master.API.Entity
{
    public class ParameterValueMaster
    { 
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public Guid? pv_id { get; set; }
        public string? pv_parameterid { get; set; }
        public string? pv_parametervalue { get; set; }
        public string? pv_code { get; set; }
        public string? pv_parametername { get; set; }
        public string? pv_isactive { get; set; }
        public DateTime? pv_createddate { get; set; }
        public DateTime? pv_updateddate { get; set; }


    }
}
