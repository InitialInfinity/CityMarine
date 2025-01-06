using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel;
using Tokens;

namespace ibillcraft.Models
{
    public class EmployeeEmailMgmtModel
    {
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public Guid? e_id { get; set; }
        [DisplayName("Email")]
        public string? e_email { get; set; }
        [DisplayName("Employee")]
        public string? e_employee {  get; set; }
        [DisplayName("IsActive")]
        public string? e_isactive { get; set; }
        public DateTime? e_createddate { get; set; }
        public DateTime? e_updateddate { get; set; }
        public string? e_createdby { get; set; }
        public string? e_updatedby { get; set; }
        [DisplayName("Employee")]
        public string? e_staffname { get; set; }
        public string? e_emailid { get; set; }
    }
}
