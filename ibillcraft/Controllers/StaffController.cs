using ClosedXML.Excel;
using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using ibillcraft.Models;
using System.Text;
using iTextSharp.tool.xml;
using System.Data;

namespace ibillcraft.Controllers
{
    [ExampleFilter1]

    public class StaffController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<StaffController> _logger;
        private readonly IStringLocalizer<StaffController> _localizer;
        public StaffController(ILogger<StaffController> logger, IStringLocalizer<StaffController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);
            _logger = logger;
            _localizer = localizer;
        }
        public IActionResult Index()
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

				var StaffModelList = new List<StaffModel>();
                string staffurl = $"{_httpClient.BaseAddress}/Staff/GetAll?UserId={UserId}&Server_Value={CUtility.serverValue}&st_com_id={new Guid(CUtility.comid)}";
                HttpResponseMessage response = _httpClient.GetAsync(staffurl).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<StaffModel>() };
                    var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    StaffModelList = response2.data;

                    if (StaffModelList != null)
                    {
                        return View(StaffModelList);
                    }
                    else
                    {
                        var StaffModelDataList = new List<StaffModel>();
                        return View(StaffModelDataList);
                    }
                }
                return View(StaffModelList);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }
        }
        public IActionResult EditIndex()
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
                ViewBag.co_country_code = CUtility.co_country_code;
                string geurl = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_ParameterValueMaster&sValue=pv_parametervalue&id=pv_id&IsActiveColumn=pv_isactive&sCoulmnName=pv_parameterid&sColumnValue=2371016c-70a0-4582-9da7-17d0386c1d9c";
                HttpResponseMessage geresponseView = _httpClient.GetAsync(geurl).Result;
                dynamic gedata = geresponseView.Content.ReadAsStringAsync().Result;
                var gerootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(gedata);
                ViewBag.st_gender = gerootObject;
                string deurl = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_DesignationMaster&sValue=de_designation_name&id=de_id&IsActiveColumn=de_isactive";//&sCoulmnName=pv_parameterid&sColumnValue=2371016c-70a0-4582-9da7-17d0386c1d9c";
                HttpResponseMessage deresponseView = _httpClient.GetAsync(deurl).Result;
                dynamic dedata = deresponseView.Content.ReadAsStringAsync().Result;
                var derootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(dedata);
                ViewBag.st_designation_id = derootObject;
                string ciurl = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_CityMaster&sValue=ci_city_name&id=ci_id&IsActiveColumn=ci_isactive";//&sCoulmnName=pv_parameterid&sColumnValue=2371016c-70a0-4582-9da7-17d0386c1d9c";
                HttpResponseMessage ciresponseView = _httpClient.GetAsync(ciurl).Result;
                dynamic cidata = ciresponseView.Content.ReadAsStringAsync().Result;
                var cirootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(cidata);
                ViewBag.st_city_id = cirootObject;

                string ceurl = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_ParameterValueMaster&sValue=pv_parametervalue&id=pv_id&IsActiveColumn=pv_isactive&sCoulmnName=pv_parameterid&sColumnValue=a7c24a46-2a70-43f4-bd80-fb652b28b71d";
                HttpResponseMessage ceresponseView = _httpClient.GetAsync(ceurl).Result;
                dynamic cedata = ceresponseView.Content.ReadAsStringAsync().Result;
                var cerootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(cedata);
                ViewBag.st_category = cerootObject;

                string url = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_RoleMaster&sValue=r_rolename&id=r_id&IsActiveColumn=r_isactive&Server_Value={CUtility.serverValue}&sCoulmnName=r_com_id&sColumnValue={CUtility.comid}";
                HttpResponseMessage responseView = _httpClient.GetAsync(url).Result;
                dynamic data1 = responseView.Content.ReadAsStringAsync().Result;
                var rootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(data1);
                ViewBag.st_rolename = rootObject;


                ViewBag.Edit = "Entry";
				ViewBag.st_isactive = "Active";
				return View();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }
        }
        [HttpPost]
        public IActionResult Create(StaffModel model)
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
                model.st_com_id = new Guid(CUtility.comid);
                model.Server_Value = CUtility.serverValue;
                model.st_createddate = DateTime.Now;
                model.st_updateddate = DateTime.Now;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/staff", content).Result;
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

        [HttpGet]
        public IActionResult Edit(Guid? st_id)
        {
            StaffModel model = new StaffModel();
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

				string url = $"{_httpClient.BaseAddress}/Staff/Get?st_id={st_id}&UserId={UserId}&Server_Value={CUtility.serverValue}&st_com_id={new Guid(CUtility.comid)}";
                HttpResponseMessage response = _httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var rootObject = JsonConvert.DeserializeObject<Staff>(data);
                     model = rootObject.data;//new { rootObject.data };
                    string geurl = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_ParameterValueMaster&sValue=pv_parametervalue&id=pv_id&IsActiveColumn=pv_isactive&sCoulmnName=pv_parameterid&sColumnValue=2371016c-70a0-4582-9da7-17d0386c1d9c";
                    HttpResponseMessage geresponseView = _httpClient.GetAsync(geurl).Result;
                    dynamic gedata = geresponseView.Content.ReadAsStringAsync().Result;
                    var gerootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(gedata);
                    ViewBag.SelectedGender = model.st_gender;
                    ViewBag.st_gender = gerootObject;

                    string deurl = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_DesignationMaster&sValue=de_designation_name&id=de_id&IsActiveColumn=de_isactive";//&sCoulmnName=pv_parameterid&sColumnValue=2371016c-70a0-4582-9da7-17d0386c1d9c";
                    HttpResponseMessage deresponseView = _httpClient.GetAsync(deurl).Result;
                    dynamic dedata = deresponseView.Content.ReadAsStringAsync().Result;
                    var derootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(dedata);
                    ViewBag.SelectedDeg = model.st_designation_id;
                    ViewBag.st_designation_id = derootObject;

                    string ciurl = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_CityMaster&sValue=ci_city_name&id=ci_id&IsActiveColumn=ci_isactive";//&sCoulmnName=pv_parameterid&sColumnValue=2371016c-70a0-4582-9da7-17d0386c1d9c";
                    HttpResponseMessage ciresponseView = _httpClient.GetAsync(ciurl).Result;
                    dynamic cidata = ciresponseView.Content.ReadAsStringAsync().Result;
                    var cirootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(cidata);
                    ViewBag.st_city_id = cirootObject;
                    ViewBag.SelectedCi = model.st_city_id;



                    string ceurl = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_ParameterValueMaster&sValue=pv_parametervalue&id=pv_id&IsActiveColumn=pv_isactive&sCoulmnName=pv_parameterid&sColumnValue=a7c24a46-2a70-43f4-bd80-fb652b28b71d";
                    HttpResponseMessage ceresponseView = _httpClient.GetAsync(ceurl).Result;
                    dynamic cedata = ceresponseView.Content.ReadAsStringAsync().Result;
                    var cerootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(cedata);
                    ViewBag.st_category = cerootObject;
                   ViewBag.SelectedCat = model.st_category;

                    string urlr = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_RoleMaster&sValue=r_rolename&id=r_id&IsActiveColumn=r_isactive&Server_Value={CUtility.serverValue}&sCoulmnName=r_com_id&sColumnValue={CUtility.comid}";
                    HttpResponseMessage rresponseView = _httpClient.GetAsync(urlr).Result;
                    dynamic rdata1 = rresponseView.Content.ReadAsStringAsync().Result;
                    var rrootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(rdata1);
                    ViewBag.st_rolename = rrootObject;
                    ViewBag.Selectedrolename = model.st_rolename;
                    if (model.co_country_code ==null)
                    {
                        ViewBag.co_country_code = CUtility.co_country_code;
                    }
                    else
                    {
                        ViewBag.co_country_code = model.co_country_code;
                    }


                    //ViewBag.SelectedCi = model.st_city_id;
                   
                    ViewBag.st_staff_name = model.st_staff_name;
                    ViewBag.st_staff_code = model.st_staff_code;
                    ViewBag.st_username = model.st_username;
                    ViewBag.st_id = model.st_id;
                    ViewBag.Edit = "Edit";
                    ViewBag.st_email = model.st_email;
                    ViewBag.st_address = model.st_address;
                    ViewBag.st_contact = model.st_contact;
                     ViewBag.st_isactive = model.st_isactive;
                    DateTime? dob = model.st_dob != null ? DateTime.Parse(model.st_dob) : (DateTime?)null;
                    ViewBag.st_dob = dob?.ToString("yyyy-MM-dd");
                    ViewBag.st_salary = model.st_salary;
                    DateTime? JDate = model.st_joining_date != null ? DateTime.Parse(model.st_joining_date) : (DateTime?)null;
                    ViewBag.st_joining_date = JDate?.ToString("yyyy-MM-dd");
                    DateTime? LDate = model.st_left_date != null ? DateTime.Parse(model.st_left_date) : (DateTime?)null;
                    ViewBag.st_left_date = LDate?.ToString("yyyy-MM-dd");                   
                    return View("EditIndex");
                }
                TempData["errorMessage"] = response.Headers.ToString();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }
        }
        public IActionResult Delete(Guid? st_id)
        {
            try
            {
                StaffModel model = new StaffModel();
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
                model.st_id = st_id;
                model.st_com_id = new Guid(CUtility.comid);
                model.Server_Value = CUtility.serverValue;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/Staff/Delete", content).Result;
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
        public async Task<IActionResult> Excel()
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

				string url = $"{_httpClient.BaseAddress}/Staff/GetExcel?UserId={UserId}&Server_Value={CUtility.serverValue}&st_com_id={CUtility.comid}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string base64Data = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(base64Data);
                    string base6412 = jsonObject["data"].ToString();
                    DataTable dt = (DataTable)JsonConvert.DeserializeObject(base6412, (typeof(DataTable)));
                    // Create a memory stream from the byte array
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                        // Load the data into an Excel package (using the EPPlus library)
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("User");
                            var currentRow = 1;
                            worksheet.Cell(currentRow, 1).Value = "Name";
                            worksheet.Cell(currentRow, 2).Value = "Email";
                            worksheet.Cell(currentRow, 3).Value = "Address";
                            worksheet.Cell(currentRow, 4).Value = "Contact";
                            worksheet.Cell(currentRow, 5).Value = "Designation";
                            worksheet.Cell(currentRow, 6).Value = "Status";
                            if (dt == null)//(dt.Rows.Count == 0)
                            {
                                TempData["errorMessage"] = "No data found to export!";
                                return Redirect($"/Staff");
                            }
                            else
                            {
                                for (int index = 1; index <= dt.Rows.Count; index++)
                                {
                                    worksheet.Cell(index + 1, 1).Value = dt.Rows[index - 1]["st_staff_name"].ToString();
                                    worksheet.Cell(index + 1, 2).Value = dt.Rows[index - 1]["st_email"].ToString();
                                    worksheet.Cell(index + 1, 3).Value = dt.Rows[index - 1]["st_address"].ToString();
                                    worksheet.Cell(index + 1, 4).Value = dt.Rows[index - 1]["st_contact"].ToString();
                                    worksheet.Cell(index + 1, 5).Value = dt.Rows[index - 1]["st_designation_id"].ToString();
                                    worksheet.Cell(index + 1, 6).Value = dt.Rows[index - 1]["st_isactive"].ToString();
                                }
                            }
                            using (var stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                var content = stream.ToArray();
                                return File(
                                    content,
                                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    "User.xlsx");
                            }
                        }
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Failed to retrieve data from the API.";
                    return Redirect($"/Staff");
                }
            }

            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Redirect($"/Staff");
            }
        }

    }
}
