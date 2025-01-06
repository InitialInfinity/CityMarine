using ibillcraft.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;
using ClosedXML.Excel;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.Extensions.Localization;
using OfficeOpenXml;
using iTextSharp.text;
using DocumentFormat.OpenXml.EMMA;

namespace ibillcraft.Controllers
{
    public class StateMasterController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<StateMasterController> _logger;
        private readonly IStringLocalizer<StateMasterController> _localizer;

        public StateMasterController(ILogger<StateMasterController> logger, IStringLocalizer<StateMasterController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);
            _logger = logger;
            _localizer = localizer;

        }
        public IActionResult Index(string CountryId, string status, string CountryName)
        {
            try
            {
                if (status == null)
                {
                    status = "1";
                }
                var stateDataList = new List<StateMasterModel>();
                Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                ViewBag.CountryId = CountryId;
                ViewBag.CountryName = CountryName;
                string url = $"{_httpClient.BaseAddress}/StateMaster/GetStateMasterByCoId?UserId={UserId}&CountryId={CountryId}&status={status}";
                List<StateMasterModel> countrylist = new List<StateMasterModel>();
                HttpResponseMessage response = _httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<StateMasterModel>() };
                    var responsemodel = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    stateDataList = responsemodel.data;
                    if (stateDataList != null)
                    {
                        return View(stateDataList);
                    }
                    else
                    {
                        var stateDataListse = new List<StateMasterModel>();
                        return View(stateDataListse);
                    }
                }
                return View(stateDataList);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }
        }

        [HttpPost]
        public IActionResult Create(StateMasterModel model)
        {
            try
            {
                Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/StateMaster", content).Result;
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
                return RedirectToAction("Index",model.s_country_name,model.s_country_id);
            }
        }

        [HttpGet]
        public IActionResult Edit(Guid? s_id)
        {
            Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
            StateMasterModel model = new StateMasterModel();
            try
            {
                string url = $"{_httpClient.BaseAddress}/StateMaster/GetStateById?s_id={s_id}&UserId={UserId}";
                HttpResponseMessage response = _httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var rootObject = JsonConvert.DeserializeObject<State>(data);
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
        public IActionResult Delete(Guid? s_id, string s_country_id, string s_country_name)
        {
            try
            {
                StateMasterModel model = new StateMasterModel();
                Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                model.s_id = s_id;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/StateMaster/DeleteState", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic responsedata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(responsedata);
                    dynamic responsemodel = rootObject.outcome;
                    string outcomeDetail = responsemodel.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail;
                    TempData["CountryId"] = s_country_id;
                    TempData["CountryName"] = s_country_name;
                    return Redirect($"/StateMaster?CountryId={s_country_id}&CountryName={s_country_name}");
                }
                TempData["errorMessage"] = response.Headers.ToString();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Redirect($"/StateMaster?CountryId={s_country_id}&CountryName={s_country_name}");
            }
        }
        public async Task<IActionResult> Excel(Guid? UserId, string countryId,string CountryName, string status)
        {
            try
            {
                var countryDataList = new List<StateMasterModel>();
                UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                string url = $"{_httpClient.BaseAddress}/StateMaster/GetExcel?UserId={UserId}&co_id={countryId}&status={status}";
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
                            var worksheet = workbook.Worksheets.Add("State Master");
                            var currentRow = 1;
                            worksheet.Cell(currentRow, 1).Value = "Country Name";
                            worksheet.Cell(currentRow, 2).Value = "Code";
                            worksheet.Cell(currentRow, 3).Value = "Name";
                            worksheet.Cell(currentRow, 4).Value = "Status";
                            if (dt == null)//(dt.Rows.Count == 0)
                            {
                                TempData["errorMessage"] = "No data found to export!";
                                return Redirect($"/StateMaster?CountryId={countryId}&CountryName={CountryName}&status={status}");
                            }
                            else
                            {
                                for (int index = 1; index <= dt.Rows.Count; index++)
                                {
                                    worksheet.Cell(index + 1, 1).Value = dt.Rows[index - 1]["s_country_name"].ToString();
                                    worksheet.Cell(index + 1, 2).Value = dt.Rows[index - 1]["s_state_code"].ToString();
                                    worksheet.Cell(index + 1, 3).Value = dt.Rows[index - 1]["s_state_name"].ToString();
                                    worksheet.Cell(index + 1, 4).Value = dt.Rows[index - 1]["s_isactive"].ToString();
                                }
                            }
                            using (var stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                var content = stream.ToArray();
                                return File(
                                    content,
                                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    "State Master.xlsx");
                            }
                        }
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Failed to retrieve data from the API.";
                    return Redirect($"/StateMaster?CountryId={countryId}&CountryName={CountryName}&status={status}");

                }
            }

            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Redirect($"/StateMaster?CountryId={countryId}&CountryName={CountryName}&status={status}");
            }
        }

        public async Task<IActionResult> Pdf(string countryId, string CountryName, string status)
        {
            MemoryStream ms1 = new MemoryStream();
            try
            {
                StateMasterModel model = new StateMasterModel();
                Guid UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                string data = JsonConvert.SerializeObject(model);
                string url = $"{_httpClient.BaseAddress}/StateMaster/GetPdf?UserId={UserId}&co_id={countryId}&status={status}";
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
                                return File(pdfBytes, "application/pdf", "StateMaster.pdf");
                            }
                        }
                        else
                        {
                            TempData["errorMessage"] = "No data found to export!";
                            return Redirect($"/StateMaster?CountryId={countryId}&CountryName={CountryName}&status={status}");

                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to retrieve data from the API.";
                        return Redirect($"/StateMaster?CountryId={countryId}&CountryName={CountryName}&status={status}");
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
