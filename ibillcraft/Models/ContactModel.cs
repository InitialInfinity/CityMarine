using System.ComponentModel;

namespace ibillcraft.Models
{
    public class ContactModel
    {
		public Guid? UserId { get; set; }
		public Guid? c_id { get; set; }
		
		public string? c_name { get; set; }
		
		public string? c_lname { get; set; }
		
		public string? c_email { get; set; }
		
		public string? c_subject { get; set; }
		public string? c_message { get; set; }
		
		
		//public Guid? c_com_id { get; set; }
		
		
		public string? Server_Value { get; set; }
		public DateTime? c_updateddate { get; set; }
		public DateTime? c_createddate { get; set; }
	}
}
