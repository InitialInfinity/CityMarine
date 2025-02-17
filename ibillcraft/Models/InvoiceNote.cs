using Tokens;

namespace ibillcraft.Models
{
    public class InvoiceNote
    {
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        //Expense
        public Guid? In_id { get; set; }
        public string? In_com_id { get; set; }
        public string? In_note { get; set; }
        public string? In_invoicename { get; set; }
        public string? In_isactive { get; set; }
        public string? tc_terms { get; set; }
        public string? con_sms { get; set; }
        public string? con_email { get; set; }
        public string? con_wp { get; set; }
        public string? df_format { get; set; }
    }
}
