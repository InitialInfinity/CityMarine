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
using Microsoft.Data.SqlClient;
using System.Net.Http.Headers;
using Microsoft.Identity.Client;
using System.Timers;
using Timer = System.Timers.Timer;
using static ibillcraft.Models.GraphApiEmailResponse;
using Twilio.TwiML.Messaging;
using System.Globalization;
using System.Net;
using System.Configuration;
using System;

namespace ibillcraft.Controllers
{
    [ExampleFilter1]
    public class InboxEmailController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClient _httpClient1;
        private readonly HttpClient _httpClient2;
        private readonly ILogger<InboxEmailController> _logger;
        private readonly IStringLocalizer<InboxEmailController> _localizer;
        private Timer timer = new Timer();
        string sql;
        public static string emailconfig;
        public InboxEmailController(ILogger<InboxEmailController> logger, IStringLocalizer<InboxEmailController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            var handler1 = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            // _httpClient1 = new HttpClient(handler);
            _httpClient1 = new HttpClient(handler1);
            _httpClient2 = new HttpClient(handler);

            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);
            sql = new(configuration.GetSection("Server:Sql").Value);
            emailconfig = new(configuration.GetSection("UploadSettings:emailconfig").Value);
            //_httpClient2.BaseAddress = new Uri(configuration.GetSection("Server:SavePath").Value);

            _logger = logger;
            _localizer = localizer;
        }

        public IActionResult Index(string tab)
        {
            if (tab == null)
            {
                tab = "Client";
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
            var inboxemailDataList = new List<InboxEmailModel>();
            var inboxemailList = new List<InboxEmailModel>();

            string clientnourl = $"{_httpClient.BaseAddress}/InboxEmail/dropdownclientno?UserId={UserId}&i_type={tab}";
            HttpResponseMessage clientnoresponseView = _httpClient.GetAsync(clientnourl).Result;
            dynamic clientnodata = clientnoresponseView.Content.ReadAsStringAsync().Result;
            var clientResponse = JsonConvert.DeserializeObject<InboxClientModel>(clientnodata);
            ViewBag.clientno = clientResponse.Data;


            string inboxemailurl = $"{_httpClient.BaseAddress}/InboxEmail/GetAll?UserId={UserId}&type={tab}";
            HttpResponseMessage response = _httpClient.GetAsync(inboxemailurl).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new List<InboxEmailModel>() };
                var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                inboxemailList = response2.data;

                if (inboxemailList != null)
                {
                    return View(inboxemailList);
                }
                else
                {
                    var inboxemailList1 = new List<InboxEmailModel>();
                    return View(inboxemailList1);
                }
            }
            return View(inboxemailDataList);
        }


        public JsonResult ClientGenaral(string tab, string? dropdownvalue)
        {
            if (tab == null)
            {
                tab = "Client";
            }

            GetCookies gk = new GetCookies();
            CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

            ViewBag.Format = CUtility.format;

            Guid? UserId = new Guid(CUtility.userid);
            var inboxemailDataList = new List<InboxEmailModel>();
            var inboxemailList = new List<InboxEmailModel>();


            string inboxemailurl = $"{_httpClient.BaseAddress}/InboxEmail/GetAll?UserId={UserId}&type={tab}&clientno={dropdownvalue}";
            HttpResponseMessage response = _httpClient.GetAsync(inboxemailurl).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new List<InboxEmailModel>() };
                var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                inboxemailList = response2.data;

                if (inboxemailList != null)
                {
                    return Json(inboxemailList);
                }
                else
                {
                    var inboxemailList1 = new List<InboxEmailModel>();
                    return Json(inboxemailList1);
                }
            }
            return Json(inboxemailDataList);
        }


        public JsonResult EnquiryClaim(string tab)
        {
            if (tab == null)
            {
                tab = "Enquiry";
            }

            GetCookies gk = new GetCookies();
            CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

            ViewBag.Format = CUtility.format;

            Guid? UserId = new Guid(CUtility.userid);
            var sentemailDataList = new List<InboxEmailModel>();
            var sentemailList = new List<InboxEmailModel>();
            string sentemailurl = $"{_httpClient.BaseAddress}/InboxEmail/EnquiryClaim?UserId={UserId}&type={tab}";
            HttpResponseMessage response = _httpClient.GetAsync(sentemailurl).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new List<InboxEmailModel>() };
                var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                sentemailList = response2.data;

                if (sentemailList != null)
                {
                    return Json(sentemailList);
                }
                else
                {
                    var sentemailList1 = new List<InboxEmailModel>();
                    return Json(sentemailList1);
                }
            }
            return Json(sentemailDataList);
        }

        public JsonResult Showdetails(string id)
        {
            GetCookies gk = new GetCookies();
            CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

            ViewBag.Format = CUtility.format;
            Guid? UserId = new Guid(CUtility.comid);

            var sentclientDataList = new InboxEmailModel();
            var sentclientList = new InboxEmailModel();
            string sentclienturl = $"{_httpClient.BaseAddress}/InboxEmail/Showdetails?UserId={UserId}&i_id={id}";
            HttpResponseMessage response = _httpClient.GetAsync(sentclienturl).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new InboxEmailModel() };
                var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                sentclientList = response2.data;

                if (sentclientList != null)
                {
                    return Json(sentclientList);
                }
                else
                {
                    var sentclientList1 = new List<InboxEmailModel>();
                    return Json(sentclientList1);
                }
            }
            return Json(sentclientDataList);
        }

        public JsonResult Inboxdates(string id)
        {

            GetCookies gk = new GetCookies();
            CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

            ViewBag.Format = CUtility.format;

            Guid? UserId = new Guid(CUtility.userid);
            var sentemailDataList = new List<InboxEmailModel>();
            var sentemailList = new List<InboxEmailModel>();
            string sentemailurl = $"{_httpClient.BaseAddress}/InboxEmail/inboxdates?UserId={UserId}&id={id}";
            HttpResponseMessage response = _httpClient.GetAsync(sentemailurl).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new List<InboxEmailModel>() };
                var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                sentemailList = response2.data;

                if (sentemailList != null)
                {
                    return Json(sentemailList);
                }
                else
                {
                    var sentemailList1 = new List<InboxEmailModel>();
                    return Json(sentemailList1);
                }
            }
            return Json(sentemailDataList);
        }

        public JsonResult EnquiryClaimfilter(string? from, string? to, string? subject, string? hasthewords, string? tab)
        {
            GetCookies gk = new GetCookies();
            CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

            ViewBag.Format = CUtility.format;

            Guid? UserId = new Guid(CUtility.userid);
            var sentemailDataList = new List<InboxEmailModel>();
            var sentemailList = new List<InboxEmailModel>();
            string sentemailurl = $"{_httpClient.BaseAddress}/InboxEmail/EnquiryClaimfilter?UserId={UserId}&from={from}&to={to}&subject={subject}&hasthewords={hasthewords}&type={tab}";
            HttpResponseMessage response = _httpClient.GetAsync(sentemailurl).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new List<InboxEmailModel>() };
                var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                sentemailList = response2.data;

                if (sentemailList != null)
                {
                    return Json(sentemailList);
                }
                else
                {
                    var sentemailList1 = new List<InboxEmailModel>();
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
            var sentemailDataList = new List<InboxEmailModel>();
            var sentemailList = new List<InboxEmailModel>();
            string sentemailurl = $"{_httpClient.BaseAddress}/InboxEmail/Generalfilter?UserId={UserId}&from={from}&to={to}&subject={subject}&hasthewords={hasthewords}&type={tab}";
            HttpResponseMessage response = _httpClient.GetAsync(sentemailurl).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new List<InboxEmailModel>() };
                var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                sentemailList = response2.data;

                if (sentemailList != null)
                {
                    return Json(sentemailList);
                }
                else
                {
                    var sentemailList1 = new List<InboxEmailModel>();
                    return Json(sentemailList1);
                }
            }
            return Json(sentemailDataList);
        }












        //public IActionResult Index(string tab)
        //{
        //    if (tab == null)
        //    {
        //        tab = "Insurance";
        //    }

        //    GetCookies gk = new GetCookies();
        //    CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

        //    if (string.IsNullOrEmpty(CUtility.comid))
        //    {
        //        // Handle missing session data
        //        return RedirectToAction("Index", "CompanyLoginRegistration");
        //    }

        //    ViewBag.Format = CUtility.format;

        //    Guid? UserId = new Guid(CUtility.userid);
        //    var sentemailDataList = new List<InboxEmailModel>();
        //    var sentemailList = new List<InboxEmailModel>();


        //    //string geurl = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_ParameterValueMaster&sValue=pv_parametervalue&id=pv_id&IsActiveColumn=pv_isactive&sCoulmnName=pv_parameterid&sColumnValue=3a6b099f-4045-4eac-891a-c2f701c45d86";
        //    //HttpResponseMessage geresponseView = _httpClient.GetAsync(geurl).Result;
        //    //dynamic gedata = geresponseView.Content.ReadAsStringAsync().Result;
        //    //var gerootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(gedata);
        //    //ViewBag.type = gerootObject;

        //    string geurl = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_EmailRuleConfg&sValue=E_value&id=E_id&IsActiveColumn=E_isactive&sCoulmnName=E_category&sColumnValue=b98e01a4-adf6-4c31-a41b-3572c8ea6cd3";
        //    HttpResponseMessage geresponseView = _httpClient.GetAsync(geurl).Result;
        //    dynamic gedata = geresponseView.Content.ReadAsStringAsync().Result;
        //    var gerootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(gedata);
        //    ViewBag.type = gerootObject;


        //        string claimnourl1 = $"{_httpClient.BaseAddress}/InboxEmail/dropdownclaimno?UserId={UserId}&i_type={tab}";
        //        HttpResponseMessage claimnoresponseView1 = _httpClient.GetAsync(claimnourl1).Result;
        //        dynamic claimnodata1 = claimnoresponseView1.Content.ReadAsStringAsync().Result;
        //        var claimResponse1 = JsonConvert.DeserializeObject<InboxClientModel>(claimnodata1);
        //        ViewBag.enquiryno = claimResponse1.Data;


        //        string claimnourl = $"{_httpClient.BaseAddress}/InboxEmail/dropdownclaimno?UserId={UserId}&i_type={tab}";
        //        HttpResponseMessage claimnoresponseView = _httpClient.GetAsync(claimnourl).Result;
        //        dynamic claimnodata = claimnoresponseView.Content.ReadAsStringAsync().Result;
        //        var claimResponse = JsonConvert.DeserializeObject<InboxClientModel>(claimnodata);
        //        ViewBag.claimno = claimResponse.Data;



        //    string sentemailurl = $"{_httpClient.BaseAddress}/InboxEmail/GetAll?UserId={UserId}&type={tab}";
        //    HttpResponseMessage response = _httpClient.GetAsync(sentemailurl).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        dynamic data = response.Content.ReadAsStringAsync().Result;
        //        var dataObject = new { data = new List<InboxEmailModel>() };
        //        var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
        //        sentemailList = response2.data;

        //        if (sentemailList != null)
        //        {
        //            return View(sentemailList);
        //        }
        //        else
        //        {
        //            var sentemailList1 = new List<InboxEmailModel>();
        //            return View(sentemailList1);
        //        }
        //    }
        //    return View(sentemailDataList);
        //}

        //public JsonResult Tab(string tab)
        //{
        //    if (tab == null)
        //    {
        //        tab = "Insurance";
        //    }

        //    GetCookies gk = new GetCookies();
        //    CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

        //    ViewBag.Format = CUtility.format;

        //    Guid? UserId = new Guid(CUtility.userid);
        //    var sentemailDataList = new List<InboxEmailModel>();
        //    var sentemailList = new List<InboxEmailModel>();
        //    string sentemailurl = $"{_httpClient.BaseAddress}/InboxEmail/GetAll?UserId={UserId}&type={tab}";
        //    HttpResponseMessage response = _httpClient.GetAsync(sentemailurl).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        dynamic data = response.Content.ReadAsStringAsync().Result;
        //        var dataObject = new { data = new List<InboxEmailModel>() };
        //        var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
        //        sentemailList = response2.data;

        //        if (sentemailList != null)
        //        {
        //            return Json(sentemailList);
        //        }
        //        else
        //        {
        //            var sentemailList1 = new List<InboxEmailModel>();
        //            return Json(sentemailList1);
        //        }
        //    }
        //    return Json(sentemailDataList);
        //}



        //public JsonResult General(string tab)
        //{
        //    tab ??= "General";  // Null coalescing assignment

        //    var gk = new GetCookies();
        //    var cUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);
        //    ViewBag.Format = cUtility.format;

        //    Guid userId = new Guid(cUtility.userid);
        //    var sentemailList = new List<InboxEmailModel>();

        //    if (tab == "Claim one" )
        //    {
        //        string claimNoUrl = $"{_httpClient.BaseAddress}/InboxEmail/dropdownclaimno?UserId={userId}&i_type={tab}";
        //        var claimNoResponse = _httpClient.GetAsync(claimNoUrl).Result;

        //        if (claimNoResponse.IsSuccessStatusCode)
        //        {
        //            var claimData = claimNoResponse.Content.ReadAsStringAsync().Result;
        //            var claimResponse = JsonConvert.DeserializeObject<InboxClientModel>(claimData);


        //            ViewBag.claimno = claimResponse.Data;
        //        }
        //    }
        //    if (tab == "Enquiry one")
        //    {
        //        string enquiryNoUrl = $"{_httpClient.BaseAddress}/InboxEmail/dropdownenquiryno?UserId={userId}&i_type={tab}";
        //        var enquiryNoResponse = _httpClient.GetAsync(enquiryNoUrl).Result;

        //            if (enquiryNoResponse.IsSuccessStatusCode)
        //            {
        //                var claimData = enquiryNoResponse.Content.ReadAsStringAsync().Result;
        //                var claimResponse = JsonConvert.DeserializeObject<InboxClientModel>(claimData);


        //                ViewBag.enquiryno = claimResponse.Data;
        //            }
        //    }

        //    // Fetch emails
        //    string emailUrl = $"{_httpClient.BaseAddress}/InboxEmail/General?UserId={userId}&type={tab}";
        //    var emailResponse = _httpClient.GetAsync(emailUrl).Result;

        //    if (emailResponse.IsSuccessStatusCode)
        //    {
        //        var data = emailResponse.Content.ReadAsStringAsync().Result;
        //        var parsed = JsonConvert.DeserializeAnonymousType(data, new { data = new List<InboxEmailModel>() });
        //        sentemailList = parsed.data ?? new List<InboxEmailModel>();
        //    }

        //    // Final JSON return
        //    if (tab == "Claim one")
        //    {
        //        return Json(new
        //        {
        //            sentemailList,
        //            claimno = ViewBag.claimno
        //        });
        //    }
        //    else if (tab == "Enquiry one")
        //    {
        //        return Json(new
        //        {
        //            sentemailList,
        //            enquiryno = ViewBag.enquiryno
        //        });
        //    }
        //    else
        //    {
        //        return Json(new
        //        {
        //            sentemailList
        //        });
        //    }
        //}


        //public JsonResult filter(string? from, string? to, string? subject, string? hasthewords, string? tab)
        //{
        //    GetCookies gk = new GetCookies();
        //    CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

        //    ViewBag.Format = CUtility.format;

        //    Guid? UserId = new Guid(CUtility.userid);
        //    var sentemailDataList = new List<InboxEmailModel>();
        //    var sentemailList = new List<InboxEmailModel>();
        //    string sentemailurl = $"{_httpClient.BaseAddress}/InboxEmail/GetEmail?UserId={UserId}&from={from}&to={to}&subject={subject}&hasthewords={hasthewords}&type={tab}";
        //    HttpResponseMessage response = _httpClient.GetAsync(sentemailurl).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        dynamic data = response.Content.ReadAsStringAsync().Result;
        //        var dataObject = new { data = new List<InboxEmailModel>() };
        //        var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
        //        sentemailList = response2.data;

        //        if (sentemailList != null)
        //        {
        //            return Json(sentemailList);
        //        }
        //        else
        //        {
        //            var sentemailList1 = new List<InboxEmailModel>();
        //            return Json(sentemailList1);
        //        }
        //    }
        //    return Json(sentemailDataList);
        //}

        //public JsonResult Generalfilter(string? from, string? to, string? subject, string? hasthewords, string? tab)
        //{
        //    GetCookies gk = new GetCookies();
        //    CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

        //    ViewBag.Format = CUtility.format;

        //    Guid? UserId = new Guid(CUtility.userid);
        //    var sentemailDataList = new List<InboxEmailModel>();
        //    var sentemailList = new List<InboxEmailModel>();
        //    string sentemailurl = $"{_httpClient.BaseAddress}/InboxEmail/Generalfilter?UserId={UserId}&from={from}&to={to}&subject={subject}&hasthewords={hasthewords}&type={tab}";
        //    HttpResponseMessage response = _httpClient.GetAsync(sentemailurl).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        dynamic data = response.Content.ReadAsStringAsync().Result;
        //        var dataObject = new { data = new List<InboxEmailModel>() };
        //        var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
        //        sentemailList = response2.data;

        //        if (sentemailList != null)
        //        {
        //            return Json(sentemailList);
        //        }
        //        else
        //        {
        //            var sentemailList1 = new List<InboxEmailModel>();
        //            return Json(sentemailList1);
        //        }
        //    }
        //    return Json(sentemailDataList);
        //}

        //public JsonResult showdetails(string id)
        //{
        //    GetCookies gk = new GetCookies();
        //    CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

        //    ViewBag.Format = CUtility.format;
        //    Guid? UserId = new Guid(CUtility.comid);

        //    var sentclientDataList = new InboxEmailModel();
        //    var sentclientList = new InboxEmailModel();
        //    string sentclienturl = $"{_httpClient.BaseAddress}/InboxEmail/Get?UserId={UserId}&i_id={id}";
        //    HttpResponseMessage response = _httpClient.GetAsync(sentclienturl).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        dynamic data = response.Content.ReadAsStringAsync().Result;
        //        var dataObject = new { data = new InboxEmailModel() };
        //        var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
        //        sentclientList = response2.data;

        //        if (sentclientList != null)
        //        {
        //            return Json(sentclientList);
        //        }
        //        else
        //        {
        //            var sentclientList1 = new List<InboxEmailModel>();
        //            return Json(sentclientList1);
        //        }
        //    }
        //    return Json(sentclientDataList);
        //}

        //public JsonResult Generaltype(string type)
        //{
        //    //if (tab == null)
        //    //{
        //    //    tab = "General";
        //    //}
        //    //  string connectionString = "Server=103.182.153.94,1433;Database=dbCityMarine_UAT;User Id=dbCityMarine_UAT;Password=dbCityMarine_UAT@2024;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;";

        //    GetCookies gk = new GetCookies();
        //    CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

        //    ViewBag.Format = CUtility.format;

        //    Guid? UserId = new Guid(CUtility.userid);
        //    var sentemailDataList = new List<InboxEmailModel>();
        //    var sentemailList = new List<InboxEmailModel>();

        //    string encodedType = Uri.EscapeDataString(type);



        //    //using (SqlConnection conn = new SqlConnection(connectionString))
        //    //{
        //    //    conn.Open();

        //    //    string query = @"INSERT INTO dbo.tbl_testvalues (labelname,value)
        //    //             VALUES (@labelname,@value)";

        //    //    using (SqlCommand cmd1 = new SqlCommand(query, conn))
        //    //    {
        //    //        // Add parameters to the insert query
        //    //        cmd1.Parameters.AddWithValue("@labelname", encodedType);
        //    //        cmd1.Parameters.AddWithValue("@value", type);

        //    //        // Execute the query to insert the email into the database
        //    //        cmd1.ExecuteNonQuery();
        //    //    }
        //    //    conn.Close();
        //    //}


        //    string sentemailurl = $"{_httpClient.BaseAddress}/InboxEmail/Generaltype?UserId={UserId}&i_generaltype={encodedType}&tab=general";
        //    HttpResponseMessage response = _httpClient.GetAsync(sentemailurl).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        dynamic data = response.Content.ReadAsStringAsync().Result;
        //        var dataObject = new { data = new List<InboxEmailModel>() };
        //        var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
        //        sentemailList = response2.data;

        //        if (sentemailList != null)
        //        {
        //            return Json(sentemailList);
        //        }
        //        else
        //        {
        //            var sentemailList1 = new List<InboxEmailModel>();
        //            return Json(sentemailList1);
        //        }
        //    }
        //    return Json(sentemailDataList);
        //}


        //public JsonResult ClaimoneNo( string? tab,  string? claim)
        //{

        //    GetCookies gk = new GetCookies();
        //    CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

        //    ViewBag.Format = CUtility.format;
        //    Guid? UserId = new Guid(CUtility.userid);

        //    var sentemailDataList = new List<InboxEmailModel>();
        //    var sentemailList = new List<InboxEmailModel>();
        //    string sentclienturl = $"{_httpClient.BaseAddress}/InboxEmail/ClaimoneNo?UserId={UserId}&i_type={tab}&i_claimno={claim}";
        //    HttpResponseMessage response = _httpClient.GetAsync(sentclienturl).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        dynamic data = response.Content.ReadAsStringAsync().Result;
        //        var dataObject = new { data = new List<InboxEmailModel>() };
        //        var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
        //        sentemailList = response2.data;

        //        if (sentemailList != null)
        //        {
        //            //return Json(sentemailList);
        //            return Json(new
        //            {
        //                sentemailList
        //            });
        //        }
        //        else
        //        {
        //            var sentemailList1 = new List<InboxEmailModel>();
        //            //return Json(sentemailList1);
        //            return Json(new
        //            {
        //                sentemailList1
        //            });
        //        }
        //    }
        //    //return Json(sentemailDataList);
        //    return Json(new
        //    {
        //        sentemailDataList
        //    });
        //}

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
            string decodedFilePath = WebUtility.UrlDecode(filePath);
            // Ensure the file exists
            if (!System.IO.File.Exists(decodedFilePath))
            {
                return NotFound("File not found.");
            }

            try
            {
                // Get the file name
                var fileName = Path.GetFileName(decodedFilePath);

                // Determine the MIME type based on the file extension
                var contentType = GetMimeType(decodedFilePath);

                // Read the file into a byte array
                var fileBytes = System.IO.File.ReadAllBytes(decodedFilePath);

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






        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            FetchEmails();
        }

        //CityMarine
        private static string tenantId = "26d892b0-3196-4399-ba55-2f2f17cf30c7"; // Azure AD tenant ID
        private static string clientId = "c84beebd-a48c-4aad-b0c1-814fcb7fba17"; // Application (client) ID
        //private static string clientSecret = "F3S8Q~RV_vbrCt-uosG.WTe8UdLth0oQOAVdwcZy"; // Application (client) secret
        private static string clientSecret = "SgD8Q~JmkeRbMMylPskmsM7CyJpL1aABPDJnxcbm"; // Application (client) secret


        private static string authority = $"https://login.microsoftonline.com/{tenantId}";


        private void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt"; ;
            if (!System.IO.File.Exists(filepath))
            {
                using (StreamWriter sw = System.IO.File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = System.IO.File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }


        public static async Task<string> GetAccessTokenAsync()
        {
            try
            {
                var confidentialClient = ConfidentialClientApplicationBuilder.Create(clientId)
                    .WithClientSecret(clientSecret)  // Use the actual client secret value here
                    .WithAuthority(new Uri(authority))
                    .Build();

                var result = await confidentialClient.AcquireTokenForClient(new string[] { "https://graph.microsoft.com/.default" })
                  .ExecuteAsync();


                Console.WriteLine($"Access Token: {result.AccessToken}");
                return result.AccessToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during token acquisition: {ex.Message}");
                throw;
            }
        }


        // private async Task FetchEmails()
        public async Task<IActionResult> FetchEmails()
        {
            try
            {
                GetCookies gk = new GetCookies();
                CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);

                if (string.IsNullOrEmpty(CUtility.comid))
                {
                    // Handle missing session data
                    return RedirectToAction("Index", "CompanyLoginRegistration");
                }

                // Get the access token
                string token = await GetAccessTokenAsync();
                // string userId = "0f5fb42b-eeab-48f5-8345-5e32fa67158e"; // Replace with the correct user ID

                string userId = "";
                // string connectionString = "Server=103.182.153.94,1433;Database=dbCityMarine_UAT;User Id=dbCityMarine_UAT;Password=dbCityMarine_UAT@2024;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;";
                //string connectionString = "Server=EMS\\MSSQLSERVER1;Database=dbCityMarine_UAT;User Id=sa;Password=sql@2025;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;";
                string connectionString = sql;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Query to fetch email rule configuration data
                    //string query1 = @"SELECT userid from dbo.tbl_emsuser";



                    //string query1 = @"select E_Key from tbl_EmailList e join tbl_staff s on s.st_email=e.E_email where st_id=@st_id";
                    string query1 = @"SELECT el.E_key FROM tbl_EmailList el JOIN tbl_employeeemailmgmt em ON el.e_id = em.e_email
WHERE ',' + em.e_employee + ',' LIKE @searchPattern AND EXISTS ( SELECT 1 FROM tbl_staff s WHERE s.st_id = @st_id )";

                    using (SqlCommand cmd = new SqlCommand(query1, conn))
                    {
                        cmd.Parameters.AddWithValue("@st_id", CUtility.userid);
                        cmd.Parameters.AddWithValue("@searchPattern", "%," + CUtility.userid + ",%");
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                userId = reader["E_Key"].ToString();
                                //            }
                                //        }

                                //    }
                                //}
                                // string vemail = "EMS@Citymarinebrokers.com";
                                using (var httpClient = new HttpClient())
                                {
                                    // Set the Authorization header
                                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                    httpClient.Timeout = TimeSpan.FromMinutes(5);

                                    // Fetch all folders to dynamically locate the Sent Items folder
                                    string folderUrl = $"https://graph.microsoft.com/v1.0/users/{userId}/mailFolders";
                                    var folderResponse = await httpClient.GetAsync(folderUrl);

                                    if (!folderResponse.IsSuccessStatusCode)
                                    {
                                        Console.WriteLine($"Error fetching folders: {folderResponse.StatusCode}");
                                        var folderErrorDetails = await folderResponse.Content.ReadAsStringAsync();
                                        Console.WriteLine($"Folder error details: {folderErrorDetails}");
                                        //return Ok;
                                    }

                                    var folderContent = await folderResponse.Content.ReadAsStringAsync();
                                    var folders = JsonConvert.DeserializeObject<GraphApiFolderResponse>(folderContent);

                                    // Get the Sent Items folder ID
                                    var sentFolder = folders.Value.FirstOrDefault(f => f.DisplayName.Equals("Sent Items", StringComparison.OrdinalIgnoreCase));
                                    if (sentFolder == null)
                                    {
                                        Console.WriteLine("Sent Items folder not found.");
                                        // return;
                                    }
                                    var inboxFolder = folders.Value.FirstOrDefault(f => f.DisplayName.Equals("Inbox", StringComparison.OrdinalIgnoreCase));
                                    if (inboxFolder == null)
                                    {
                                        Console.WriteLine("Sent Items folder not found.");
                                        //return;
                                    }
                                    string inboxfolderid = inboxFolder.Id;
                                    string sentFolderId = sentFolder.Id;

                                    string number = "";
                                    string connstring = sql;
                                    using (SqlConnection con = new SqlConnection(connectionString))
                                    {
                                        con.Open();

                                        // Query to fetch email rule configuration data
                                        string query12 = @"SELECT number from dbo.tbl_fetchemailno";

                                        using (SqlCommand cmd2 = new SqlCommand(query12, con))
                                        {
                                            using (SqlDataReader reader2 = cmd2.ExecuteReader())
                                            {
                                                while (reader2.Read())
                                                {

                                                    number = reader2["number"].ToString();
                                                }
                                            }

                                        }
                                    }

                                    string inboxUrl = $"https://graph.microsoft.com/v1.0/users/{userId}/mailFolders/{inboxfolderid}/messages?$expand=attachments&$top={number}";
                                    string sentUrl = $"https://graph.microsoft.com/v1.0/users/{userId}/mailFolders/{sentFolderId}/messages?$expand=attachments&$top={number}";

                                    httpClient.DefaultRequestHeaders.Add("Prefer", "outlook.timezone=\"Asia/Kolkata\"");

                                    //INBOX
                                    // Fetch and process emails from Inbox
                                    var inboxResponse = await httpClient.GetAsync(inboxUrl);
                                    if (inboxResponse.IsSuccessStatusCode)
                                    {
                                        try
                                        {
                                            // Read the response content
                                            var inboxContent = await inboxResponse.Content.ReadAsStringAsync();
                                            Console.WriteLine("Raw Inbox Response: " + inboxContent);

                                            // Deserialize the JSON content into GraphApiEmailResponse
                                            var inboxEmails = JsonConvert.DeserializeObject<GraphApiEmailResponse>(inboxContent);
                                            string time = "";

                                            // Check if emails exist
                                            if (inboxEmails?.Value?.Any() == true)
                                            {
                                                // Filter for unread emails
                                                //var unreadInboxEmails = inboxEmails.Value.Where(email => !email.IsRead).ToList();

                                                //latest email
                                                //var Emails = inboxEmails.Value.OrderByDescending(email => email.ReceivedDateTime).GroupBy(email => email.ReceivedDateTime).FirstOrDefault();

                                                //string connectionString2 = "Server=103.182.153.94,1433;Database=dbCityMarine_UAT;User Id=dbCityMarine_UAT;Password=dbCityMarine_UAT@2024;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;";
                                                //string connectionString2 = "Server=EMS\\MSSQLSERVER1;Database=dbCityMarine_UAT;User Id=sa;Password=sql@2025;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;";
                                                string connectionString2 = sql;

                                                using (SqlConnection conn1 = new SqlConnection(connectionString2))
                                                {
                                                    conn1.Open();
                                                    string query2 = @"SELECT top 1 e_time from tbl_eventlog order by e_time desc";
                                                    using (SqlCommand cmd1 = new SqlCommand(query2, conn1))
                                                    {
                                                        using (SqlDataReader reader1 = cmd1.ExecuteReader())
                                                        {
                                                            // Loop through each row in the result
                                                            while (reader1.Read())
                                                            {
                                                                time = reader1["e_time"].ToString();
                                                            }
                                                        }
                                                    }

                                                }




                                                //today's all emails
                                                //var today = DateTime.UtcNow.Date; // Get today's date in UTC
                                                //var Emails = inboxEmails.Value
                                                //    .Where(email => email.ReceivedDateTime.HasValue && email.ReceivedDateTime.Value.Date == today) // Ensure the value is not null
                                                //    .OrderByDescending(email => email.ReceivedDateTime); // Order by ReceivedDateTime descending

                                                //DateTime startDateTime = DateTime.ParseExact(time, "MM/dd/yy hh:mm:ss tt", CultureInfo.InvariantCulture); // Parse the string to DateTime
                                                //DateTime currentDateTime = DateTime.UtcNow; // Get the current UTC time

                                                //var Emails = inboxEmails.Value
                                                //    .Where(email => email.ReceivedDateTime.HasValue &&
                                                //                    email.ReceivedDateTime.Value >= startDateTime &&
                                                //                    email.ReceivedDateTime.Value <= currentDateTime) // Ensure email ReceivedDateTime is within the range
                                                //    .OrderByDescending(email => email.ReceivedDateTime);






                                                //for local
                                                DateTime startDateTime = DateTime.ParseExact(time, "MM/dd/yy h:mm:ss tt", CultureInfo.InvariantCulture);


                                                //for server
                                                //DateTime startDateTime = DateTime.ParseExact(time, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                                                //DateTime currentDateTime = DateTime.UtcNow.AddHours(-5).AddMinutes(-30);// Get the current UTC time


                                                // Get the current UTC time
                                                DateTime currentUtcTime = DateTime.UtcNow;

                                                // Convert the UTC time to UTC+04:00 (Indian Standard Time)
                                                 TimeZoneInfo istTimeZone1 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                                                //TimeZoneInfo istTimeZone1 = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");


                                                DateTime currentDateTimeInIst = TimeZoneInfo.ConvertTimeFromUtc(currentUtcTime, istTimeZone1);

                                                // Format the converted time to the desired format: "yyyy-MM-dd HH:mm:ss.fff"
                                                string formattedDateTime = currentDateTimeInIst.ToString("yyyy-MM-dd HH:mm:ss.fff");

                                                // Convert the formatted string back to DateTime
                                                DateTime currentDateTime = DateTime.ParseExact(formattedDateTime, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

                                                //DateTime adjustedDateTime = startDateTime.AddHours(-5).AddMinutes(-30);
                                                DateTime adjustedDateTime = startDateTime.AddHours(-4);
                                                //DateTime adjustedDateTime = startDateTime;


                                                //var Emails = inboxEmails.Value
                                                //    .Where(email => email.ReceivedDateTime.HasValue &&
                                                //                    email.ReceivedDateTime.Value >= adjustedDateTime &&
                                                //                    email.ReceivedDateTime.Value <= currentDateTime) // Ensure email ReceivedDateTime is within the range
                                                //    .OrderByDescending(email => email.ReceivedDateTime);


                                                var Emails = inboxEmails.Value;

                                                //foreach (var email1 in Emails)
                                                //{
                                                //    // Assuming ReceivedDateTime is in UTC
                                                //    if (email1.ReceivedDateTime.HasValue)
                                                //    {
                                                //        DateTime receivedDateTimeUtc = email1.ReceivedDateTime.Value;

                                                //        // Convert the UTC time to UTC+05:30 (Indian Standard Time)
                                                //        TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"); // UTC+05:30
                                                //        DateTime receivedDateTimeInIst = TimeZoneInfo.ConvertTimeFromUtc(receivedDateTimeUtc, istTimeZone);

                                                //        // Compare if the email's ReceivedDateTime in IST is within the specified range
                                                //        if (receivedDateTimeInIst >= adjustedDateTime && receivedDateTimeInIst <= currentDateTime)
                                                //        {
                                                if (Emails.Any())
                                                {
                                                    foreach (var email in Emails)
                                                    {
                                                        try
                                                        {
                                                            DateTime receivedDateTimeUtc1 = email.ReceivedDateTime.Value;

                                                            // Convert the UTC time to UTC+05:30 (Indian Standard Time)
                                                            //TimeZoneInfo istTimeZone1 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"); // UTC+05:30
                                                            DateTime receivedDateTimeInIst1 = TimeZoneInfo.ConvertTimeFromUtc(receivedDateTimeUtc1, istTimeZone1);
                                                            email.ReceivedDateTime = receivedDateTimeInIst1;

                                                            if (receivedDateTimeInIst1 >= adjustedDateTime && receivedDateTimeInIst1 <= currentDateTime)
                                                            {
                                                                InboxEmail(email, userId);
                                                            }



                                                            // Mark email as read after processing
                                                            // await MarkEmailAsRead(httpClient, userId, email.Id);

                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine($"Error processing email with ID {email.Id}: {ex.Message}");
                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    Console.WriteLine("No unread emails found in Inbox.");
                                                }
                                                //        }
                                                //    }
                                                //}







                                                // var Emails = inboxEmails.Value;


                                                //foreach (var email1 in Emails)
                                                //{
                                                //    // Assuming ReceivedDateTime is in UTC
                                                //    if (email1.ReceivedDateTime.HasValue)
                                                //    {
                                                //        DateTime receivedDateTimeUtc = email1.ReceivedDateTime.Value;

                                                //        // Convert the UTC time to UTC+05:30 (Indian Standard Time)
                                                //        TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"); // UTC+05:30
                                                //        DateTime receivedDateTimeInIst = TimeZoneInfo.ConvertTimeFromUtc(receivedDateTimeUtc, istTimeZone);

                                                //        // Compare if the email's ReceivedDateTime in IST is within the specified range
                                                //        if (receivedDateTimeInIst >= adjustedDateTime && receivedDateTimeInIst <= currentDateTime)
                                                //        {
                                                //            if (Emails.Any())
                                                //            {
                                                //                foreach (var email in Emails)
                                                //                {
                                                //                    try
                                                //                    {
                                                //                        DateTime receivedDateTimeUtc1 = email.ReceivedDateTime.Value;

                                                //                        // Convert the UTC time to UTC+05:30 (Indian Standard Time)
                                                //                        //TimeZoneInfo istTimeZone1 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"); // UTC+05:30
                                                //                        DateTime receivedDateTimeInIst1 = TimeZoneInfo.ConvertTimeFromUtc(receivedDateTimeUtc1, istTimeZone1);
                                                //                        email.ReceivedDateTime = receivedDateTimeInIst1;
                                                //                        // Process unread Inbox email
                                                //                        InboxEmail(email, userId);

                                                //                        // Mark email as read after processing
                                                //                        // await MarkEmailAsRead(httpClient, userId, email.Id);

                                                //                    }
                                                //                    catch (Exception ex)
                                                //                    {
                                                //                        WriteToFile("catch");
                                                //                        Console.WriteLine($"Error processing email with ID {email.Id}: {ex.Message}");
                                                //                    }
                                                //                }

                                                //            }
                                                //            else
                                                //            {
                                                //                WriteToFile("noemail");
                                                //                Console.WriteLine("No unread emails found in Inbox.");
                                                //            }
                                                //        }
                                                //    }
                                                //}






                                                using (SqlConnection conn1 = new SqlConnection(connectionString2))
                                                {
                                                    conn1.Open();

                                                    string query = @"INSERT INTO dbo.tbl_time (startdate, enddate, emaildate)
                                                        VALUES (@startdate, @enddate, @emaildate)";

                                                    using (SqlCommand cmd1 = new SqlCommand(query, conn1))
                                                    {
                                                        // Add parameters to the insert query
                                                        cmd1.Parameters.AddWithValue("@startdate", startDateTime);
                                                        cmd1.Parameters.AddWithValue("@enddate", currentDateTime);
                                                        cmd1.Parameters.AddWithValue("@emaildate", "Success");


                                                        // Execute the query to insert the email into the database
                                                        cmd1.ExecuteNonQuery();
                                                    }
                                                    conn1.Close();
                                                }





                                                //       if (Emails.Any())
                                                //       {
                                                //           foreach (var email in Emails)
                                                //           {
                                                //               try
                                                //               {
                                                //                   // Process unread Inbox email
                                                //                   InboxEmail(email, userId);

                                                //                   // Mark email as read after processing
                                                //                   // await MarkEmailAsRead(httpClient, userId, email.Id);



                                                //               }
                                                //               catch (Exception ex)
                                                //               {
                                                //                   Console.WriteLine($"Error processing email with ID {email.Id}: {ex.Message}");
                                                //               }
                                                //           }
                                                ////           string connectionString1 = "Server=103.182.153.94,1433;Database=dbCityMarine_UAT;User Id=dbCityMarine_UAT;Password=dbCityMarine_UAT@2024;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;";

                                                ////           using (SqlConnection conn = new SqlConnection(connectionString1))
                                                ////           {
                                                ////               conn.Open();

                                                ////               string query = @"INSERT INTO dbo.tbl_eventlog (e_time, e_source, e_status)
                                                ////VALUES (@e_time, @e_source, @e_status)";

                                                ////               using (SqlCommand cmd1 = new SqlCommand(query, conn))
                                                ////               {
                                                ////                   // Add parameters to the insert query
                                                ////                   cmd1.Parameters.AddWithValue("@e_time", System.DateTime.Now);
                                                ////                   cmd1.Parameters.AddWithValue("@e_source", "Log");
                                                ////                   cmd1.Parameters.AddWithValue("@e_status", "Success");


                                                ////                   // Execute the query to insert the email into the database
                                                ////                   cmd1.ExecuteNonQuery();
                                                ////               }
                                                ////               conn.Close();
                                                ////           }
                                                //       }
                                                //       else
                                                //       {
                                                //           Console.WriteLine("No unread emails found in Inbox.");
                                                //       }
                                            }
                                            else
                                            {
                                                Console.WriteLine("No emails found in Inbox.");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Error processing inbox response: {ex.Message}");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Failed to retrieve inbox emails. Status Code: {inboxResponse.StatusCode}");
                                    }

                                    //SENT
                                    // Fetch and process emails from Sent Items
                                    var sentResponse = await httpClient.GetAsync(sentUrl);
                                    if (sentResponse.IsSuccessStatusCode)
                                    {
                                        try
                                        {
                                            var sentContent = await sentResponse.Content.ReadAsStringAsync();
                                            var sentEmails = JsonConvert.DeserializeObject<GraphApiEmailResponse>(sentContent);

                                            string time = "";

                                            if (sentEmails?.Value?.Any() == true)
                                            {
                                                // Filter for unread emails
                                                //var unreadEmails = sentEmails.Value.Where(email => email.IsRead == false).ToList();
                                                //var unreadEmails = sentEmails.Value.ToList(); // Simply take all emails


                                                //latest email
                                                //var Emails = sentEmails.Value.OrderByDescending(email => email.ReceivedDateTime).GroupBy(email => email.ReceivedDateTime).FirstOrDefault();

                                                //today's all emails
                                                //var today = DateTime.UtcNow.Date; // Get today's date in UTC
                                                //var Emails = sentEmails.Value
                                                //    .Where(email => email.ReceivedDateTime.HasValue && email.ReceivedDateTime.Value.Date == today) // Ensure the value is not null
                                                //    .OrderByDescending(email => email.ReceivedDateTime); // Order by ReceivedDateTime descending
                                                // string connectionString2 = "Server=103.182.153.94,1433;Database=dbCityMarine_UAT;User Id=dbCityMarine_UAT;Password=dbCityMarine_UAT@2024;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;";
                                                //string connectionString2 = "Server=EMS\\MSSQLSERVER1;Database=dbCityMarine_UAT;User Id=sa;Password=sql@2025;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;";
                                                string connectionString2 = sql;

                                                using (SqlConnection conn1 = new SqlConnection(connectionString2))
                                                {
                                                    conn1.Open();
                                                    string query2 = @"SELECT top 1 e_time from tbl_eventlog order by e_time desc";
                                                    using (SqlCommand cmd1 = new SqlCommand(query2, conn1))
                                                    {
                                                        using (SqlDataReader reader1 = cmd1.ExecuteReader())
                                                        {
                                                            // Loop through each row in the result
                                                            while (reader1.Read())
                                                            {
                                                                time = reader1["e_time"].ToString();
                                                            }
                                                        }
                                                    }

                                                }
                                                //for server
                                               // DateTime startDateTime = DateTime.ParseExact(time, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);


                                                //for local
                                                DateTime startDateTime = DateTime.ParseExact(time, "MM/dd/yy h:mm:ss tt", CultureInfo.InvariantCulture);

                                                //DateTime currentDateTime = DateTime.UtcNow.AddHours(-5).AddMinutes(-30);// Get the current UTC time


                                                // Get the current UTC time
                                                DateTime currentUtcTime = DateTime.UtcNow;

                                                // Convert the UTC time to UTC+05:30 (Indian Standard Time)
                                                 TimeZoneInfo istTimeZone1 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                                                //TimeZoneInfo istTimeZone1 = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");
                                                DateTime currentDateTimeInIst = TimeZoneInfo.ConvertTimeFromUtc(currentUtcTime, istTimeZone1);

                                                // Format the converted time to the desired format: "yyyy-MM-dd HH:mm:ss.fff"
                                                string formattedDateTime = currentDateTimeInIst.ToString("yyyy-MM-dd HH:mm:ss.fff");

                                                // Convert the formatted string back to DateTime
                                                DateTime currentDateTime = DateTime.ParseExact(formattedDateTime, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

                                                //DateTime adjustedDateTime = startDateTime.AddHours(-5).AddMinutes(-30);
                                                DateTime adjustedDateTime = startDateTime.AddHours(-4);
                                                // DateTime adjustedDateTime = startDateTime;


                                                //var Emails = inboxEmails.Value
                                                //    .Where(email => email.ReceivedDateTime.HasValue &&
                                                //                    email.ReceivedDateTime.Value >= adjustedDateTime &&
                                                //                    email.ReceivedDateTime.Value <= currentDateTime) // Ensure email ReceivedDateTime is within the range
                                                //    .OrderByDescending(email => email.ReceivedDateTime);


                                                var Emails = sentEmails.Value;


                                                if (Emails.Any())
                                                {
                                                    foreach (var email in Emails)
                                                    {
                                                        try
                                                        {
                                                            DateTime receivedDateTimeUtc1 = email.ReceivedDateTime.Value;

                                                            // Convert the UTC time to UTC+05:30 (Indian Standard Time)
                                                            //TimeZoneInfo istTimeZone1 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"); // UTC+05:30
                                                            DateTime receivedDateTimeInIst1 = TimeZoneInfo.ConvertTimeFromUtc(receivedDateTimeUtc1, istTimeZone1);
                                                            email.ReceivedDateTime = receivedDateTimeInIst1;

                                                            if (receivedDateTimeInIst1 >= adjustedDateTime && receivedDateTimeInIst1 <= currentDateTime)
                                                            {
                                                                SentEmail(email, userId);
                                                            }



                                                            // Mark email as read after processing
                                                            // await MarkEmailAsRead(httpClient, userId, email.Id);

                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine($"Error processing email with ID {email.Id}: {ex.Message}");
                                                        }


                                                    }


                                                }
                                                else
                                                {
                                                    Console.WriteLine("No unread emails found in Sent Items.");
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("No emails found in Sent Items.");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Error processing inbox response: {ex.Message}");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Failed to retrieve inbox emails. Status Code: {sentResponse.StatusCode}");
                                    }
                                }

                                //SEPTEMBER 01 2025 START


                            }
                        }

                    }
                }
                //SEPTEMBER 01 2025 END
                //string connectionString1 = "Server=103.182.153.94,1433;Database=dbCityMarine_UAT;User Id=dbCityMarine_UAT;Password=dbCityMarine_UAT@2024;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;";
                //string connectionString1 = "Server=EMS\\MSSQLSERVER1;Database=dbCityMarine_UAT;User Id=sa;Password=sql@2025;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;";
                string connectionString1 = sql;

                using (SqlConnection conn1 = new SqlConnection(connectionString1))
                {
                    conn1.Open();

                    string query = @"INSERT INTO dbo.tbl_eventlog (e_actualtime,e_time, e_source, e_status)
                         VALUES (@e_actualtime,@e_time, @e_source, @e_status)";



                    DateTime startUtcTime = DateTime.UtcNow;
                    TimeZoneInfo istTimeZone1 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    //TimeZoneInfo istTimeZone1 = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");
                    DateTime startDateTimeInIst = TimeZoneInfo.ConvertTimeFromUtc(startUtcTime, istTimeZone1);

                    // Format the converted time to the desired format: "yyyy-MM-dd HH:mm:ss.fff"
                    string formattedDateTime = startDateTimeInIst.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    // Convert the formatted string back to DateTime
                    DateTime startDateTime = DateTime.ParseExact(formattedDateTime, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);






                    using (SqlCommand cmd1 = new SqlCommand(query, conn1))
                    {
                        // Add parameters to the insert query
                        cmd1.Parameters.AddWithValue("@e_actualtime", System.DateTime.Now);
                        //  cmd1.Parameters.AddWithValue("@e_time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        cmd1.Parameters.AddWithValue("@e_time", startDateTime);
                        cmd1.Parameters.AddWithValue("@e_source", "Log");
                        cmd1.Parameters.AddWithValue("@e_status", "Success");


                        // Execute the query to insert the email into the database
                        cmd1.ExecuteNonQuery();
                    }
                    conn1.Close();
                }





            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FetchEmails: {ex.Message}");
            }

            return RedirectToAction("Index");
        }

        private async Task InboxEmail(GraphApiEmailResponse.GraphApiMessage email, string userId)
        {
            try
            {
                // Extract email details with null checks
                string subject = email.Subject ?? string.Empty;
                string from = email.From?.EmailAddress?.Address ?? string.Empty;
                string to = string.Join(", ", email.ToRecipients?.Select(r => r.EmailAddress?.Address) ?? new List<string>());
                string body = email.Body?.Content ?? string.Empty;
                string inReplyTo = email.InReplyToId ?? string.Empty;
                string messageId = email.Id ?? string.Empty;
                DateTime receivedDate = email.ReceivedDateTime ?? DateTime.MinValue;
                string emailType = "General";

                // Create an instance of AttachmentSaver to save attachments
                var attachmentSaver = new AttachmentSaver();

                // Await the asynchronous SaveAttachments method

                using (HttpClient httpClient = new HttpClient())
                {
                    // Create an instance of AttachmentSaver to save attachments
                    // Pass the HttpClient instance to the SaveAttachments method
                    string attachmentPath = await attachmentSaver.SaveAttachments(email, userId, httpClient);

                    string connectionString = sql;

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        bool exists = false;
                        //string queryy = @"Select i_messageid from dbo.tbl_InboxEmail";
                        string queryy = @"SELECT 1 FROM dbo.tbl_InboxEmail WHERE i_messageid = @i_messageid";
                        using (SqlCommand cmdd = new SqlCommand(queryy, conn))
                        {
                            cmdd.Parameters.AddWithValue("@i_messageid", messageId);



                            using (SqlDataReader reader = cmdd.ExecuteReader())
                            {

                                if (reader.HasRows)
                                {
                                    exists = true;
                                }
                                // Loop through each row in the result
                                //while (reader.Read())
                                //{
                                //    string i_messageid = reader["i_messageid"].ToString();

                                //    if (i_messageid != messageId)
                                //    {



                                //        // Insert into the database
                                //        InsertInboxEmailToDatabase(subject, from, to, body, inReplyTo, messageId, receivedDate, attachmentPath, emailType);

                                //    }
                                //}
                            }
                        }

                        if (!exists)
                        {
                            InsertInboxEmailToDatabase(subject, from, to, body, inReplyTo, messageId, receivedDate, attachmentPath, emailType);
                        }
                    }






                    // Log success
                    WriteToFile($"Email processed successfully: {subject}");
                }
                // Insert into the database

            }
            catch (Exception ex)
            {
                // Log the error with details
                WriteToFile($"Error processing email (ID: {email?.Id ?? "Unknown"}): {ex.Message}");
            }
        }
        private void InsertInboxEmailToDatabase(string subject, string from, string to, string body, string inReplyTo, string messageId, DateTime receivedDate, string attachmentPath, string emailType)
        {
            //string connectionString = "Server=P3NWPLSK12SQL-v13.shr.prod.phx3.secureserver.net;Database=CityMarineMgmt;User Id=CityMarineMgmt;Password=bZl34u0^6;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;";
            //string connectionString = "Server=103.182.153.94,1433;Database=dbCityMarine_UAT;User Id=dbCityMarine_UAT;Password=dbCityMarine_UAT@2024;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;";
            //string connectionString = "Server=EMS\\MSSQLSERVER1;Database=dbCityMarine_UAT;User Id=sa;Password=sql@2025;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;";
            string connectionString = sql;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Query to fetch email rule configuration data
                string query1 = @"SELECT E_id, pv1.pv_parametervalue as E_parameterName, pv2.pv_parametervalue as E_conditionName,
                                pv3.pv_parametervalue as E_categoryName, E_category, E_value, E_parameter, E_condition,
                                E_createdby, E_updatedby, E_updateddate, E_createddate, E_isactive 
                          FROM [dbo].[tbl_EmailRuleConfg] ec
                          JOIN tbl_parametervaluemaster pv1 ON pv1.pv_id = ec.E_parameter
                          JOIN tbl_parametervaluemaster pv2 ON pv2.pv_id = ec.E_condition
                          JOIN tbl_parametervaluemaster pv3 ON pv3.pv_id = ec.E_category 
                          WHERE E_isactive = '1'";




                using (SqlCommand cmd = new SqlCommand(query1, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Loop through each row in the result
                        while (reader.Read())
                        {
                            string E_parameterName = reader["E_parameterName"].ToString();
                            string E_conditionName = reader["E_conditionName"].ToString();
                            string E_categoryName = reader["E_categoryName"].ToString();
                            string E_value = reader["E_value"].ToString();

                            if (E_parameterName == "Subject")
                            {
                                if (E_conditionName == "Contains")
                                {
                                    //if (subject.Contains(E_value))
                                    if (subject.IndexOf(E_value, StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        emailType = E_categoryName;
                                    }
                                }
                                else if (E_conditionName == "Begin With")
                                {
                                    if (subject.StartsWith(E_value, StringComparison.OrdinalIgnoreCase))
                                    {
                                        emailType = E_categoryName;
                                    }
                                }
                                else if (E_conditionName == "Equal To")
                                {
                                    if (subject.Equals(E_value, StringComparison.OrdinalIgnoreCase))
                                    {
                                        emailType = E_categoryName;
                                    }
                                }

                            }
                            else if (E_parameterName == "Domain")
                            {
                                if (E_conditionName == "Contains")
                                {
                                    //if (subject.Contains(E_value))
                                    if (subject.IndexOf(E_value, StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        emailType = E_categoryName;
                                    }
                                }
                                else if (E_conditionName == "Begin With")
                                {
                                    if (subject.StartsWith(E_value, StringComparison.OrdinalIgnoreCase))
                                    {
                                        emailType = E_categoryName;
                                    }
                                }
                                else if (E_conditionName == "Equal To")
                                {
                                    if (subject.Equals(E_value, StringComparison.OrdinalIgnoreCase))
                                    {
                                        emailType = E_categoryName;
                                    }
                                }
                            }
                        }
                    }
                }


                //string query2 = @"SELECT COUNT(*) FROM [dbo].[tbl_customermaster] WHERE SUBSTRING(c_email, CHARINDEX('@', c_email) + 1, LEN(c_email)) = @Email";
                string query2 = @"SELECT COUNT(*) FROM [dbo].[tbl_customermaster] WHERE EXISTS ( SELECT 1 FROM STRING_SPLIT(c_emaildomain, ',') AS email
                WHERE RIGHT(email.value, LEN(email.value) - CHARINDEX('@', email.value)) =  @Email);";
                string domain = from.Substring(from.IndexOf('@') + 1);

                using (SqlCommand cmd2 = new SqlCommand(query2, conn))
                {
                    cmd2.Parameters.AddWithValue("@Email", domain);

                    // Execute the query and get the count
                    int count = (int)cmd2.ExecuteScalar();

                    if (count == 0)
                    {
                        emailType = "General";
                    }
                }

                //string queryy = @"Select i_messageid from dbo.tbl_InboxEmail";
                //using (SqlCommand cmdd = new SqlCommand(queryy, conn))
                //{
                //    using (SqlDataReader reader = cmdd.ExecuteReader())
                //    {
                //        // Loop through each row in the result
                //        while (reader.Read())
                //        {
                //            string i_messageid = reader["i_messageid"].ToString();

                //            if (i_messageid != messageId)
                //            {
                string query = @"INSERT INTO dbo.tbl_InboxEmail (i_subject, i_from, i_to, i_body, i_replyto, i_messageid, i_receiveddate, i_attachment, i_type)
                         VALUES (@i_subject, @i_from, @i_to, @i_body, @i_replyto, @i_messageid, @i_receiveddate, @i_attachment, @i_type)";

                using (SqlCommand cmd1 = new SqlCommand(query, conn))
                {
                    // Add parameters to the insert query
                    cmd1.Parameters.AddWithValue("@i_subject", subject);
                    cmd1.Parameters.AddWithValue("@i_from", from);
                    cmd1.Parameters.AddWithValue("@i_to", to);
                    cmd1.Parameters.AddWithValue("@i_body", body);
                    cmd1.Parameters.AddWithValue("@i_replyto", inReplyTo);
                    cmd1.Parameters.AddWithValue("@i_messageid", messageId);
                    cmd1.Parameters.AddWithValue("@i_receiveddate", receivedDate);
                    cmd1.Parameters.AddWithValue("@i_attachment", attachmentPath);
                    cmd1.Parameters.AddWithValue("@i_type", emailType);

                    // Execute the query to insert the email into the database
                    cmd1.ExecuteNonQuery();
                }

                // }
                //  }
                //   }
                // }




                // Insert email data into the tbl_InboxEmail table
                //string query = @"INSERT INTO dbo.tbl_InboxEmail (i_subject, i_from, i_to, i_body, i_replyto, i_messageid, i_receiveddate, i_attachment, i_type)
                //     VALUES (@i_subject, @i_from, @i_to, @i_body, @i_replyto, @i_messageid, @i_receiveddate, @i_attachment, @i_type)";

                //using (SqlCommand cmd1 = new SqlCommand(query, conn))
                //{
                //    // Add parameters to the insert query
                //    cmd1.Parameters.AddWithValue("@i_subject", subject);
                //    cmd1.Parameters.AddWithValue("@i_from", from);
                //    cmd1.Parameters.AddWithValue("@i_to", to);
                //    cmd1.Parameters.AddWithValue("@i_body", body);
                //    cmd1.Parameters.AddWithValue("@i_replyto", inReplyTo);
                //    cmd1.Parameters.AddWithValue("@i_messageid", messageId);
                //    cmd1.Parameters.AddWithValue("@i_receiveddate", receivedDate);
                //    cmd1.Parameters.AddWithValue("@i_attachment", attachmentPath);
                //    cmd1.Parameters.AddWithValue("@i_type", emailType);

                //    // Execute the query to insert the email into the database
                //    cmd1.ExecuteNonQuery();
                //}







            }
        }

        private async Task SentEmail(GraphApiEmailResponse.GraphApiMessage email, string userId)
        {
            try
            {
                // Extract email details with null checks
                string subject = email.Subject ?? string.Empty;
                string from = email.From?.EmailAddress?.Address ?? string.Empty;
                string to = string.Join(", ", email.ToRecipients?.Select(r => r.EmailAddress?.Address) ?? new List<string>());
                string body = email.Body?.Content ?? string.Empty;
                string inReplyTo = email.InReplyToId ?? string.Empty;
                string messageId = email.Id ?? string.Empty;
                DateTime sentDate = email.ReceivedDateTime ?? DateTime.MinValue; // Use appropriate sent date field if available
                string emailType = "General";

                // Process attachments if available
                var attachmentSaver = new AttachmentSaver();

                using (HttpClient httpClient = new HttpClient())
                {
                    // Save attachments
                    string attachmentPath = await attachmentSaver.SaveAttachments1(email, userId, httpClient);

                    // Insert email into the database along with attachments



                    string connectionString = sql;

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        bool exists = false;
                        //string queryy = @"Select i_messageid from dbo.tbl_InboxEmail";
                        string queryy = @"SELECT 1 FROM dbo.tbl_SentEmail WHERE s_messageid = @s_messageid";
                        using (SqlCommand cmdd = new SqlCommand(queryy, conn))
                        {
                            cmdd.Parameters.AddWithValue("@s_messageid", messageId);



                            using (SqlDataReader reader = cmdd.ExecuteReader())
                            {

                                if (reader.HasRows)
                                {
                                    exists = true;
                                }
                            }
                        }

                        if (!exists)
                        {
                            InsertSentEmailToDatabase(subject, from, to, body, inReplyTo, messageId, sentDate, attachmentPath, emailType);
                        }
                    }

                    // Log success
                    WriteToFile($"Sent email processed successfully: {subject} with attachments.");
                }

            }
            catch (Exception ex)
            {
                // Log the error with details
                // string emailId = email?.Id ?? "Unknown";
                WriteToFile($"Error processing sent email (ID: {email}): {ex.Message}");
            }
        }


        private void InsertSentEmailToDatabase(string subject, string from, string to, string body, string inReplyTo, string messageId, DateTime sendDate, string attachmentPath, string sType)
        {
            //string connectionString = "Server=103.182.153.94,1433;Database=dbCityMarine_UAT;User Id=dbCityMarine_UAT;Password=dbCityMarine_UAT@2024;Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False;";
            string connectionString = sql;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Query to fetch email rules
                string query1 = @"SELECT E_id, pv1.pv_parametervalue as E_parameterName, pv2.pv_parametervalue as E_conditionName,
                                pv3.pv_parametervalue as E_categoryName, E_category, E_value, E_parameter, E_condition,
                                E_createdby, E_updatedby, E_updateddate, E_createddate, E_isactive 
                          FROM [dbo].[tbl_EmailRuleConfg] ec
                          JOIN tbl_parametervaluemaster pv1 ON pv1.pv_id = ec.E_parameter
                          JOIN tbl_parametervaluemaster pv2 ON pv2.pv_id = ec.E_condition
                          JOIN tbl_parametervaluemaster pv3 ON pv3.pv_id = ec.E_category 
                          WHERE E_isactive = '1'";




                using (SqlCommand cmd = new SqlCommand(query1, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Loop through each row in the result
                        while (reader.Read())
                        {
                            string E_parameterName = reader["E_parameterName"].ToString();
                            string E_conditionName = reader["E_conditionName"].ToString();
                            string E_categoryName = reader["E_categoryName"].ToString();
                            string E_value = reader["E_value"].ToString();

                            if (E_parameterName == "Subject")
                            {
                                if (E_conditionName == "Contains")
                                {
                                    //if (subject.Contains(E_value))
                                    if (subject.IndexOf(E_value, StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        sType = E_categoryName;
                                    }
                                }
                                else if (E_conditionName == "Begin With")
                                {
                                    if (subject.StartsWith(E_value, StringComparison.OrdinalIgnoreCase))
                                    {
                                        sType = E_categoryName;
                                    }
                                }
                                else if (E_conditionName == "Equal To")
                                {
                                    if (subject.Equals(E_value, StringComparison.OrdinalIgnoreCase))
                                    {
                                        sType = E_categoryName;
                                    }
                                }

                            }
                            else if (E_parameterName == "Domain")
                            {
                                if (E_conditionName == "Contains")
                                {
                                    //if (subject.Contains(E_value))
                                    if (subject.IndexOf(E_value, StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        sType = E_categoryName;
                                    }
                                }
                                else if (E_conditionName == "Begin With")
                                {
                                    if (subject.StartsWith(E_value, StringComparison.OrdinalIgnoreCase))
                                    {
                                        sType = E_categoryName;
                                    }
                                }
                                else if (E_conditionName == "Equal To")
                                {
                                    if (subject.Equals(E_value, StringComparison.OrdinalIgnoreCase))
                                    {
                                        sType = E_categoryName;
                                    }
                                }
                            }
                        }
                    }
                }


                //string query2 = @"SELECT COUNT(*) FROM [dbo].[tbl_customermaster] WHERE SUBSTRING(c_email, CHARINDEX('@', c_email) + 1, LEN(c_email)) = @Email";

                //// string domain = from.Substring(from.IndexOf('@') + 1);
                //using (SqlCommand cmd2 = new SqlCommand(query2, conn))
                //{
                //    cmd2.Parameters.AddWithValue("@Email", from);

                //    // Execute the query and get the count
                //    int count = (int)cmd2.ExecuteScalar();

                //    if (count == 0)
                //    {
                //        sType = "General";
                //    }
                //}

                string query2 = @"SELECT COUNT(*) FROM [dbo].[tbl_customermaster] WHERE SUBSTRING(c_email, CHARINDEX('@', c_email) + 1, LEN(c_email)) = @Email";
                string domain = to.Substring(to.IndexOf('@') + 1);

                using (SqlCommand cmd2 = new SqlCommand(query2, conn))
                {
                    cmd2.Parameters.AddWithValue("@Email", domain);

                    // Execute the query and get the count
                    int count = (int)cmd2.ExecuteScalar();

                    if (count == 0)
                    {
                        sType = "General";
                    }
                }


                // Insert email into SentEmail table
                string insertQuery = @"INSERT INTO dbo.tbl_SentEmail (s_subject,s_from,s_to,s_body,s_replyto,s_messageid,s_sentdate,s_attachment,s_type) 
            VALUES (@s_subject,@s_from,@s_to,@s_body,@s_replyto,@s_messageid,@s_sentdate,@s_attachment,@s_type)";

                using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@s_subject", subject);
                    insertCmd.Parameters.AddWithValue("@s_from", from);
                    insertCmd.Parameters.AddWithValue("@s_to", to);
                    insertCmd.Parameters.AddWithValue("@s_body", body);
                    insertCmd.Parameters.AddWithValue("@s_replyto", inReplyTo);
                    insertCmd.Parameters.AddWithValue("@s_messageid", messageId);
                    insertCmd.Parameters.AddWithValue("@s_sentdate", sendDate);
                    insertCmd.Parameters.AddWithValue("@s_attachment", attachmentPath);
                    insertCmd.Parameters.AddWithValue("@s_type", sType);

                    insertCmd.ExecuteNonQuery();
                }
            }
        }

        //public class AttachmentSaver
        //{

        //    //Inbox
        //    public async Task<string> SaveAttachments(GraphApiEmailResponse.GraphApiMessage email, string userId, HttpClient httpClient)
        //    {
        //        List<string> filePaths = new List<string>();
        //        string attachmentpath = "CityMarine_EmailCRM/wwwroot/Email_Attachment";

        //        // Ensure email has attachments
        //        if (email.Attachments != null && email.Attachments.Count > 0)
        //        {
        //            foreach (var attachment in email.Attachments)
        //            {
        //                string senderEmail = email.From?.EmailAddress?.Address ?? "UnknownSender";
        //                string attachmentsFolder = Path.Combine(attachmentpath, senderEmail);

        //                // Ensure the folder exists
        //                if (!Directory.Exists(attachmentsFolder))
        //                {
        //                    Directory.CreateDirectory(attachmentsFolder);
        //                }

        //                string fileName = SanitizeFileName(attachment.Name);
        //                string filePath = Path.Combine(attachmentsFolder, fileName);

        //                // If the attachment is of type 'GraphApiAttachment'
        //                if (attachment is GraphApiAttachment fileAttachment)
        //                {
        //                    try
        //                    {
        //                        // Check if the attachment has a contentUrl or contentBytes
        //                        if (!string.IsNullOrEmpty(fileAttachment.ContentUrl))
        //                        {
        //                            // Download the attachment using its contentUrl
        //                            await DownloadAttachmentFromUrl(fileAttachment.ContentUrl, filePath, httpClient);
        //                        }
        //                        else if (!string.IsNullOrEmpty(fileAttachment.ContentBytes))
        //                        {
        //                            // Decode and save attachment from Base64 contentBytes
        //                            byte[] content = Convert.FromBase64String(fileAttachment.ContentBytes);
        //                            System.IO.File.WriteAllBytes(filePath, content);
        //                        }

        //                        filePaths.Add(filePath);
        //                        Console.WriteLine($"Attachment saved: {filePath}");
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine($"Error downloading attachment {fileAttachment.Name}: {ex.Message}");
        //                    }
        //                }
        //                else
        //                {
        //                    Console.WriteLine($"Attachment {attachment.Name} is not a file attachment.");
        //                }
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("No attachments found.");
        //            return "No attachments available.";
        //        }

        //        return string.Join(Environment.NewLine, filePaths);
        //    }


        //    private async Task DownloadAttachmentFromUrl(string contentUrl, string filePath, HttpClient httpClient)
        //    {
        //        var response = await httpClient.GetAsync(contentUrl);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var content = await response.Content.ReadAsByteArrayAsync();
        //            System.IO.File.WriteAllBytes(filePath, content);
        //        }
        //        else
        //        {
        //            Console.WriteLine($"Failed to download attachment from URL: {contentUrl}");
        //        }
        //    }

        //    private string SanitizeFileName(string fileName)
        //    {

        //        return string.Concat(fileName.Split(Path.GetInvalidFileNameChars()));
        //    }
        //    //Send
        //    public async Task<string> SaveAttachments1(GraphApiEmailResponse.GraphApiMessage email, string userId, HttpClient httpClient)
        //    {
        //        List<string> filePaths = new List<string>();
        //        string attachmentpath = "CityMarine_EmailCRM/wwwroot/Email_Attachment";

        //        // Ensure email has attachments
        //        if (email.Attachments != null && email.Attachments.Any())
        //        {
        //            foreach (var attachment in email.Attachments)
        //            {
        //                string senderEmail = email.From?.EmailAddress?.Address ?? "UnknownSender";
        //                string attachmentsFolder = Path.Combine(attachmentpath, senderEmail);

        //                // Ensure the folder exists
        //                if (!Directory.Exists(attachmentsFolder))
        //                {
        //                    Directory.CreateDirectory(attachmentsFolder);
        //                }

        //                string fileName = SanitizeFileName(attachment.Name);
        //                string filePath = Path.Combine(attachmentsFolder, fileName);

        //                try
        //                {
        //                    // Check if the attachment is of type 'GraphApiAttachment'
        //                    if (attachment is GraphApiEmailResponse.GraphApiAttachment graphAttachment)
        //                    {
        //                        // If the attachment has contentBytes (Base64 encoded)
        //                        if (!string.IsNullOrEmpty(graphAttachment.ContentBytes))
        //                        {
        //                            byte[] content = Convert.FromBase64String(graphAttachment.ContentBytes);
        //                            System.IO.File.WriteAllBytes(filePath, content);
        //                        }
        //                        // If the attachment has a contentUrl, download the file
        //                        else if (!string.IsNullOrEmpty(graphAttachment.ContentUrl))
        //                        {
        //                            await DownloadAttachmentFromUrl1(graphAttachment.ContentUrl, filePath, httpClient);
        //                        }

        //                        filePaths.Add(filePath);
        //                        Console.WriteLine($"Attachment saved: {filePath}");
        //                    }
        //                    else
        //                    {
        //                        Console.WriteLine($"Attachment {attachment.Name} is not a file attachment.");
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine($"Error processing attachment {attachment.Name}: {ex.Message}");
        //                }
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("No attachments found.");
        //            return "No attachments available.";
        //        }

        //        return string.Join(Environment.NewLine, filePaths);
        //    }

        //    public async Task DownloadAttachmentFromUrl1(string contentUrl, string filePath, HttpClient httpClient)
        //    {
        //        try
        //        {
        //            // Get the attachment content from the URL
        //            var response = await httpClient.GetAsync(contentUrl);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                var content = await response.Content.ReadAsByteArrayAsync();
        //                System.IO.File.WriteAllBytes(filePath, content);
        //                Console.WriteLine($"Attachment downloaded: {filePath}");
        //            }
        //            else
        //            {
        //                Console.WriteLine($"Failed to download attachment from {contentUrl}");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Error downloading attachment: {ex.Message}");
        //        }
        //    }
        //}

        public class AttachmentSaver
        {

            public async Task<string> SaveAttachments(GraphApiEmailResponse.GraphApiMessage email, string userId, HttpClient httpClient)
            {
                List<string> filePaths = new List<string>();
                List<Task> downloadTasks = new List<Task>();
                string attachmentpath = emailconfig;
                //string attachmentpath = ConfigurationManager.AppSettings["attachmentpath"];

                if (email.Attachments != null && email.Attachments.Count > 0)
                {
                    foreach (var attachment in email.Attachments)
                    {
                        string senderEmail = email.From?.EmailAddress?.Address ?? "UnknownSender";
                        string Id = email.Id ?? "UnknownSender1";
                        string lastId = Id.Length > 13 ? Id.Substring(Id.Length - 13) : Id;
                        string attachmentsFolder = Path.Combine(attachmentpath, senderEmail, lastId);

                        if (!Directory.Exists(attachmentsFolder))
                        {
                            Directory.CreateDirectory(attachmentsFolder);
                        }

                        string fileName = SanitizeFileName(attachment.Name);
                        string filePath = Path.Combine(attachmentsFolder, fileName);

                        if (attachment is GraphApiAttachment fileAttachment)
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(fileAttachment.ContentUrl))
                                {
                                    downloadTasks.Add(DownloadAttachmentFromUrl(fileAttachment.ContentUrl, filePath, httpClient));
                                }
                                else if (!string.IsNullOrEmpty(fileAttachment.ContentBytes))
                                {
                                    byte[] content = Convert.FromBase64String(fileAttachment.ContentBytes);
                                    await Task.Run(() => System.IO.File.WriteAllBytes(filePath, content));
                                    Console.WriteLine($"Attachment saved: {filePath}");
                                }

                                filePaths.Add(filePath);
                                Console.WriteLine($"Attachment queued for saving: {filePath}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error downloading attachment {fileAttachment.Name}: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Attachment {attachment.Name} is not a file attachment.");
                        }
                    }

                    // Wait for all downloads and file writes to complete
                    await Task.WhenAll(downloadTasks);
                }
                else
                {
                    Console.WriteLine("No attachments found.");
                    return "No attachments available.";
                }

                return string.Join(",", filePaths);
            }

            private async Task DownloadAttachmentFromUrl(string contentUrl, string filePath, HttpClient httpClient)
            {
                var response = await httpClient.GetAsync(contentUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();

                    // Write file asynchronously using Task.Run
                    await Task.Run(() => System.IO.File.WriteAllBytes(filePath, content)); // ✅ Fix applied
                }
                else
                {
                    Console.WriteLine($"Failed to download attachment from URL: {contentUrl}");
                }
            }


            private string SanitizeFileName(string fileName)
            {

                return string.Concat(fileName.Split(Path.GetInvalidFileNameChars()));
            }
            //Send
            public async Task<string> SaveAttachments1(GraphApiEmailResponse.GraphApiMessage email, string userId, HttpClient httpClient)
            {
                List<string> filePaths = new List<string>();
                //string attachmentpath = ConfigurationManager.AppSettings["attachmentpath"];
                string attachmentpath = emailconfig;

                // Ensure email has attachments
                if (email.Attachments != null && email.Attachments.Any())
                {
                    foreach (var attachment in email.Attachments)
                    {
                        string senderEmail = email.From?.EmailAddress?.Address ?? "UnknownSender";
                        string attachmentsFolder = Path.Combine(attachmentpath, senderEmail);

                        // Ensure the folder exists
                        if (!Directory.Exists(attachmentsFolder))
                        {
                            Directory.CreateDirectory(attachmentsFolder);
                        }

                        string fileName = SanitizeFileName(attachment.Name);
                        string filePath = Path.Combine(attachmentsFolder, fileName);

                        try
                        {
                            // Check if the attachment is of type 'GraphApiAttachment'
                            if (attachment is GraphApiEmailResponse.GraphApiAttachment graphAttachment)
                            {
                                // If the attachment has contentBytes (Base64 encoded)
                                if (!string.IsNullOrEmpty(graphAttachment.ContentBytes))
                                {
                                    byte[] content = Convert.FromBase64String(graphAttachment.ContentBytes);
                                    System.IO.File.WriteAllBytes(filePath, content);
                                }
                                // If the attachment has a contentUrl, download the file
                                else if (!string.IsNullOrEmpty(graphAttachment.ContentUrl))
                                {
                                    await DownloadAttachmentFromUrl1(graphAttachment.ContentUrl, filePath, httpClient);
                                }

                                filePaths.Add(filePath);
                                Console.WriteLine($"Attachment saved: {filePath}");
                            }
                            else
                            {
                                Console.WriteLine($"Attachment {attachment.Name} is not a file attachment.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error processing attachment {attachment.Name}: {ex.Message}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No attachments found.");
                    return "No attachments available.";
                }

                return string.Join(Environment.NewLine, filePaths);
            }

            public async Task DownloadAttachmentFromUrl1(string contentUrl, string filePath, HttpClient httpClient)
            {
                try
                {
                    // Get the attachment content from the URL
                    var response = await httpClient.GetAsync(contentUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsByteArrayAsync();
                        System.IO.File.WriteAllBytes(filePath, content);
                        Console.WriteLine($"Attachment downloaded: {filePath}");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to download attachment from {contentUrl}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error downloading attachment: {ex.Message}");
                }
            }
        }
    }
}
