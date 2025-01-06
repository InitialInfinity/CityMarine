using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ibillcraft.Models;
using System.Text;
using PageSize = iTextSharp.text.PageSize;
using Document = iTextSharp.text.Document;
using ClosedXML.Excel;
using OfficeOpenXml;
using System.Data;

namespace ibillcraft.Controllers
{
    public class UnitConfigurationController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UnitConfigurationController> _logger;
        private readonly IStringLocalizer<UnitConfigurationController> _localizer;

        public UnitConfigurationController(ILogger<UnitConfigurationController> logger, IStringLocalizer<UnitConfigurationController> localizer, IConfiguration configuration)
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
                var unitcList = new List<UnitConfigurationModel>();
                Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                string url = $"{_httpClient.BaseAddress}/UnitConfiguration/GetAll?UserId={UserId}&status={status}";
                HttpResponseMessage response = _httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<UnitConfigurationModel>() };
                    var responsemodel = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    unitcList = responsemodel.data;
                    if (unitcList != null)
                    {
                        return View(unitcList);
                    }
                    else
                    {
                        var unitlistse = new List<UnitConfigurationModel>();
                        return View(unitlistse);
                    }
                }
                return View(unitcList);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }
        }

        [HttpPost]
        public IActionResult Create(UnitConfigurationModel model)
        {
            try
            {
                Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                model.u_createddate=DateTime.Now;
                model.u_updateddate = DateTime.Now;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/UnitConfiguration", content).Result;
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

        public IActionResult Delete(Guid? u_id)
        {
            try
            {
                UnitConfigurationModel model = new UnitConfigurationModel();
                Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                model.u_id = u_id;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/UnitConfiguration/Delete", content).Result;
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

        public async Task<IActionResult> Excel(Guid? UserId,string status)
        {
            try
            {
                UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                string url = $"{_httpClient.BaseAddress}/UnitConfiguration/GetExcel?UserId={UserId}&status={status}";
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
                            var worksheet = workbook.Worksheets.Add("Unit Configuration");
                            var currentRow = 1;

                            worksheet.Cell(currentRow, 1).Value = "Unit";
                            worksheet.Cell(currentRow, 2).Value = "Height";
                            worksheet.Cell(currentRow, 3).Value = "Amount";
                            worksheet.Cell(currentRow, 4).Value = "Size";
                            worksheet.Cell(currentRow, 5).Value = "Width";
                            worksheet.Cell(currentRow, 6).Value = "Status";
                            if (dt == null)//(dt.Rows.Count == 0)
                            {
                                TempData["Message"] = "No data found to export!";
                                return Redirect($"/UnitConfiguration?status={status}");
                            }
                            else
                            {
                                for (int index = 1; index <= dt.Rows.Count; index++)
                                {
                                    worksheet.Cell(index + 1, 1).Value = dt.Rows[index - 1]["u_unit"].ToString();
                                    worksheet.Cell(index + 1, 2).Value = dt.Rows[index - 1]["u_height"].ToString();
                                    worksheet.Cell(index + 1, 3).Value = dt.Rows[index - 1]["u_amount"].ToString();
                                    worksheet.Cell(index + 1, 4).Value = dt.Rows[index - 1]["u_size"].ToString();
                                    worksheet.Cell(index + 1, 5).Value = dt.Rows[index - 1]["u_width"].ToString();
                                    worksheet.Cell(index + 1, 6).Value = dt.Rows[index - 1]["u_isactive"].ToString();
                                }
                            }
                            using (var stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                var content = stream.ToArray();
                                return File(
                                    content,
                                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    "Unit Configuration.xlsx");
                            }
                        }

                    }
                }
                else
                {
                    TempData["errorMessage"] = "Failed to retrieve data from the API.";
                    return Redirect($"/UnitConfiguration?status={status}");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Pdf(Guid? UserId,string status)
        {
            MemoryStream ms1 = new MemoryStream();
            try
            {
                UnitConfigurationModel model = new UnitConfigurationModel();
                UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                string data = JsonConvert.SerializeObject(model);
                string url = $"{_httpClient.BaseAddress}/UnitConfiguration/GetPdf?UserId={UserId}&status={status}";
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
                                return File(pdfBytes, "application/pdf", "UnitConfiguration.pdf");
                            }
                        }
                        else
                        {
                            TempData["errorMessage"] = "Failed to retrieve data from the API.";
                            return Redirect($"/UnitConfiguration?status={status}");
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to retrieve data from the API.";
                        return Redirect($"/UnitConfiguration?status={status}");
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
