using System.ComponentModel;

namespace ibillcraft.Models
{
    public class ServerAllocationModel
    {
        public Guid? ser_id { get; set; }
        public Guid? com_id { get; set; }
        public Guid? UserId { get; set; }
        public string? sub_id { get; set; }
        public string? py_id { get; set; }

        [DisplayName("Company Name")]
        public string? com_company_name { get; set; }
        [DisplayName("Email")]
        public string? com_email { get; set; }
        [DisplayName("Contact No")]
        public string? com_contact { get; set; }
        [DisplayName("Country")]
        public string? CountryId { get; set; }
        [DisplayName("Staff No")]
        public string? com_staff_no { get; set; }
        [DisplayName("Package Name")]
        public string? package_name { get; set; }
        [DisplayName("Start Date")]
        public string? StartDate { get; set; }
        [DisplayName("End Date")]
        public string? EndDate { get; set; }
        [DisplayName("Package Amount")]
        public string? final_Amount { get; set; }
        [DisplayName("Payment Mode")]
        public string? Payment_Mode { get; set; }      
        public string? Server_Key { get; set; }
        public string? Server_Value { get; set; }
        public string? result { get; set; }
        public string? Allotted_Date { get; set; }
        public string? com_code { get; set; }
        public string? Country { get; set; }

        public DateTime? ser_updateddate { get; set; }
        public DateTime? ser_createddate { get; set; }
    }
}
