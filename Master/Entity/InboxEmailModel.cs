using Tokens;

namespace Master.Entity
{
    public class InboxEmailModel
    {
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public string? i_id { get; set; }
        public string? i_from { get; set; }
        public string? i_to { get; set; }
        public string? i_subject { get; set; }
        public string? i_body { get; set; }
        public string? i_receiveddate { get; set; }
        public string? i_messageid { get; set; }
        public string? i_replyto { get; set; }
        public string? i_attachment { get; set; }
        public string? i_type { get; set; }
        public string? i_generaltype {  get; set; }
        public string? i_claimno {  get; set; }
    }
}
