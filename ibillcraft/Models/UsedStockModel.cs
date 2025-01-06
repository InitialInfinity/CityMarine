namespace ibillcraft.Models
{
    public class UsedStockModel
    {
        public Guid? UserId { get; set; }
        public Guid? u_id { get; set; }
        public string? u_product_id { get; set; }
        public string? u_product_name { get; set; }
        public DateTime? u_date { get; set; }
        public string? u_sqrft { get; set; }
        public string? u_quanity { get; set; }
        public string? Server_Value { get; set; }
        public string? u_isactive { get; set; }
        public string? p_id { get; set; }
        public string? p_name { get; set; }
        public string? p_sgst { get; set; }
        public string? p_cgst { get; set; }
        public string? p_igst { get; set; }
        public string? p_ugst { get; set; }
        public string? p_stock { get; set; }
        public string? p_hsn_code { get; set; }
        public string? p_rate { get; set; }
        public string? p_unit { get; set; }
        public string? p_desc { get; set; }
        public DateTime? u_updateddate { get; set; }
        public string? u_com_id { get; set; }
        public DateTime? u_createddate { get; set; }
    }
}
