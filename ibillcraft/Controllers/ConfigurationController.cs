using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using ibillcraft.Models;
using System.Net.Http;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace ibillcraft.Controllers
{
	[OnExceptions]

	public class ConfigurationController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ConfigurationController> _logger;
        private readonly IStringLocalizer<ConfigurationController> _localizer;
        public ConfigurationController(ILogger<ConfigurationController> logger, IStringLocalizer<ConfigurationController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:User").Value);
            _logger = logger;
            _localizer = localizer;
        }
        public IActionResult Index(string type)
        {
            try
            {
                if (type == null)
                {
                    type = "Terms";
                }
				GetCookies gk = new GetCookies();
				CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);
				// Check if session data is available

				if (string.IsNullOrEmpty(CUtility.comid))
				{
					// Handle missing session data
					return RedirectToAction("Index", "CompanyLoginRegistration");
				}
				ViewBag.Format = CUtility.format;
				Guid UserId = CUtility.rolename == "Admin" ? new Guid(CUtility.comid) : new Guid(CUtility.userid);

				var ConfigurationModelList = new List<ConfigurationModel>();
                string staffurl = $"{_httpClient.BaseAddress}/Configuration/GetAll?UserId={UserId}&Server_Value={CUtility.serverValue}&type={type}&tc_com_id={new Guid(CUtility.comid)}";
                HttpResponseMessage response = _httpClient.GetAsync(staffurl).Result;
                string selectedTab = "Terms-tab";
                if (type == "Note")
                {
                    selectedTab = "Note-tab";
                }
                if (type == "Series")
                {
                    selectedTab = "Series-tab";
                }
                ViewBag.SelectedTab = selectedTab;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<ConfigurationModel>() };
                    var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    ConfigurationModelList = response2.data;

                    if (ConfigurationModelList != null)
                    {
                        return View(ConfigurationModelList);
                    }
                    else
                    {
                        var ConfigurationModelDataList = new List<ConfigurationModel>();
                        return View(ConfigurationModelDataList);
                    }
                }
                return View(ConfigurationModelList);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }
        }
        [HttpPost]
        public IActionResult Create(ConfigurationModel model)
        {
            try
            {
				GetCookies gk = new GetCookies();
				CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);
				// Check if session data is available

				if (string.IsNullOrEmpty(CUtility.comid))
				{
					// Handle missing session data
					return RedirectToAction("Index", "CompanyLoginRegistration");
				}
				ViewBag.Format = CUtility.format;
				Guid UserId = CUtility.rolename == "Admin" ? new Guid(CUtility.comid) : new Guid(CUtility.userid);

				model.UserId = UserId;
                model.tc_com_id = new Guid(CUtility.comid);
                model.Server_Value = CUtility.serverValue;
                
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/Configuration", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic responsedata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(responsedata);
                    dynamic responsemodel = rootObject.outcome;
                    string outcomeDetail = responsemodel.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail;
                    return Ok(outcomeDetail);
                }
                TempData["errorMessage"] = response.Headers.ToString();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        public IActionResult Delete(Guid? id, string type)
        {
            try
            {
                ConfigurationModel model = new ConfigurationModel();
				GetCookies gk = new GetCookies();
				CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);
				// Check if session data is available

				if (string.IsNullOrEmpty(CUtility.comid))
				{
					// Handle missing session data
					return RedirectToAction("Index", "CompanyLoginRegistration");
				}
				ViewBag.Format = CUtility.format;
				Guid UserId = CUtility.rolename == "Admin" ? new Guid(CUtility.comid) : new Guid(CUtility.userid);

				model.UserId = UserId;
                model.tc_id = id;
                model.tc_com_id = new Guid(CUtility.comid);
                model.Server_Value = CUtility.serverValue;
                model.type = type;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/Configuration/Delete", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic responsedata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(responsedata);
                    dynamic responsemodel = rootObject.outcome;
                    string outcomeDetail = responsemodel.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail;
                    return RedirectToAction("Index");
                }
                TempData["errorMessage"] = response.Headers.ToString();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
