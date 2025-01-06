using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;


namespace ibillcraft.Models
{
	public class GetCookies
	{
		public IConfiguration configuration;
		public CookiesUtility GetCookiesvalue(string jwtToken)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.ReadJwtToken(jwtToken);

			// Access claims
			string? comid = token.Claims.FirstOrDefault(c => c.Type == "com_id")?.Value;
			string? serverValue = token.Claims.FirstOrDefault(c => c.Type == "Server_Value")?.Value;
			string? RoleId = token.Claims.FirstOrDefault(c => c.Type == "RoleId")?.Value;
			string? userid = token.Claims.FirstOrDefault(c => c.Type == "StaffId")?.Value;
			string? rolename = token.Claims.FirstOrDefault(c => c.Type == "RoleName")?.Value;
			string? co_timezone = token.Claims.FirstOrDefault(c => c.Type == "co_timezone")?.Value;
			string? format = token.Claims.FirstOrDefault(c => c.Type == "cm_currency_format")?.Value;
			string? loginId = token.Claims.FirstOrDefault(c => c.Type == "loginId")?.Value;
			string? BaseAddress = token.Claims.FirstOrDefault(c => c.Type == "BaseAddress")?.Value;
			string? co_country_code = token.Claims.FirstOrDefault(c => c.Type == "co_country_code")?.Value;
			string? cm_currencysymbol = token.Claims.FirstOrDefault(c => c.Type == "cm_currencysymbol")?.Value;
			string? baseaddresuser = token.Claims.FirstOrDefault(c => c.Type == "baseaddresuser")?.Value;
			string? st_staff_name = token.Claims.FirstOrDefault(c => c.Type == "st_staff_name")?.Value;
			string? comname = token.Claims.FirstOrDefault(c => c.Type == "comname")?.Value;
			CookiesUtility ck = new CookiesUtility();
			ck.comid = comid;
			ck.serverValue = serverValue;
			ck.rolename = rolename;
			ck.userid = userid;
			ck.co_timezone = co_timezone;
			ck.format = format;
			ck.loginId = loginId;
			ck.Baseaddress = BaseAddress;
			ck.co_country_code = co_country_code;
			ck.cm_currencysymbol = cm_currencysymbol;
			ck.baseaddresuser = baseaddresuser;
			ck.st_staff_name = st_staff_name;
			ck.RoleId = RoleId;
			ck.comname = comname;
			return ck;

		}
		public List<Home> GetMenu(string token )
		{
			GetCookies gk = new GetCookies();
			CookiesUtility CUtility = gk.GetCookiesvalue(token);
			var homeDataList = new List<Home>();
			HttpClient _httpClient= new HttpClient();
			var handler = new HttpClientHandler();
			handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
			_httpClient = new HttpClient(handler);
			//_httpClient.BaseAddress= "https://localhost:7119/api";
			HttpResponseMessage response = _httpClient.GetAsync($"{CUtility.Baseaddress}/GetWebMenu/GetAll?RoleId={CUtility.RoleId}&server={CUtility.serverValue}").Result;

			if (response.IsSuccessStatusCode)
			{
				dynamic data = response.Content.ReadAsStringAsync().Result;
				var dataObject = new { data = new List<Home>() };
				var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
				homeDataList = response2.data;
				return homeDataList;
			}
			return homeDataList;
		}

        public Object GetImage(string token)
        {
            GetCookies gk = new GetCookies();
            CookiesUtility CUtility = gk.GetCookiesvalue(token);

            HttpClient _httpClient = new HttpClient();
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);

            HttpResponseMessage response = _httpClient.GetAsync($"{CUtility.baseaddresuser}/CompanyDetails/GetCompany?UserId={CUtility.userid}&Server_Value={CUtility.serverValue}&com_id={new Guid(CUtility.comid)}").Result;
            string comlogo = "";
            string staffName = CUtility.st_staff_name;
            var result1 = new { ImageBase64 = comlogo, StaffName = staffName };
            if (response.IsSuccessStatusCode)
            {
                dynamic logodata = response.Content.ReadAsStringAsync().Result;
                var logodataObject = new { data = new CompanyDetailsModel() };
                var logoresponse = JsonConvert.DeserializeAnonymousType(logodata, logodataObject);
                comlogo = logoresponse.data.ImageBase64;
				var result = new { ImageBase64= comlogo, StaffName = staffName };
				return result;
            }
            return result1;
        }
    }
}
