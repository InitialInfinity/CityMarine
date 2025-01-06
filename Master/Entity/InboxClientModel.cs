using Tokens;

namespace Master.Entity
{
    public class InboxClientModel
    {
        public BaseModel BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public string? ic_id { get; set; }
        public string? ic_count { get; set; }
        public string? ic_year { get; set; }
        public string? ic_from { get; set; }
        public string? ic_body { get; set; }
        public string? ic_to { get; set; }
        public string? ic_attachment { get; set; }
        public string? ic_receiveddate { get; set; }
        public string? ic_subject { get; set; }
        public string? ic_type { get; set; }
        public string? clientid { get; set; }
    }
}
