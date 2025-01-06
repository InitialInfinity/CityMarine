using Tokens;

namespace ibillcraft.Models
{
    public class Dashboard
    {
        public BaseModel? BaseModel { get; set; }
        public string? RoleId { get; set; }
        public string? ParentId { get; set; }
        public string? m_api { get; set; }
        public string? m_menuname { get; set; }
        public string? m_id { get; set; }
        public string? m_action { get; set; }
        public string? m_controller { get; set; }
        public string? m_icon { get; set; }
        public string? Id { get; set; }

        public string? monthlysale { get; set; }
        public string? monthlyrollpurchase { get; set; }
        public string? monthlyexpense { get; set; }
        public string? monthlyprofit { get; set; }

        public string? monthlyquotation { get; set; }
        public string? monthlycashorder { get; set; }


        public string? dailysale { get; set; }
        public string? dailyadvance { get; set; }
        public string? dailyexpense { get; set; }
        public string? dailybalance { get; set; }
        public string? com_id { get; set; }

        public string? totalamount { get; set; }
        public string? totaladvance { get; set; }
        public string? totalpurchase { get; set; }
        public string? totalexpense { get; set; }
        public string? totalprofit { get; set; }
      
    }
    public class DashboardC
    {
        public Dashboard? data { get; set; }
		public List<Dashboard>? list5 { get; set; }
		
		public List<Listvalues>? list1 { get; set; }
		public List<Listvalues>? list2 { get; set; }
		public List<Listvalues>? list3 { get; set; }
		public List<Listvalues>? list4 { get; set; }
	}
	public class Listvalues
	{
		public string? Month { get; set; }
		public string? DailyOrderTotal { get; set; }
		public string? ExpenseTotal { get; set; }
		public string? AllTotal { get; set; }
		public string? SaleTotal { get; set; }
		public string? TotalSalePay { get; set; }
	}
}
