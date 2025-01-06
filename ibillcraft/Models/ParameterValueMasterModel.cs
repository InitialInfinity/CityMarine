using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ibillcraft.Models
{
    public class ParameterValueMasterModel
    {
        public Guid? UserId { get; set; }
        public Guid? pv_id { get; set; }
        [DisplayName("Parameter Id")]
        public string? pv_parameterid { get; set; }
        [DisplayName("Name")]
        public string? pv_parametervalue { get; set; }
        [DisplayName("Code")]
        public string? pv_code { get; set; }
        [DisplayName("Parameter Name")]
        public string? pv_parametername { get; set; }
        [DisplayName("Status")]
        public string? pv_isactive { get; set; }
        public DateTime? pv_createddate { get; set; }
        public DateTime? pv_updateddate { get; set; }


    }
    public class RootObject3
    {
        public ParameterValueMasterModel? data { get; set; }
    }

}
