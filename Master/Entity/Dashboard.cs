using Tokens;

namespace Master.Entity
{
    public class Dashboard
    {
        public string? monthlysale { get; set; }
        public string? monthlyrollpurchase { get; set; }
        public string? monthlyexpense { get; set; }
        public string? monthlyprofit { get; set; }

        public string? dailysale { get; set; }
        public string? dailyadvance { get; set; }
        public string? dailyexpense { get; set; }
        public string? dailybalance { get; set; }
        public string? com_id { get; set; }
        public BaseModel? BaseModel { get; set; }


        public string? monthlyquotation { get; set; }
        public string? monthlycashorder { get; set; }

        public string? totalamount { get; set; }
        public string? totaladvance { get; set; }
        public string? totalpurchase { get; set; }
        public string? totalexpense { get; set; }
        public string? totalprofit { get; set; }
    }
}
