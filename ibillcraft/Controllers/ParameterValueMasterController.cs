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
    public class ParameterValueMasterController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ParameterValueMasterController> _logger;
        private readonly IStringLocalizer<ParameterValueMasterController> _localizer;
        public ParameterValueMasterController(ILogger<ParameterValueMasterController> logger, IStringLocalizer<ParameterValueMasterController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);
            _logger = logger;
            _localizer = localizer;
        }

        [HttpGet]
        public IActionResult Index(string ParameterId,string status)
        {
            try
            {
                if (status == null)
                {
                    status = "1";
                }
                var ParameterValueDataList = new List<ParameterValueMasterModel>();
                Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                string url = $"{_httpClient.BaseAddress}/ParameterValueMaster/GetAll?UserId={UserId}&Parameterid={ParameterId}&status={status}";
                HttpResponseMessage response = _httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<ParameterValueMasterModel>() };
                    var seresponse = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    ParameterValueDataList = seresponse.data;
                    if (ParameterValueDataList != null)
                    {
                        return View(ParameterValueDataList);
                    }
                    else
                    {
                        var ParameterValueList = new List<ParameterValueMasterModel>();
                        return View(ParameterValueList);
                    }
                }
                return View(ParameterValueDataList);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                throw;
            }
        }

        [HttpPost]
        public IActionResult Create(ParameterValueMasterModel model)
        {
            try
            {
                Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                model.pv_createddate = DateTime.Now;
                model.pv_updateddate = DateTime.Now;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/ParameterValueMaster", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    dynamic sedata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(sedata);
                    dynamic semodel = rootObject.outcome;
                    string outcomeDetail = semodel.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail;
                    return Ok(outcomeDetail);
                }
                return Redirect($"/ParameterValueMaster?ParameterId={model.pv_parameterid}&ParameterName={model.pv_parametername}");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Redirect($"/ParameterValueMaster?ParameterId={model.pv_parameterid}&ParameterName={model.pv_parametername}");
            }
        }

        public IActionResult Delete(Guid? pv_id,string ParameterId,string ParameterName)
        {
            try
            {
                ParameterValueMasterModel model = new ParameterValueMasterModel();
                Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                model.pv_id = pv_id;
                model.pv_parameterid = ParameterId;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/ParameterValueMaster/Delete", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic sedata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(sedata);
                    dynamic semodel = rootObject.outcome;
                    string outcomeDetail = semodel.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail; //.outcome.outcomeid;
                    return Redirect($"/ParameterValueMaster?ParameterId={ParameterId}&ParameterName={ParameterName}");
                }
                return Redirect($"/ParameterValueMaster?ParameterId={ParameterId}&ParameterName={ParameterName}");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Redirect($"/ParameterValueMaster?ParameterId={ParameterId}&ParameterName={ParameterName}");
            }
        }

        public async Task<IActionResult> Excel(Guid? UserId, string status, string ParameterId,string ParameterName)
        {
            try
            {
                UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                string url = $"{_httpClient.BaseAddress}/ParameterValueMaster/GetExcel?UserId={UserId}&status={status}&ParameterId={ParameterId}";
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
                            var worksheet = workbook.Worksheets.Add("Parameter Value Master");
                            var currentRow = 1;
                            worksheet.Cell(currentRow, 1).Value = "Parameter Name";
                            worksheet.Cell(currentRow, 2).Value = "Parameter Value Code";
                            worksheet.Cell(currentRow, 3).Value = "Parameter Value Name";
                            worksheet.Cell(currentRow, 4).Value = "Status";
                            if (dt == null)//(dt.Rows.Count == 0)
                            {
                                TempData["errorMessage"] = "No data found to export!";
                                return Redirect($"/ParameterValueMaster?status={status}&ParameterId={ParameterId}&ParameterName={ParameterName}");
                            }
                            else
                            {
                                for (int index = 1; index <= dt.Rows.Count; index++)
                                {
                                    worksheet.Cell(index + 1, 1).Value = dt.Rows[index - 1]["pv_parametervalue"].ToString();
                                    worksheet.Cell(index + 1, 2).Value = dt.Rows[index - 1]["pv_code"].ToString();
                                    worksheet.Cell(index + 1, 3).Value = dt.Rows[index - 1]["pv_parametername"].ToString();
                                    worksheet.Cell(index + 1, 4).Value = dt.Rows[index - 1]["pv_isactive"].ToString();
                                }
                            }
                            using (var stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                var content = stream.ToArray();
                                return File(
                                    content,
                                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    "Parameter Value Master.xlsx");
                            }
                        }
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Failed to retrieve data from the API.";
                    return Redirect($"/ParameterValueMaster?status={status}&ParameterId={ParameterId}&ParameterName={ParameterName}");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return BadRequest(ex.Message);
            }
        }
        public async Task<IActionResult> Pdf(Guid? UserId, string status, string ParameterId, string ParameterName)
        {
            MemoryStream ms1 = new MemoryStream();
            try
            {
                UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                string url = $"{_httpClient.BaseAddress}/ParameterValueMaster/GetPdf?UserId={UserId}&status={status}&ParameterId={ParameterId}";
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
                                return File(pdfBytes, "application/pdf", "ParameterValueMaster.pdf");
                            }
                        }
                        else
                        {
                            TempData["errorMessage"] = "No data found to export!";
                            return Redirect($"/ParameterValueMaster?status={status}&ParameterId={ParameterId}&ParameterName={ParameterName}");
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to retrieve data from the API.";
                        return Redirect($"/ParameterValueMaster?status={status}&ParameterId={ParameterId}&ParameterName={ParameterName}");
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
