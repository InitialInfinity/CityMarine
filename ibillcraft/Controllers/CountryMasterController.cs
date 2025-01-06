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

namespace ibillcraft.Controllers
{
    public class CountryMasterController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CountryMasterController> _logger;
        private readonly IStringLocalizer<CountryMasterController> _localizer;
        public CountryMasterController(ILogger<CountryMasterController> logger, IStringLocalizer<CountryMasterController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);
            _logger = logger;
            _localizer = localizer;

        }
        [HttpGet]
        public IActionResult Index(string status)
        {
            try
            {
                if (status == null)
                {
                    status = "1";
                }
                string url = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_CurrencyMaster&sValue=cm_currencyname&id=cm_id&IsActiveColumn=cm_isactive&sCoulmnName";
                HttpResponseMessage responseView = _httpClient.GetAsync(url).Result;
                dynamic data1 = responseView.Content.ReadAsStringAsync().Result;
                var rootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(data1);
                ViewBag.co_currency_name = rootObject;
                var countryDataList = new List<CountryMasterModel>(); ;
                Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/CountryMaster/GetAll?UserId={UserId}&status={status}").Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<CountryMasterModel>() };
                    var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    countryDataList = response2.data;
                    if (countryDataList != null)
                    {
                        return View(countryDataList);
                    }
                    else
                    {
                        var countryDataList2 = new List<CountryMasterModel>();
                        return View(countryDataList2);
                    }
                }
                return View(countryDataList);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                throw;
            }
        }
        [HttpPost]
        public IActionResult Create(CountryMasterModel model)
        {
            try
            {
                Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                model.co_createddate = DateTime.Now;
                model.co_updateddate = DateTime.Now;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/CountryMaster", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data2 = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(data2);
                    dynamic model2 = rootObject.outcome;
                    string outcomeDetail = model2.outcomeDetail;
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
        [HttpGet]
        public IActionResult Edit(Guid? co_id)
        {
            Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
            CountryMasterModel model = new CountryMasterModel();
            try
            {
                string url = $"{_httpClient.BaseAddress}/CountryMaster/Get?co_id={co_id}&UserId={UserId}";
                HttpResponseMessage response = _httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var rootObject = JsonConvert.DeserializeObject<RootObject>(data);
                    model = rootObject.data;
                }
                return Ok(model);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        public IActionResult Delete(Guid? co_id)
        {
            try
            {
                CountryMasterModel model = new CountryMasterModel();
                Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                model.co_id = co_id;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/CountryMaster/Delete", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data2 = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(data2);
                    dynamic model2 = rootObject.outcome;
                    string outcomeDetail = model2.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail; //.outcome.outcomeid;
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

        public async Task<IActionResult> Excel(Guid? UserId, string status)
        {
            try
            {
                var countryDataList = new List<CountryMasterModel>();
                UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                string url = $"{_httpClient.BaseAddress}/CountryMaster/GetExcel?UserId={UserId}&status={status}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string base64Data = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(base64Data);
                    // Extract the "data" property
                    string base6412 = jsonObject["data"].ToString();
                    DataTable dt = (DataTable)JsonConvert.DeserializeObject(base6412, (typeof(DataTable)));
                    // Create a memory stream from the byte array
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                        // Load the data into an Excel package (using the EPPlus library)
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Country Master");
                            var currentRow = 1;
                            worksheet.Cell(currentRow, 1).Value = "Country Code";
                            worksheet.Cell(currentRow, 2).Value = "Country Name";
                            worksheet.Cell(currentRow, 3).Value = "Currency Name";
                            worksheet.Cell(currentRow, 4).Value = "Country Timezone";
                            worksheet.Cell(currentRow, 5).Value = "Status";
                            if (dt == null)//(dt.Rows.Count == 0)
                            {
                                TempData["Message"] = "No data found to export!";
                                return Redirect($"/CountryMaster?status={status}");
                            }
                            else
                            {

                                for (int index = 1; index <= dt.Rows.Count; index++)
                                {
                                    worksheet.Cell(index + 1, 1).Value = dt.Rows[index - 1]["co_country_code"].ToString();
                                    worksheet.Cell(index + 1, 2).Value = dt.Rows[index - 1]["co_country_name"].ToString();
                                    worksheet.Cell(index + 1, 3).Value = dt.Rows[index - 1]["co_currency_name"].ToString();
                                    worksheet.Cell(index + 1, 4).Value = dt.Rows[index - 1]["co_timezone"].ToString();
                                    worksheet.Cell(index + 1, 5).Value = dt.Rows[index - 1]["co_isactive"].ToString();
                                }

                            }
                            using (var stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                var content = stream.ToArray();
                                return File(
                                    content,
                                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    "Country Master.xlsx");
                            }
                        }
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Failed to retrieve data from the API.";
                    return Redirect($"/CountryMaster?status={status}");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Redirect($"/CountryMaster?status={status}");
            }
        }

        public async Task<IActionResult> Pdf(Guid? UserId, string status)
        {
            MemoryStream ms1 = new MemoryStream();
            try
            {
                CountryMasterModel model = new CountryMasterModel();
                UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                string data = JsonConvert.SerializeObject(model);
                string url = $"{_httpClient.BaseAddress}/CountryMaster/GetPdf?UserId={UserId}&status={status}";
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
                               return File(pdfBytes, "application/pdf", "CountryMaster.pdf");
                            }
                        }
                        else
                        {
                            TempData["errorMessage"] = "No data found to export!";
                            return Redirect($"/CountryMaster?status={status}");
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to retrieve data from the API.";
                        return Redirect($"/CountryMaster?status={status}");
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