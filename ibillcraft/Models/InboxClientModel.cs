namespace ibillcraft.Models
{
    public class InboxClientModel
    {
        public Guid? UserId { get; set; }
        public string? ic_id { get; set; }
        public string? ic_count { get; set; }
        public string? ic_year { get; set; }
        public string? ic_to { get; set; }
        public string? ic_attachment { get; set; }
        public string? icc_attachment { get; set; }
        public string? ic_receiveddate { get; set; }
        public string? ic_subject { get; set; }
        public string? ic_type { get; set; }
        public string? ic_from { get; set; }
        public string? ic_body { get; set; }
        public string? ic_fromemail { get; set; }
        public string? ic_email { get; set; }
        public string? clientid { get; set; }
        public string? ic_fromname { get; set; }
    }
}
