using System.ComponentModel;
using Tokens;

namespace Master.Entity
{
    public class EmailList
    {
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public Guid? E_id { get; set; }
        [DisplayName("Email Id")]

        public string? E_email { get; set; }
        [DisplayName("Password")]

        public string? E_password { get; set; }
        [DisplayName("SMTP Host")]
        public string? E_smtph { get; set; }

        [DisplayName("SMTP Port")]
        public string? E_smtpp { get; set; }

        [DisplayName("IMAP Host")]
        public string? E_imaph { get; set; }

        [DisplayName("IMAP Port")]
        public string? E_imapp { get; set; }

        [DisplayName("POP3 Host")]
        public string? E_poph { get; set; }

        [DisplayName("POP3 Port")]
        public string? E_popp { get; set; }


        [DisplayName("Key")]
        public string? E_key { get; set; }
        public string? E_isactive { get; set; }
        public string? E_issslEnable { get; set; }
        public DateTime? E_createddate { get; set; }
        public DateTime? E_updateddate { get; set; }
    }
}
