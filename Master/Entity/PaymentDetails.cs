using Tokens;

namespace Master.Entity
{
    public class PaymentDetails
    {
        public Guid? py_id { get; set; }
        public Guid? sub_id { get; set; }
        public Guid? UserId { get; set; }
        public BaseModel? BaseModel { get; set; }
        public Guid? com_id { get; set; }
        public string? com_company_name { get; set; }
        public string? com_email { get; set; }
        public string? Amount { get; set; }
        public string? Payment_Mode { get; set; }
        public string? Country { get; set; }
        public DateTime? com_updateddate { get; set; }
        public DateTime? com_createddate { get; set; }

    }
}
