using Common;
using ibillcraft.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Text;

namespace ibillcraft.Controllers
{
	public class ContactusController : Controller
	{
        private readonly HttpClient _httpClient;
        private readonly ILogger<ContactusController> _logger;
        private readonly IStringLocalizer<ContactusController> _localizer;
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _cache;
        private readonly IWebHostEnvironment _env;
        public ContactusController(ILogger<ContactusController> logger, IStringLocalizer<ContactusController> localizer, IConfiguration configuration, IDistributedCache cache, IWebHostEnvironment env)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:User").Value);
            _logger = logger;
            _localizer = localizer;
            _configuration = configuration;
            _cache = cache;
            _env = env;
        }
        public IActionResult Index()
		{
			return View();
		}

        [HttpPost]
        public IActionResult Create(ContactModel model)
        {
            try
            {
                //GetCookies gk = new GetCookies();
                //CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);
                //// Check if session data is available

                //if (string.IsNullOrEmpty(CUtility.comid))
                //{
                //    // Handle missing session data
                //    return RedirectToAction("Index", "CompanyLoginRegistration");
                //}
                //ViewBag.Format = CUtility.format;
                //Guid UserId = CUtility.rolename == "Admin" ? new Guid(CUtility.comid) : new Guid(CUtility.userid);

                //model.UserId = UserId;
                //model.c_com_id = new Guid(CUtility.comid);
                model.Server_Value = "Server_1";
                
                //SendMail sendmail = new SendMail();
                //DateTime? currentTime = sendmail.findtime(CUtility.co_timezone);

                //model.c_createddate = currentTime;
                //model.c_updateddate = currentTime;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/Contactus", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic responsedata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(responsedata);
                    dynamic responsemodel = rootObject.outcome;
                    dynamic outcomedata = rootObject.data;
                    string outcomeDetail = responsemodel.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail;
                    //if (outcomeDetail == "Order addded successfully!")
                    // {
                    // SendEmail(outcomedata);
                    //}
                    //var combinedData = new
                    //{
                    //    ProductData = outcomeDetail,
                        
                    //};
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
    }
}
