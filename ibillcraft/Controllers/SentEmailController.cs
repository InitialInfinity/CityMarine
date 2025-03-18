using Common;
using ibillcraft.Models;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;
using PageSize = iTextSharp.text.PageSize;
using Document = iTextSharp.text.Document;
using ClosedXML.Excel;
using OfficeOpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.IdentityModel.Tokens;
using System.Web.Helpers;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace ibillcraft.Controllers
{
    [ExampleFilter1]
    public class SentEmailController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SentEmailController> _logger;
        private readonly IStringLocalizer<SentEmailController> _localizer;
        public SentEmailController(ILogger<SentEmailController> logger, IStringLocalizer<SentEmailController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);
            _logger = logger;
            _localizer = localizer;
        }
        public IActionResult Index(string tab)
        {
            if(tab==null)
            {
                tab = "Insurance";
            }

            GetCookies gk = new GetCookies();
            CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

            if (string.IsNullOrEmpty(CUtility.comid))
            {
                // Handle missing session data
                return RedirectToAction("Index", "CompanyLoginRegistration");
            }

            ViewBag.Format = CUtility.format;

            Guid? UserId = new Guid(CUtility.userid);
            var sentemailDataList = new List<SentEmailModel>(); 
            var sentemailList = new List<SentEmailModel>();

            string geurl = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_EmailRuleConfg&sValue=E_value&id=E_id&IsActiveColumn=E_isactive&sCoulmnName=E_category&sColumnValue=b98e01a4-adf6-4c31-a41b-3572c8ea6cd3";
            HttpResponseMessage geresponseView = _httpClient.GetAsync(geurl).Result;
            dynamic gedata = geresponseView.Content.ReadAsStringAsync().Result;
            var gerootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(gedata);
            ViewBag.type = gerootObject;


            string sentemailurl = $"{_httpClient.BaseAddress}/SentEmail/GetAll?UserId={UserId}&type={tab}";
            HttpResponseMessage response = _httpClient.GetAsync(sentemailurl).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new List<SentEmailModel>() };
                var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                sentemailList = response2.data;

                if (sentemailList != null)
                {
                    return View(sentemailList);
                }
                else
                {
                    var sentemailList1 = new List<SentEmailModel>();
                    return View(sentemailList1);
                }
            }
            return View(sentemailDataList);
        }

        public JsonResult Tab(string tab)
        {
            if (tab == null)
            {
                tab = "Insurance";
            }

            GetCookies gk = new GetCookies();
            CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

            ViewBag.Format = CUtility.format;

            Guid? UserId = new Guid(CUtility.userid);
            var sentemailDataList = new List<SentEmailModel>();
            var sentemailList = new List<SentEmailModel>();
            string sentemailurl = $"{_httpClient.BaseAddress}/SentEmail/GetAll?UserId={UserId}&type={tab}";
            HttpResponseMessage response = _httpClient.GetAsync(sentemailurl).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new List<SentEmailModel>() };
                var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                sentemailList = response2.data;

                if (sentemailList != null)
                {
                    return Json(sentemailList);
                }
                else
                {
                    var sentemailList1 = new List<SentEmailModel>();
                    return Json(sentemailList1);
                }
            }
            return Json(sentemailDataList);
        }

        public JsonResult General(string tab)
        {
            if (tab == null)
            {
                tab = "General";
            }

            GetCookies gk = new GetCookies();
            CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

            ViewBag.Format = CUtility.format;

            Guid? UserId = new Guid(CUtility.userid);
            var sentemailDataList = new List<SentEmailModel>();
            var sentemailList = new List<SentEmailModel>();
            string sentemailurl = $"{_httpClient.BaseAddress}/SentEmail/General?UserId={UserId}&type={tab}";
            HttpResponseMessage response = _httpClient.GetAsync(sentemailurl).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new List<SentEmailModel>() };
                var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                sentemailList = response2.data;

                if (sentemailList != null)
                {
                    return Json(sentemailList);
                }
                else
                {
                    var sentemailList1 = new List<SentEmailModel>();
                    return Json(sentemailList1);
                }
            }
            return Json(sentemailDataList);
        }

        public JsonResult filter(string? from,string? to,string? subject,string? hasthewords, string? tab)
        {
            GetCookies gk = new GetCookies();
            CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

            ViewBag.Format = CUtility.format;

            Guid? UserId = new Guid(CUtility.userid);
            var sentemailDataList = new List<SentEmailModel>();
            var sentemailList = new List<SentEmailModel>();
            string sentemailurl = $"{_httpClient.BaseAddress}/SentEmail/GetEmail?UserId={UserId}&from={from}&to={to}&subject={subject}&hasthewords={hasthewords}&type={tab}";
            HttpResponseMessage response = _httpClient.GetAsync(sentemailurl).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new List<SentEmailModel>()};
                var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                sentemailList = response2.data;

                if (sentemailList != null)
                {
                    return Json(sentemailList);
                }
                else
                {
                    var sentemailList1 = new List<SentEmailModel>();
                    return Json(sentemailList1);
                }
            }
            return Json(sentemailDataList);
        }

        public JsonResult Generalfilter(string? from, string? to, string? subject, string? hasthewords, string? tab)
        {
            GetCookies gk = new GetCookies();
            CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

            ViewBag.Format = CUtility.format;

            Guid? UserId = new Guid(CUtility.userid);
            var sentemailDataList = new List<SentEmailModel>();
            var sentemailList = new List<SentEmailModel>();
            string sentemailurl = $"{_httpClient.BaseAddress}/SentEmail/Generalfilter?UserId={UserId}&from={from}&to={to}&subject={subject}&hasthewords={hasthewords}&type={tab}";
            HttpResponseMessage response = _httpClient.GetAsync(sentemailurl).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new List<SentEmailModel>() };
                var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                sentemailList = response2.data;

                if (sentemailList != null)
                {
                    return Json(sentemailList);
                }
                else
                {
                    var sentemailList1 = new List<SentEmailModel>();
                    return Json(sentemailList1);
                }
            }
            return Json(sentemailDataList);
        }

        public JsonResult showdetails(string id)
        {
            GetCookies gk = new GetCookies();
            CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

            ViewBag.Format = CUtility.format;
            Guid? UserId = new Guid(CUtility.comid);

            var sentclientDataList = new SentEmailModel();
            var sentclientList = new SentEmailModel();
            string sentclienturl = $"{_httpClient.BaseAddress}/SentEmail/Get?UserId={UserId}&s_id={id}";
            HttpResponseMessage response = _httpClient.GetAsync(sentclienturl).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new SentEmailModel() };
                var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                sentclientList = response2.data;

                if (sentclientList != null)
                {
                    return Json(sentclientList);
                }
                else
                {
                    var sentclientList1 = new List<SentEmailModel>();
                    return Json(sentclientList1);
                }
            }
            return Json(sentclientDataList);
        }


        public JsonResult Generaltype(string type)
        {
            //if (tab == null)
            //{
            //    tab = "General";
            //}
            //  string connectionString = "Server=103.182.153.94,1433;Database=dbCityMarine_UAT;User Id=dbCityMarine_UAT;Password=dbCityMarine_UAT@2024;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;";

            GetCookies gk = new GetCookies();
            CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

            ViewBag.Format = CUtility.format;

            Guid? UserId = new Guid(CUtility.userid);
            var sentemailDataList = new List<SentEmailModel>();
            var sentemailList = new List<SentEmailModel>();

            string encodedType = Uri.EscapeDataString(type);


            string sentemailurl = $"{_httpClient.BaseAddress}/SentEmail/Generaltype?UserId={UserId}&s_generaltype={encodedType}&tab=general";
            HttpResponseMessage response = _httpClient.GetAsync(sentemailurl).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new List<SentEmailModel>() };
                var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                sentemailList = response2.data;

                if (sentemailList != null)
                {
                    return Json(sentemailList);
                }
                else
                {
                    var sentemailList1 = new List<SentEmailModel>();
                    return Json(sentemailList1);
                }
            }
            return Json(sentemailDataList);
        }

        private string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath)?.ToLowerInvariant();

            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".pdf" => "application/pdf",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".txt" => "text/plain",
                ".csv" => "text/csv",
                _ => "application/octet-stream", // Default MIME type for unknown extensions
            };
        }

        [HttpGet]
        public IActionResult DownloadFile(string filePath)
        {
            // Check if the file path is provided
            if (string.IsNullOrEmpty(filePath))
            {
                return BadRequest("File path cannot be null or empty.");
            }

            // Ensure the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            try
            {
                // Get the file name
                var fileName = Path.GetFileName(filePath);

                // Determine the MIME type based on the file extension
                var contentType = GetMimeType(filePath);

                // Read the file into a byte array
                var fileBytes = System.IO.File.ReadAllBytes(filePath);

                // Return the file as a download
                return File(fileBytes, contentType, fileName);
            }
            catch (Exception ex)
            {
                // Handle errors (e.g., log the error)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult ViewFile(string filePath)
        {
            // Check if the file path is provided
            if (string.IsNullOrEmpty(filePath))
            {
                return BadRequest("File path cannot be null or empty.");
            }

            // Ensure the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            try
            {
                // Determine the MIME type based on the file extension
                var contentType = GetMimeType(filePath);

                // Read the file into a byte array
                var fileBytes = System.IO.File.ReadAllBytes(filePath);

                // Return the file to be viewed in the browser
                return File(fileBytes, contentType);
            }
            catch (Exception ex)
            {
                // Handle errors (e.g., log the error)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        public JsonResult Inboxdates(string id)
        {

            GetCookies gk = new GetCookies();
            CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

            ViewBag.Format = CUtility.format;

            Guid? UserId = new Guid(CUtility.userid);
            var sentemailDataList = new List<SentEmailModel>();
            var sentemailList = new List<SentEmailModel>();
            string sentemailurl = $"{_httpClient.BaseAddress}/SentEmail/inboxdates?UserId={UserId}&id={id}";
            HttpResponseMessage response = _httpClient.GetAsync(sentemailurl).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new List<SentEmailModel>() };
                var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                sentemailList = response2.data;

                if (sentemailList != null)
                {
                    return Json(sentemailList);
                }
                else
                {
                    var sentemailList1 = new List<SentEmailModel>();
                    return Json(sentemailList1);
                }
            }
            return Json(sentemailDataList);
        }

    }
}
