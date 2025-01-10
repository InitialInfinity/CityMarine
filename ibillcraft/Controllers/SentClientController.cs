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


namespace ibillcraft.Controllers
{
    [ExampleFilter1]
    public class SentClientController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SentClientController> _logger;
        private readonly IStringLocalizer<SentClientController> _localizer;
        public SentClientController(ILogger<SentClientController> logger, IStringLocalizer<SentClientController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);
            _logger = logger;
            _localizer = localizer;
        }
        public IActionResult Index(string email, string tab)
        {
            if(tab==null)
            {
                tab = "Enquiry";
            }
            GetCookies gk = new GetCookies();
            CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

            if (string.IsNullOrEmpty(CUtility.comid))
            {
                // Handle missing session data
                return RedirectToAction("Index", "CompanyLoginRegistration");
            }

            ViewBag.Format = CUtility.format;
            Guid? UserId = new Guid(CUtility.comid);
            string url = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_Customermaster&sValue=c_name&id=c_id&IsActiveColumn=c_isactive";
            HttpResponseMessage responseView = _httpClient.GetAsync(url).Result;
            dynamic data1 = responseView.Content.ReadAsStringAsync().Result;
            var rootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(data1);
            ViewBag.customer = rootObject;

            string geurl = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_ParameterValueMaster&sValue=pv_parametervalue&id=pv_id&IsActiveColumn=pv_isactive&sCoulmnName=pv_parameterid&sColumnValue=ce8449d3-24fb-49c4-8dd8-6a6093d7607c";
            HttpResponseMessage geresponseView = _httpClient.GetAsync(geurl).Result;
            dynamic gedata = geresponseView.Content.ReadAsStringAsync().Result;
            var gerootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(gedata);
            ViewBag.year = gerootObject;


            var sentclientDataList = new List<SentClientModel>();
            var sentclientList = new List<SentClientModel>();
            string sentclienturl = $"{_httpClient.BaseAddress}/SentClient/GetAll?UserId={UserId}&sc_to={email}&sc_type={tab}";
            HttpResponseMessage response = _httpClient.GetAsync(sentclienturl).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new List<SentClientModel>() };
                var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                sentclientList = response2.data;
                //ViewBag.sc_to = sentclientList[0].sc_to;
                //ViewBag.sc_email = sentclientList[0].sc_email;
                //ViewBag.sc_toemail = sentclientList[0].sc_toemail;

                if (sentclientList != null)
                {
                    return View(sentclientList);
                }
                else
                {
                    var sentclientList1 = new List<SentClientModel>();
                    return View(sentclientList1);
                }
            }
            return View(sentclientDataList);
        }

        public JsonResult Tab(string email, string tab)
        {
            GetCookies gk = new GetCookies();
            CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

            //if (string.IsNullOrEmpty(CUtility.comid))
            //{
            //    // Handle missing session data
            //    return RedirectToAction("Index", "CompanyLoginRegistration");
            //}

            ViewBag.Format = CUtility.format;
            Guid? UserId = new Guid(CUtility.comid);
            string url = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_Customermaster&sValue=c_name&id=c_id&IsActiveColumn=c_isactive";
            HttpResponseMessage responseView = _httpClient.GetAsync(url).Result;
            dynamic data1 = responseView.Content.ReadAsStringAsync().Result;
            var rootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(data1);
            ViewBag.customer = rootObject;


            var sentclientDataList = new List<SentClientModel>();
            var sentclientList = new List<SentClientModel>();
            string sentclienturl = $"{_httpClient.BaseAddress}/SentClient/GetAll?UserId={UserId}&sc_to={email}&sc_type={tab}";
            HttpResponseMessage response = _httpClient.GetAsync(sentclienturl).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new List<SentClientModel>() };
                var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                sentclientList = response2.data;
                //ViewBag.sc_to = sentclientList[0].sc_to;
                //ViewBag.sc_email = sentclientList[0].sc_email;
                //ViewBag.sc_toemail = sentclientList[0].sc_toemail;

                if (sentclientList != null)
                {
                    return Json(sentclientList);
                }
                else
                {
                    var sentclientList1 = new List<SentClientModel>();
                    return Json(sentclientList1);
                }
            }
            return Json(sentclientDataList);
        }

        public JsonResult fetchdetails(string? sc_year, string? sc_from,string? tab)
        {
            GetCookies gk = new GetCookies();
            CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);         

            ViewBag.Format = CUtility.format;
            Guid? UserId = new Guid(CUtility.comid);

            var sentclientDataList = new List<SentClientModel>();
            var sentclientList = new List<SentClientModel>();
            string sentclienturl = $"{_httpClient.BaseAddress}/SentClient/GetDetails?UserId={UserId}&sc_year={sc_year}&sc_from={sc_from}&sc_type={tab}";
            HttpResponseMessage response = _httpClient.GetAsync(sentclienturl).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new List<SentClientModel>() };
                var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                sentclientList = response2.data;
                //ViewBag.sc_to = sentclientList.sc_to;

                if (sentclientList != null)
                {
                    return Json(sentclientList);
                }
                else
                {
                    var sentclientList1 = new List<SentClientModel>();
                    return Json(sentclientList1);
                }
            }
            return Json(sentclientDataList);
        }

        public JsonResult showdetails(string id)
        {
            GetCookies gk = new GetCookies();
            CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

            ViewBag.Format = CUtility.format;
            Guid? UserId = new Guid(CUtility.comid);

            var sentclientDataList = new SentClientModel();
            var sentclientList = new SentClientModel();
            string sentclienturl = $"{_httpClient.BaseAddress}/SentClient/Get?UserId={UserId}&sc_id={id}";
            HttpResponseMessage response = _httpClient.GetAsync(sentclienturl).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new SentClientModel() };
                var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                sentclientList = response2.data;

                if (sentclientList != null)
                {
                    return Json(sentclientList);
                }
                else
                {
                    var sentclientList1 = new List<SentClientModel>();
                    return Json(sentclientList1);
                }
            }
            return Json(sentclientDataList);
        }

        public JsonResult Clientchange(string? clientid, string? tab)
        {
            GetCookies gk = new GetCookies();
            CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

            ViewBag.Format = CUtility.format;
            Guid? UserId = new Guid(CUtility.comid);

            var sentclientDataList = new List<SentClientModel>();
            var sentclientList = new List<SentClientModel>();
            string sentclienturl = $"{_httpClient.BaseAddress}/SentClient/Clientchange?UserId={UserId}&clientid={clientid}&sc_type={tab}";
            HttpResponseMessage response = _httpClient.GetAsync(sentclienturl).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new List<SentClientModel>() };
                var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                sentclientList = response2.data;

                if (sentclientList != null)
                {
                    return Json(sentclientList);
                }
                else
                {
                    var sentclientList1 = new List<SentClientModel>();
                    return Json(sentclientList1);
                }
            }
            return Json(sentclientDataList);
        }

        public JsonResult filter(string? from, string? to, string? subject, string? hasthewords, string year, string? tab)
        {
            GetCookies gk = new GetCookies();
            CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

            ViewBag.Format = CUtility.format;

            Guid? UserId = new Guid(CUtility.comid);
            var sentemailDataList = new List<SentClientModel>();
            var sentemailList = new List<SentClientModel>();
            string sentemailurl = $"{_httpClient.BaseAddress}/SentClient/GetEmail?UserId={UserId}&from={from}&to={to}&subject={subject}&hasthewords={hasthewords}&sc_year={year}&type={tab}";
            HttpResponseMessage response = _httpClient.GetAsync(sentemailurl).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new List<SentClientModel>() };
                var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                sentemailList = response2.data;

                if (sentemailList != null)
                {
                    return Json(sentemailList);
                }
                else
                {
                    var sentemailList1 = new List<SentClientModel>();
                    return Json(sentemailList1);
                }
            }
            return Json(sentemailDataList);
        }

        public JsonResult general(string tab)
        {
            GetCookies gk = new GetCookies();
            CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

            //if (string.IsNullOrEmpty(CUtility.comid))
            //{
            //    // Handle missing session data
            //    return RedirectToAction("Index", "CompanyLoginRegistration");
            //}

            ViewBag.Format = CUtility.format;
            Guid? UserId = new Guid(CUtility.comid);
            string url = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_Customermaster&sValue=c_name&id=c_id&IsActiveColumn=c_isactive";
            HttpResponseMessage responseView = _httpClient.GetAsync(url).Result;
            dynamic data1 = responseView.Content.ReadAsStringAsync().Result;
            var rootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(data1);
            ViewBag.customer = rootObject;


            var sentclientDataList = new List<SentClientModel>();
            var sentclientList = new List<SentClientModel>();
            string sentclienturl = $"{_httpClient.BaseAddress}/SentClient/GetGeneral?UserId={UserId}&sc_type={tab}";
            HttpResponseMessage response = _httpClient.GetAsync(sentclienturl).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new List<SentClientModel>() };
                var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                sentclientList = response2.data;
                //ViewBag.sc_to = sentclientList[0].sc_to;
                //ViewBag.sc_email = sentclientList[0].sc_email;
                //ViewBag.sc_toemail = sentclientList[0].sc_toemail;

                if (sentclientList != null)
                {
                    return Json(sentclientList);
                }
                else
                {
                    var sentclientList1 = new List<SentClientModel>();
                    return Json(sentclientList1);
                }
            }
            return Json(sentclientDataList);
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

        public JsonResult Clientchange1(string? clientid, string? tab, string? year)
        {
            GetCookies gk = new GetCookies();
            CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

            ViewBag.Format = CUtility.format;
            Guid? UserId = new Guid(CUtility.comid);

            var sentclientDataList = new List<SentClientModel>();
            var sentclientList = new List<SentClientModel>();
            string sentclienturl = $"{_httpClient.BaseAddress}/SentClient/Clientchange1?UserId={UserId}&clientid={clientid}&sc_type={tab}&sc_year={year}";
            HttpResponseMessage response = _httpClient.GetAsync(sentclienturl).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new List<SentClientModel>() };
                var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                sentclientList = response2.data;

                if (sentclientList != null)
                {
                    return Json(sentclientList);
                }
                else
                {
                    var sentclientList1 = new List<SentClientModel>();
                    return Json(sentclientList1);
                }
            }
            return Json(sentclientDataList);
        }

    }

}
