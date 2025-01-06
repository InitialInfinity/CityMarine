using Tokens;

namespace Master.Entity
{
    public class EmployeeEmailMgmtModel
    {
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public Guid? e_id { get; set; }
        public string? e_email { get; set; }
        public string? e_employee { get; set; }
        public string? e_isactive { get; set; }
        public DateTime? e_createddate { get; set; }
        public DateTime? e_updateddate { get; set; }
        public string? e_createdby { get; set; }
        public string? e_updatedby { get; set; }
    }
}
