using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ibillcraft.Models
{
    public class ParameterMasterModel
    {
        public Guid? UserId { get; set; }
        public Guid? p_id { get; set; }
        [DisplayName("Name")]

        public string? p_parametername { get; set; }
        [DisplayName("Code")]

        public string? p_code { get; set; }
        [DisplayName("Status")]
        public string? p_isactive { get; set; }
        public DateTime? p_createddate { get; set; }
        public DateTime? p_updateddate { get; set; }

    }
    public class RootObject2
    {
        public ParameterMasterModel? data { get; set; }
    }

}
