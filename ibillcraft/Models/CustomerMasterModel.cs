using System.ComponentModel;
using Tokens;

namespace ibillcraft.Models
{
    public class CustomerMasterModel
    {

        public Guid? UserId { get; set; }
        public string? Server_Value { get; set; }
        public string? c_id { get; set; }
        [DisplayName("Customer Name")]

        public string? c_name { get; set; }
        [DisplayName("Code")]
        public string? c_ccode { get; set; }
        [DisplayName("Customer Address")]
        public string? c_address { get; set; }
        [DisplayName("Customer Contact No")]

        public string? c_contact { get; set; }
        [DisplayName("Customer Alternate Contact No")]
        public string? c_contact2 { get; set; }
     
        [DisplayName("Customer Email")]
        public string? c_email { get; set; }
        [DisplayName("DOB")]
        public string? c_dob { get; set; }
        [DisplayName("Anniversary Date")]
        public string? c_anidate { get; set; }
       
        [DisplayName("Customer Note")]
        public string? c_note { get; set; }
      
        [DisplayName("Status")]
        public string? c_isactive { get; set; }
        public string? c_type { get; set; }
     
        public string? cd_id { get; set; }
        public string? c_domain {  get; set; }



        public DateTime? c_updateddate { get; set; }
        public DateTime? c_createddate { get; set; }

        public string? c_com_id { get; set; }
        public string? co_country_code { get; set; }

    
        public List<CustomerAttachment>? CustomerAttachment { get; set; }
    }
    public class RootObjectC
    {
        public CustomerMasterModel? data { get; set; }
    }

    public class CustomerAttachment
    {
        public string? c_type_id { get; set; }
        public string? name { get; set; }
        public string? c_type { get; set; }
       

        public string? attachment { get; set; }//

        public string? mdate { get; set; }
        public string? idate { get; set; }//

        public string? vdate { get; set; }//
       

       

       

       

    }
}
