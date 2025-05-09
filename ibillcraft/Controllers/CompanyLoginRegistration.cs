using Common;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using ibillcraft.Models;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UAParser;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using Org.BouncyCastle.Asn1.Ocsp;
using Context;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ibillcraft.Controllers
{
	[OnExceptions]
	public class CompanyLoginRegistration : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CompanyLoginRegistration> _logger;
        private readonly IStringLocalizer<CompanyLoginRegistration> _localizer;
        private readonly string baseaddresuser;
        public CompanyLoginRegistration(ILogger<CompanyLoginRegistration> logger, IStringLocalizer<CompanyLoginRegistration> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);
            baseaddresuser = new Uri(configuration.GetSection("Server:Master").Value).ToString();
            _logger = logger;
            _localizer = localizer;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            try
            {
                string tblurl = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_CountryMaster&sValue=co_country_name&id=co_id&IsActiveColumn=co_isactive&sCoulmnName";
                HttpResponseMessage responseView = _httpClient.GetAsync(tblurl).Result;
                dynamic tbldata = responseView.Content.ReadAsStringAsync().Result;
                var rootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(tbldata);
                ViewBag.CountryId = rootObject;
                
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorIndex", "Home");
            }
        }
        [HttpPost]
        public IActionResult Create(LoginDetailsModel model)
        {
            try
            {
                string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    ipAddress = endPoint.Address.ToString();
                }
                string userAgentString = Request.Headers["User-Agent"].ToString();
                var uaParser = Parser.GetDefault();
                ClientInfo clientInfo = uaParser.Parse(userAgentString);
                string browserName = clientInfo.UserAgent.Family;
                string browserVersion = clientInfo.UserAgent.Major + "." + clientInfo.UserAgent.Minor;
                model.ip_address = ipAddress;
                model.browser_version = browserVersion;
                model.browser_name = browserName;
                model.CreatedDate = DateTime.Now;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string outcomeDetail = "Please enter valid credentials!!!";

                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/LoginDetails", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic responsedata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(responsedata);
                    dynamic responsemodel = rootObject.result.outcome;
                     outcomeDetail = responsemodel.outcomeDetail;
                    if (rootObject.result.data == null)
                    {
                        return Ok(outcomeDetail);

                    }
                    dynamic comid = rootObject.result.data;
                    string Server_Key = comid.Server_Key;
                    string com_id = comid.com_id;
                    string loginId = comid.LoginId;
                    string? comname = comid.com_company_name;
                    string? RoleId = comid.RoleId;
                    string? staffid = comid.staffid;
                    string? rolename = comid.rolename;
                    string? co_country_code = comid.co_country_code;
                    string? co_timezone = comid.co_timezone;
                    string? cm_currency_format = comid.cm_currency_format;
                    string? cm_currencysymbol = comid.cm_currencysymbol;
                    string? st_staff_name = comid.st_staff_name;
                    string Baseaddress =_httpClient.BaseAddress.ToString();

                    // ViewBag.comname = comname;

                    if (outcomeDetail == "Please enter valid credentials" || outcomeDetail == "Your subscription has been expired!" || RoleId == null)
                    {
                        if (RoleId == null)
                        {
                            outcomeDetail = "Please enter valid credentials!!!!";
                            return Ok(outcomeDetail);
                        }
                        return Ok(outcomeDetail);
                    }
                    else
                    {
                        string counturl = $"{_httpClient.BaseAddress}/LoginDetails/GetIsfirst?Server_Value={Server_Key}&com_id={com_id}";
                        jwtTokenCreate jkt = new jwtTokenCreate(_configuration);
                        
                        HttpResponseMessage response1 = _httpClient.GetAsync(counturl).Result;

                        dynamic cdata = response1.Content.ReadAsStringAsync().Result;
                        var cdataObject = new { data = new Object() };
                        var cresponses = JsonConvert.DeserializeAnonymousType(cdata, cdataObject);
                        int Count = cresponses.data.OutcomeId;
                        if (Count == 1)
                        {
                            TempData["successMessage"] = "Please select your Configuration Details";
                        }
                        else 
                        {
                            TempData["successMessage"] = outcomeDetail;
                        }
                        var result = new { Server_Key, com_id,Count, rolename };
                        
                        TempData["comname"] = comname;
                        //HttpContext.Session.SetString("com_id", com_id);
                        //HttpContext.Session.SetString("Server_Value", Server_Key);
                        //HttpContext.Session.SetString("loginId", loginId);
                        //HttpContext.Session.SetString("RoleId", RoleId);
                        //HttpContext.Session.SetString("RoleName", rolename);
                        //HttpContext.Session.SetString("StaffId", staffid);
                        //HttpContext.Session.SetString("BaseAddress", Baseaddress);
                        //HttpContext.Session.SetString("co_country_code", co_country_code);
                        //HttpContext.Session.SetString("co_timezone", co_timezone);
                        //HttpContext.Session.SetString("cm_currency_format", cm_currency_format);
                        //HttpContext.Session.SetString("cm_currencysymbol", cm_currencysymbol);
                        //HttpContext.Session.SetString("BaseAddressUser", baseaddresuser);

                        string jwtToken = jkt.GenerateJwtToken(com_id, Server_Key, comname, loginId, RoleId, rolename, staffid, Baseaddress, co_country_code, co_timezone, cm_currency_format, cm_currencysymbol, st_staff_name, baseaddresuser);
                        Response.Cookies.Append("jwtToken", jwtToken.ToString());

                        return Ok(result);
                    }
                }
                TempData["errorMessage"] = response.Headers.ToString();
                return Ok(outcomeDetail);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;

                return Ok(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult Delete()
        {
            try
            {
                LoginDetailsModel model = new LoginDetailsModel();
				GetCookies gk = new GetCookies();
				CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);
				// Check if session data is available

				if (string.IsNullOrEmpty(CUtility.comid))
				{
					// Handle missing session data
					return RedirectToAction("Index", "CompanyLoginRegistration");
				}
				ViewBag.Format = CUtility.format;

				string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    ipAddress = endPoint.Address.ToString();
                }
                string userAgentString = Request.Headers["User-Agent"].ToString();
                var uaParser = Parser.GetDefault();
                ClientInfo clientInfo = uaParser.Parse(userAgentString);
                string browserName = clientInfo.UserAgent.Family;
                string browserVersion = clientInfo.UserAgent.Major + "." + clientInfo.UserAgent.Minor;
                model.ip_address = ipAddress;
                model.browser_version = browserVersion;
                model.browser_name = browserName;
                model.CreatedDate = DateTime.Now;
                model.com_id = CUtility.comid;
                model.LoginId = CUtility.loginId;
                model.server_Value = CUtility.serverValue;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/LoginDetails/LogOut", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic responcedata = response.Content.ReadAsStringAsync().Result;
                    if (responcedata != null)
                    {
                       TempData["successMessage"] = responcedata;
                       Response.Cookies.Delete("jwtToken");
                      
                        return RedirectToAction("Index");
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public IActionResult ForgotPassword(LoginDetailsModel model)
        {
            try
            {
              
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/LoginDetails/ForgotPass", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic responcedata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(responcedata);
                    dynamic responsemodel = rootObject.outcomeDetail;
                    string outcomeDetail = responsemodel;
                    if (responcedata != null)
                    {
                       TempData["successMessage"] = outcomeDetail;
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public IActionResult Language(string culture)
        {
            culture = culture.Replace("?ui-culture=", "");
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
             CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMonths(1) });
            return RedirectToAction("Index", "CompanyLoginRegistration");
        }
    }
}
