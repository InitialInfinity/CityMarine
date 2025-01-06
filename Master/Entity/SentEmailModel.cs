using Tokens;

namespace Master.Entity
{
    public class SentEmailModel
    {
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public string? s_id { get; set; }
        public string? s_from { get; set; }
        public string? s_to { get; set; }
        public string? s_subject { get; set; }
        public string? s_body { get; set; }
        public string? s_sentdate { get; set; }
        public string? s_messageid { get; set; }
        public string? s_replyto { get; set; }
        public string? s_attachment { get; set; }
        public string? s_type { get; set; }
    }
}
