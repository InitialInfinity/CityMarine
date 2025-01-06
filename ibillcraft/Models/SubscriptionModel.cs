namespace ibillcraft.Models
{
    public class SubscriptionModel
    {
        public Guid? sub_id { get; set; }
        public Guid? sm_id { get; set; }
         public Guid? UserId { get; set; }
        public Guid? com_id { get; set; }
        public string? com_company_name { get; set; }
        
        public string? subAmount { get; set; }
        public string? package_name { get; set; }
   

        public string? payment_method { get; set; }
        public string? sm_service { get; set; }
        public string? subDiscount { get; set; }
        public string? final_Amount { get; set; }
        public string? Invoice { get; set; }
        public string? Quotation { get; set; }
        public string? Expence { get; set; }
        public string? cash_order { get; set; }
        public string? sub_duration { get; set; }
        public string? com_isactive { get; set; }
        public string? EndDate { get; set; }
        public string? StartDate { get; set; }
        public DateTime? com_updateddate { get; set; }
        public DateTime? com_createddate { get; set; }

    }
}