using Tokens;

namespace Master.Entity
{
    public class SentClientModel
    {
        public BaseModel BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public string? sc_id { get; set; }
        public string? sc_count { get; set; }
        public string? sc_year { get; set; }
        public string? sc_from { get; set; }
        public string? sc_body { get; set; }
        public string? sc_to { get; set; }
        public string? sc_attachment { get; set; }
        public string? sc_sentdate { get; set; }
        public string? sc_subject { get; set; }
        public string? sc_type { get; set; }
        public string? clientid { get; set; }
        public string? sc_claimno { get; set; }
    }
}
