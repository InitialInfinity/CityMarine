using System.ComponentModel;
namespace ibillcraft.Models
{
    public class StaffModel
    {
        public Guid? UserId { get; set; }
        public Guid? st_id { get; set; }
        [DisplayName("Staff Name")]
        public string? st_staff_name { get; set; }
        [DisplayName("Email")]
        public string? st_email { get; set; }
        [DisplayName("Address")]
        public string? st_address { get; set; }
        [DisplayName("Contact No.")]
        public string? st_contact { get; set; }
        [DisplayName("DOB")]
        public string? st_dob { get; set; }
        [DisplayName("Gender")]
        public string? st_gender { get; set; }
        [DisplayName("Salary")]
        public string? st_salary { get; set; }
        [DisplayName("Date Of Joining")]
        public string? st_joining_date { get; set; }
        public Guid? st_com_id { get; set; }
        public string? st_state_id { get; set; }
        public string? pv_parametervalue { get; set; }
        public string? st_city_id { get; set; }
        [DisplayName("Country")]
        public string? st_country_id { get; set; }
        [DisplayName("Designation")]
        public string? st_designation_id { get; set; }
        [DisplayName("Department Name")]

        public string? st_rolename { get; set; }

        public string? st_category { get; set; }
        public string? st_department_id { get; set; }
        [DisplayName("BloodGroup")]
        public string? st_bloodgroup { get; set; }
        [DisplayName("Status")]
        public string? st_isactive { get; set; }
        [DisplayName("Left Date")]

        public string? st_username { get; set; }
        public string? st_staff_code { get; set; }
        public string? st_left_date { get; set; }
        public string? Server_Value { get; set; }
        public string? co_country_code { get; set; }
       
        public DateTime? st_updateddate { get; set; }
        public DateTime? st_createddate { get; set; }
        public string? st_password { get; set; }

    }
    public class Staff
    {
        public StaffModel? data { get; set; }
    }
}
