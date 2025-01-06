using Tokens;

namespace Master.Entity
{
    public class Staff
    {
        public BaseModel? BaseModel { get; set; }
        public Guid? UserId { get; set; }
        public Guid? st_id { get; set; }
        public string? st_staff_name { get; set; }
        public string? st_email { get; set; }
        public string? st_address { get; set; }
        public string? st_contact { get; set; }
        public string? st_dob { get; set; }
        public string? st_gender { get; set; }
        public string? st_salary { get; set; }
        public string? st_joining_date { get; set; }
        public Guid? st_com_id { get; set; }
        public string? st_state_id { get; set; }
        public string? st_city_id { get; set; }
        public string? st_country_id { get; set; }
        public string? st_designation_id { get; set; }
        public string? st_department_id { get; set; }

        public string? st_rolename { get; set; }
        public string? st_username { get; set; }

        public string? st_category { get; set; }
        public string? st_staff_code { get; set; }
        public string? st_bloodgroup { get; set; }
        public string? Server_Value { get; set; }
        public string? st_isactive { get; set; }
        public string? st_left_date { get; set; }
        public string? co_country_code { get; set; }
        public DateTime? st_updateddate { get; set; }
        public DateTime? st_createddate { get; set; }
       

    }
}
