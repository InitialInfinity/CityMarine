using ibillcraft.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json.Linq;
using PageSize = iTextSharp.text.PageSize;
using Document = iTextSharp.text.Document;
using ClosedXML.Excel;
using OfficeOpenXml;
using System.Data;
using Microsoft.Extensions.Localization;
namespace ibillcraft.Controllers
{
    public class SubscriptionMasterController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SubscriptionMasterController> _logger;
        private readonly IStringLocalizer<SubscriptionMasterController> _localizer;
        public SubscriptionMasterController(ILogger<SubscriptionMasterController> logger, IStringLocalizer<SubscriptionMasterController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);
            _logger = logger;
            _localizer = localizer;
        }
        public IActionResult Index(string status)
        {
            try
            {
                if (status == null)
                {
                    status = "1";
                }
                var subDataList = new List<SubscriptionModel>(); ;
                Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/SubscriptionMaster/GetSubscription?UserId=" + UserId + "&status=" + status).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<SubscriptionModel>() };
                    var decerialze = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    subDataList = decerialze.data;
                    if (subDataList != null)
                    {
                        return View(subDataList);
                    }
                    else
                    {
                        var sub_DataList = new List<SubscriptionModel>();
                        return View(sub_DataList);
                    }
                }
                return View(subDataList);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }
        }
        [HttpPost]
        public IActionResult Create(SubscriptionModel model)
        {
            try
            {
                model.com_createddate = DateTime.Now;
                model.com_updateddate = DateTime.Now;
                Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/SubscriptionMaster", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic responsedata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(responsedata);
                    dynamic responsemodel = rootObject.outcome;
                    string outcomeDetail = responsemodel.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail; //.outcome.outcomeid;
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

        public IActionResult Delete(Guid? sm_id)
        {
            try
            {
                SubscriptionModel model = new SubscriptionModel();
                Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                model.sm_id = sm_id;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/SubscriptionMaster/DeleteSubscription", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic responsedata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(responsedata);
                    dynamic responsemodel = rootObject.outcome;
                    string outcomeDetail = responsemodel.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail; //.outcome.outcomeid;
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

        public async Task<IActionResult> Excel(Guid? UserId, string status)
        {
            try
            {
                var subscriptionModelList = new List<SubscriptionModel>();
                UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                string url = $"{_httpClient.BaseAddress}/SubscriptionMaster/GetExcel?UserId={UserId}&status={status}";
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
                        // Load the data into an Excel package (using the EPPlus library)
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Subscription Master");
                            var currentRow = 1;
                            worksheet.Cell(currentRow, 1).Value = "Package Name";
                            worksheet.Cell(currentRow, 2).Value = "Service";
                            worksheet.Cell(currentRow, 3).Value = "Amount";
                            worksheet.Cell(currentRow, 4).Value = "Discount";
                            worksheet.Cell(currentRow, 5).Value = "Final Amount";
                            worksheet.Cell(currentRow, 6).Value = "Duration in months";
                            worksheet.Cell(currentRow, 7).Value = "Invoice";
                            worksheet.Cell(currentRow, 8).Value = "Quotation";
                            worksheet.Cell(currentRow, 9).Value = "Expence";
                            worksheet.Cell(currentRow, 10).Value = "Cash Order";
                            worksheet.Cell(currentRow, 11).Value = "Status";
                            if (dt == null)//(dt.Rows.Count == 0)
                            {
                                TempData["errorMessage"] = "No data found to export!";
                                return Redirect($"/SubscriptionMaster?status={status}");

                            }
                            else
                            {
                                for (int index = 1; index <= dt.Rows.Count; index++)
                                {
                                    worksheet.Cell(index + 1, 1).Value = dt.Rows[index - 1]["package_name"].ToString();
                                    worksheet.Cell(index + 1, 2).Value = dt.Rows[index - 1]["sm_service"].ToString();
                                    worksheet.Cell(index + 1, 3).Value = dt.Rows[index - 1]["subAmount"].ToString();
                                    worksheet.Cell(index + 1, 4).Value = dt.Rows[index - 1]["subDiscount"].ToString();
                                    worksheet.Cell(index + 1, 5).Value = dt.Rows[index - 1]["final_Amount"].ToString();
                                    worksheet.Cell(index + 1, 6).Value = dt.Rows[index - 1]["sub_duration"].ToString();
                                    worksheet.Cell(index + 1, 7).Value = dt.Rows[index - 1]["Invoice"].ToString();
                                    worksheet.Cell(index + 1, 8).Value = dt.Rows[index - 1]["Quotation"].ToString();
                                    worksheet.Cell(index + 1, 9).Value = dt.Rows[index - 1]["Expence"].ToString();
                                    worksheet.Cell(index + 1, 10).Value = dt.Rows[index - 1]["cash_order"].ToString();
                                    worksheet.Cell(index + 1, 11).Value = dt.Rows[index - 1]["com_isactive"].ToString();
                                }
                            }
                            using (var stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                var content = stream.ToArray();
                                return File(
                                    content,
                                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    "Subscription Master.xlsx");
                            }
                        }
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Failed to retrieve data from the API.";
                    return Redirect($"/SubscriptionMaster?status={status}");
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
                SubscriptionModel model = new SubscriptionModel();
                UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                string data = JsonConvert.SerializeObject(model);
                string url = $"{_httpClient.BaseAddress}/SubscriptionMaster/GetPdf?UserId={UserId}&status={status}";
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(url); // Replace with your actual API endpoint URL
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
                                return File(pdfBytes, "application/pdf", "SubscriptionMaster.pdf");
                            }
                        }
                        else
                        {
                            TempData["errorMessage"] = "Failed to retrieve data from the API.";
                            return Redirect($"/SubscriptionMaster?status={status}");
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to retrieve data from the API.";
                        return Redirect($"/SubscriptionMaster?status={status}");
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
