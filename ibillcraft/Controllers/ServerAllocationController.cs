using Common.Token;
using ibillcraft.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using iTextSharp.tool.xml;
using iTextSharp.text.pdf;
using PageSize = iTextSharp.text.PageSize;
using Document = iTextSharp.text.Document;
using ClosedXML.Excel;
using OfficeOpenXml;
using System.Data;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Localization;
namespace ibillcraft.Controllers
{
    public class ServerAllocationController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ServerAllocationController> _logger;
        private readonly IStringLocalizer<ServerAllocationController> _localizer;
        public ServerAllocationController(ILogger<ServerAllocationController> logger, IStringLocalizer<ServerAllocationController> localizer, IConfiguration configuration)
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
                string url = $"{_httpClient.BaseAddress}/ServerAllocation/getServers";
                HttpResponseMessage responseView = _httpClient.GetAsync(url).Result;
                dynamic dataView = responseView.Content.ReadAsStringAsync().Result;
                var rootObject = JsonConvert.DeserializeObject<List<ServerOption>>(dataView);
                ViewBag.server = rootObject;
                var compList = new List<ServerAllocationModel>();
                Guid? UserId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6");
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/ServerAllocation/GetCompany").Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<ServerAllocationModel>() };
                    var successResponse = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    int i = 0;
                    compList = successResponse.data;
                    if (compList.Count > 0)
                    {
                        foreach (var item in compList)
                        {
                            Guid? comid = compList[i].com_id;
                            string itemurl = $"{_httpClient.BaseAddress}/ServerAllocation/GetServerValue?userId&com_id={comid}";
                            HttpResponseMessage itemresponse = _httpClient.GetAsync(itemurl).Result;
                            dynamic itemdata = itemresponse.Content.ReadAsStringAsync().Result;
                            var dataObject1 = new { data = new ServerAllocationModel() };
                            var responseitem = JsonConvert.DeserializeAnonymousType(itemdata, dataObject1);
                            var result = "";
                            var Allotted_Date = "";
                            var com_code = "";
                            if (responseitem.data != null)
                            {
                                result = responseitem.data.Server_Value;
                                Allotted_Date = responseitem.data.Allotted_Date;
                                com_code = responseitem.data.com_code;
                            }
                            else
                            {
                                result = null;
                                Allotted_Date = null;
                                com_code = null;
                            }
                            compList[i].result = result;
                            compList[i].Allotted_Date = Allotted_Date;
                            compList[i].com_code = com_code;
                            i++;
                        }
                    }
                    if (compList != null)
                    {
                        return View(compList);
                    }
                    else
                    {
                        var compListdata = new List<ServerAllocationModel>();
                        return View(compListdata);
                    }
                }
                return View(compList);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                throw;
            }
        }

        [HttpGet]
        public IActionResult Get(Guid? com_id)
        {
            try
            {
                var comp = new ServerAllocationModel();
                Guid? UserId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6");
                string url = $"{_httpClient.BaseAddress}/ServerAllocation/GetServerValue?userId={UserId}&com_id={com_id}";
                HttpResponseMessage response = _httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(data);
                    dynamic model = rootObject.outcome;
                    string outcomeDetail = model.outcomeDetail;
                    dynamic comid = rootObject.data;
                    string Server = comid.Server_Value;
                    var result = new { Server };
                    return Ok(Server);
                }
                TempData["errorMessage"] = response.Headers.ToString();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                throw;
            }
        }

        [HttpPost]
        public IActionResult Create(ServerAllocationModel model)
        {
            try
            {
                model.ser_createddate = DateTime.Now;
                model.ser_updateddate = DateTime.Now;
                Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/ServerAllocation", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic successData = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(successData);
                    dynamic successModel = rootObject.outcome;
                    string outcomeDetail = successModel.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail; //.outcome.outcomeid;
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
        public async Task<IActionResult> Excel(Guid? UserId)
        {
            try
            {
                var countryDataList = new List<ServerAllocationModel>();
                UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                string url = $"{_httpClient.BaseAddress}/ServerAllocation/GetExcel?UserId={UserId}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string base64Data = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(base64Data);
                    // Extract the "data" property
                    string base6412 = jsonObject["data"].ToString();
                    DataTable dt = (DataTable)JsonConvert.DeserializeObject(base6412, (typeof(DataTable)));
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Server Allocation");
                            var currentRow = 1;
                            worksheet.Cell(currentRow, 1).Value = "Company Name";
                            worksheet.Cell(currentRow, 2).Value = "Company Code";
                            worksheet.Cell(currentRow, 3).Value = "Contact Number";
                            worksheet.Cell(currentRow, 4).Value = "Email";
                            worksheet.Cell(currentRow, 5).Value = "Staff Number";
                            worksheet.Cell(currentRow, 6).Value = "Country";
                            worksheet.Cell(currentRow, 7).Value = "Package Name";
                            worksheet.Cell(currentRow, 8).Value = "Amount";
                            worksheet.Cell(currentRow, 9).Value = "Subscription Start Date";
                            worksheet.Cell(currentRow, 10).Value = "Subscription End Date";
                            worksheet.Cell(currentRow, 11).Value = "Payment Mode";
                            worksheet.Cell(currentRow, 12).Value = "Server Key";
                            worksheet.Cell(currentRow, 13).Value = "Alloted Date";
                            if (dt.Rows.Count == 0)
                            {
                                TempData["Message"] = "No data found to export!";
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                for (int index = 1; index <= dt.Rows.Count; index++)
                                {
                                    worksheet.Cell(index + 1, 1).Value = dt.Rows[index - 1]["com_company_name"].ToString();
                                    worksheet.Cell(index + 1, 2).Value = dt.Rows[index - 1]["com_code"].ToString();
                                    worksheet.Cell(index + 1, 3).Value = dt.Rows[index - 1]["com_contact"].ToString();
                                    worksheet.Cell(index + 1, 4).Value = dt.Rows[index - 1]["com_email"].ToString();
                                    worksheet.Cell(index + 1, 5).Value = dt.Rows[index - 1]["com_staff_no"].ToString();
                                    worksheet.Cell(index + 1, 6).Value = dt.Rows[index - 1]["CountryId"].ToString();
                                    worksheet.Cell(index + 1, 7).Value = dt.Rows[index - 1]["package_name"].ToString();
                                    worksheet.Cell(index + 1, 8).Value = dt.Rows[index - 1]["final_Amount"].ToString();
                                    worksheet.Cell(index + 1, 9).Value = dt.Rows[index - 1]["StartDate"].ToString();
                                    worksheet.Cell(index + 1, 10).Value = dt.Rows[index - 1]["EndDate"].ToString();
                                    worksheet.Cell(index + 1, 11).Value = dt.Rows[index - 1]["Payment_Mode"].ToString();
                                    worksheet.Cell(index + 1, 12).Value = dt.Rows[index - 1]["Server_Key"].ToString();
                                    worksheet.Cell(index + 1, 13).Value = dt.Rows[index - 1]["Allotted_Date"].ToString();
                                }
                            }
                            using (var stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                var content = stream.ToArray();
                                return File(
                                    content,
                                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    "Server Allocation.xlsx");
                            }
                        }
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Failed to retrieve data from the API.";
                    return BadRequest("Failed to retrieve data from the API.");
                }
            }

            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Pdf(Guid? UserId)
        {
            MemoryStream ms1 = new MemoryStream();
            try
            {
                ServerAllocationModel model = new ServerAllocationModel();
                UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                string data = JsonConvert.SerializeObject(model);
                string url = $"{_httpClient.BaseAddress}/ServerAllocation/GetPdf?UserId={UserId}";
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(url); // Replace with your actual API endpoint URL
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        string htmlContent = content.ToString();
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
                            return File(pdfBytes, "application/pdf", "ServerAllocation.pdf");
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to retrieve data from the API.";
                        return BadRequest("Failed to retrieve data from the API.");
                    }
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
