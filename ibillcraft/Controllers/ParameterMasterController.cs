using ClosedXML.Excel;
using ibillcraft.Models;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System.Text;
using PageSize = iTextSharp.text.PageSize;
using Document = iTextSharp.text.Document;
using System.Data;
using Microsoft.Extensions.Localization;

namespace ibillcraft.Controllers
{
    public class ParameterMasterController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ParameterMasterController> _logger;
        private readonly IStringLocalizer<ParameterMasterController> _localizer;
        public ParameterMasterController(ILogger<ParameterMasterController> logger, IStringLocalizer<ParameterMasterController> localizer, IConfiguration configuration)
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
                var ParameterDataList = new List<ParameterMasterModel>(); ;
                Guid? UserId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6");
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/ParameterMaster/GetAll?UserId=" + UserId + "&status=" + status).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<ParameterMasterModel>() };
                    var suresponse = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    ParameterDataList = suresponse.data;
                    if (ParameterDataList != null)
                    {
                        return View(ParameterDataList);
                    }
                    else
                    {
                        var ParameterDataListse = new List<ParameterValueMasterModel>();
                        return View(ParameterDataListse);
                    }
                }
                return View(ParameterDataList);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                throw;
            }
        }
        [HttpPost]
        public IActionResult Create(ParameterMasterModel model)
        {
            try
            {
                Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                model.p_createddate = DateTime.Now;
                model.p_updateddate = DateTime.Now;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/ParameterMaster", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic sudata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(sudata);
                    dynamic sumodel = rootObject.outcome;
                    string outcomeDetail = sumodel.outcomeDetail;
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
        public IActionResult Delete(Guid? p_id)
        {
            try
            {
                ParameterMasterModel model = new ParameterMasterModel();
                Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                model.p_id = p_id;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/ParameterMaster/Delete", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data2 = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(data2);
                    dynamic model2 = rootObject.outcome;
                    string outcomeDetail = model2.outcomeDetail;
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

        public async Task<IActionResult> Excel(Guid? UserId, string status)
        {
            try
            {
                var parameterDataList = new List<ParameterMasterModel>();
                UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                string url = $"{_httpClient.BaseAddress}/ParameterMaster/GetExcel?UserId={UserId}&status={status}";
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
                            var worksheet = workbook.Worksheets.Add("Parameter Master");
                            var currentRow = 1;
                            worksheet.Cell(currentRow, 1).Value = "Parameter Code";
                            worksheet.Cell(currentRow, 2).Value = "Parameter Name";
                            worksheet.Cell(currentRow, 3).Value = "Status";
                            if (dt == null)//(dt.Rows.Count == 0)
                            {
                                TempData["errorMessage"] = "No data found to export!";
                                return Redirect($"/ParameterMaster?status={status}");
                            }
                            else
                            {
                                for (int index = 1; index <= dt.Rows.Count; index++)
                                {
                                    worksheet.Cell(index + 1, 1).Value = dt.Rows[index - 1]["p_code"].ToString();
                                    worksheet.Cell(index + 1, 2).Value = dt.Rows[index - 1]["p_parametername"].ToString();
                                    worksheet.Cell(index + 1, 3).Value = dt.Rows[index - 1]["p_isactive"].ToString();
                                }
                            }
                            using (var stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                var content = stream.ToArray();
                                return File(
                                    content,
                                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    "Parameter Master.xlsx");
                            }
                        }
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Failed to retrieve data from the API.";
                    return Redirect($"/ParameterMaster?status={status}");
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
                ParameterMasterModel model = new ParameterMasterModel();
                UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                string data = JsonConvert.SerializeObject(model);
                string url = $"{_httpClient.BaseAddress}/ParameterMaster/GetPdf?UserId={UserId}&status={status}";
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
                                return File(pdfBytes, "application/pdf", "ParameterMaster.pdf");
                            }
                        }
                        else
                        {
                            TempData["errorMessage"] = "No data found to export!";
                            return Redirect($"/ParameterMaster?status={status}");
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to retrieve data from the API.";
                        return Redirect($"/ParameterMaster?status={status}");
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
