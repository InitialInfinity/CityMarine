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
    public class EmployeeEmailMgmtController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EmployeeEmailMgmtController> _logger;
        private readonly IStringLocalizer<EmployeeEmailMgmtController> _localizer;
        public EmployeeEmailMgmtController(ILogger<EmployeeEmailMgmtController> logger, IStringLocalizer<EmployeeEmailMgmtController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);
            _logger = logger;
            _localizer = localizer;
        }
        public IActionResult Index(string? status)
        {
            if (status == null)
            {
                status = "1";
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

            string url = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_staff&sValue=st_staff_name&id=st_id&IsActiveColumn=st_isactive&sCoulmnName=st_com_id&sColumnValue={CUtility.comid}";
            HttpResponseMessage responseView = _httpClient.GetAsync(url).Result;
            dynamic data1 = responseView.Content.ReadAsStringAsync().Result;
            var rootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(data1);
            ViewBag.staffname = rootObject;


            string emailurl = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_EmailList&sValue=E_email&id=E_id&IsActiveColumn=E_isactive";
            HttpResponseMessage emailresponseView = _httpClient.GetAsync(emailurl).Result;
            dynamic emaildata = emailresponseView.Content.ReadAsStringAsync().Result;
            var emailrootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(emaildata);
            ViewBag.email = emailrootObject;



            var emailmgmtDataList = new List<EmployeeEmailMgmtModel>(); ;

            var emailmgmtList = new List<EmployeeEmailMgmtModel>();
            string emailmgmturl = $"{_httpClient.BaseAddress}/EmployeeEmailMgmt/GetAll?UserId={UserId}&status={status}";
            HttpResponseMessage response = _httpClient.GetAsync(emailmgmturl).Result;
            if (response.IsSuccessStatusCode)
            {
                dynamic data = response.Content.ReadAsStringAsync().Result;
                var dataObject = new { data = new List<EmployeeEmailMgmtModel>() };
                var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                emailmgmtList = response2.data;

                if (emailmgmtList != null)
                {
                    return View(emailmgmtList);
                }
                else
                {
                    var emailmgmtDataList1 = new List<EmployeeEmailMgmtModel>();
                    return View(emailmgmtDataList1);
                }
            }
            return View(emailmgmtDataList);
        }

        [HttpPost]
        public IActionResult Create(EmployeeEmailMgmtModel model)
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

                ViewBag.Format = CUtility.format;
                Guid? UserId = new Guid(CUtility.comid);
                model.UserId = UserId;
                model.e_createddate = DateTime.Now;
                model.e_updateddate = DateTime.Now;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/EmployeeEmailMgmt", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic resdata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(resdata);
                    dynamic resmodel = rootObject.outcome;
                    string outcomeDetail = resmodel.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail;
                    return Ok(outcomeDetail);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        public IActionResult Delete(Guid? e_id)
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
                EmployeeEmailMgmtModel model = new EmployeeEmailMgmtModel();
                string? UserId = CUtility.userid;
                model.UserId = new Guid(UserId);
                model.e_id = e_id;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/EmployeeEmailMgmt/Delete", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic resdata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(resdata);
                    dynamic resmodel = rootObject.outcome;
                    string outcomeDetail = resmodel.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail;
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Excel(string? UserId, string status)
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
                 UserId = CUtility.userid;

                string url = $"{_httpClient.BaseAddress}/EmployeeEmailMgmt/GetExcel?UserId={UserId}&status={status}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string base64Data = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(base64Data);
                    string base6412 = jsonObject["data"].ToString();
                    DataTable dt = (DataTable)JsonConvert.DeserializeObject(base6412, (typeof(DataTable)));
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Employee Email Management");
                            var currentRow = 1;
                            worksheet.Cell(currentRow, 1).Value = "Sr.No";
                            worksheet.Cell(currentRow, 2).Value = "Email";
                            worksheet.Cell(currentRow, 3).Value = "Employees";
                            worksheet.Cell(currentRow, 4).Value = "Status";
                            if (dt == null)
                            {
                                TempData["errorMessage"] = "No data found to export!";
                                return Redirect($"/EmployeeEmailMgmt?status={status}");
                            }
                            else
                            {
                                for (int index = 1; index <= dt.Rows.Count; index++)
                                {
                                    worksheet.Cell(index + 1, 1).Value = index.ToString();
                                    worksheet.Cell(index + 1, 2).Value = dt.Rows[index - 1]["e_email"].ToString();
                                    worksheet.Cell(index + 1, 3).Value = dt.Rows[index - 1]["e_staffname"].ToString();
                                    worksheet.Cell(index + 1, 4).Value = dt.Rows[index - 1]["e_isactive"].ToString();
                                }

                            }
                            using (var stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                var content = stream.ToArray();
                                return File(
                                    content,
                                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    "Employee Email Management.xlsx");
                            }
                        }
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Failed to retrieve data from the API.";
                    return Redirect($"/EmployeeEmailMgmt?status={status}");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Pdf(Guid? UserId, string status)
        {
            MemoryStream ms1 = new MemoryStream();
            try
            {
                EmployeeEmailMgmtModel model = new EmployeeEmailMgmtModel();
                UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                model.e_isactive = status;
                string data = JsonConvert.SerializeObject(model);
                string url = $"{_httpClient.BaseAddress}/EmployeeEmailMgmt/GetPdf?UserId={UserId}&status={status}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    string htmlContent = content.ToString();
                    if (htmlContent.Contains("<td"))
                    {
                        MemoryStream ms = new MemoryStream();
                        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, ms);
                        pdfDoc.Open();
                        TextReader sr = new StringReader(htmlContent);
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                        pdfDoc.Close();
                        byte[] pdfBytes = ms.ToArray();
                        string base64Pdf = Convert.ToBase64String(pdfBytes);
                        ms1 = ms;
                        using (var stream = new MemoryStream())
                        {
                            return File(pdfBytes, "application/pdf", "Employee Email Management.pdf");
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "No data found to export!";
                        return Redirect($"/EmployeeEmailMgmt?status={status}");
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Failed to retrieve data from the API.";
                    return Redirect($"/EmployeeEmailMgmt?status={status}");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            return new FileStreamResult(ms1, "application/pdf");
        }
    }
}
