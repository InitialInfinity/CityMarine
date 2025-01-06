using System.ComponentModel;

namespace ibillcraft.Models
{
    public class EmailRuleConfg
    {
        public Guid? UserId { get; set; }
        public Guid? E_id { get; set; }
        [DisplayName("")]

        public string? E_parameter { get; set; }
        [DisplayName("")]

        public string? E_condition { get; set; }

        [DisplayName("")]

        public string? E_category { get; set; }
        [DisplayName("Value")]
        public string? E_value{ get; set; }


        [DisplayName("Parameter ")]

        public string? E_parameterName { get; set; }

        [DisplayName("Condition")]

        public string? E_conditionName { get; set; }

        [DisplayName("Category")]

        public string? E_categoryName { get; set; }
        [DisplayName("Status")]
        public string? E_isactive { get; set; }
        public DateTime? E_createddate { get; set; }
        public DateTime? E_updateddate { get; set; }

    }
   
}
