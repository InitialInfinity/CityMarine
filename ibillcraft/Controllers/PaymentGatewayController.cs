using ibillcraft.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Common;
using Microsoft.Extensions.Localization;

namespace ibillcraft.Controllers
{
    public class PaymentGatewayController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PaymentGatewayController> _logger;
        private readonly IStringLocalizer<PaymentGatewayController> _localizer;

        public PaymentGatewayController(ILogger<PaymentGatewayController> logger, IStringLocalizer<PaymentGatewayController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);
            _logger = logger;
            _localizer = localizer;
        }
        public IActionResult Index(string? ComId,string? SubId,string com_company_name,string com_company_email,string final_Amt,string country)
        {
            try
            {
                string tblurl = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_CountryMaster&sValue=co_country_name&id=co_id&IsActiveColumn=co_isactive&sCoulmnName";
                HttpResponseMessage responseView = _httpClient.GetAsync(tblurl).Result;
                dynamic tbldata = responseView.Content.ReadAsStringAsync().Result;
                var rootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(tbldata);
                ViewBag.CountryId = rootObject;
                ViewBag.ComId = ComId;
                ViewBag.sub_id = SubId;
                ViewBag.com_company_name = com_company_name;
                ViewBag.com_company_email = com_company_email;
                ViewBag.country = country;
                ViewBag.final_Amt = final_Amt;
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorIndex", "Home");
            }
        }
       
    }
}
