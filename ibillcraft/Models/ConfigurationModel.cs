namespace ibillcraft.Models
{
    public class ConfigurationModel
    {
        public Guid? UserId { get; set; }
        //Terms
        public Guid? tc_id { get; set; }
        public Guid? tc_com_id { get; set; }
        public string? tc_terms { get; set; }
        public string? tc_invoicename { get; set; }
        public string? tc_isactive { get; set; }
        //Note
        public Guid? in_id { get; set; }
        public Guid? in_com_id { get; set; }
        public string? in_note { get; set; }
        public string? in_invoicename { get; set; }
        public string? in_isactive { get; set; }
        //Series
        public Guid? s_id { get; set; }
        public Guid? s_com_id { get; set; }
        public string? s_series { get; set; }
        public string? s_invoicename { get; set; }
        public string? s_isactive { get; set; }
        public string? Server_Value { get; set; }
        public string? type { get; set; }
    }
}
