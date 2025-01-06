using ClosedXML.Excel;
using Common;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Text;
using ibillcraft.Models;
using System.Data;
using iTextSharp.text;

namespace ibillcraft.Controllers
{
    public class EmailListController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EmailListController> _logger;
        private readonly IStringLocalizer<EmailListController> _localizer;
        public EmailListController(ILogger<EmailListController> logger, IStringLocalizer<EmailListController> localizer, IConfiguration configuration)
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
               

                var ParameterDataList = new List<EmailList>(); ;
                Guid? UserId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6");
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/EmailList/GetAll?UserId=" + UserId ).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<EmailList>() };
                    var suresponse = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    ParameterDataList = suresponse.data;
                    if (ParameterDataList != null)
                    {
                        return View(ParameterDataList);
                    }
                    else
                    {
                        var ParameterDataListse = new List<EmailList>();
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
        public IActionResult Create(EmailList model)
        {
            try
            {
                Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                model.E_createddate = DateTime.Now;
                model.E_updateddate = DateTime.Now;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/EmailList", content).Result;
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

        public IActionResult Delete(Guid? E_id)
        {
            try
            {
                EmailRuleConfg model = new EmailRuleConfg();
                Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                model.E_id = E_id;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/EmailList/Delete", content).Result;
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

        public async Task<IActionResult> Excel(Guid? UserId)
        {
            try
            {
                var parameterDataList = new List<EmailList>();
                UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                string url = $"{_httpClient.BaseAddress}/EmailList/GetExcel?UserId={UserId}";
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
                            var worksheet = workbook.Worksheets.Add("E-mail List");
                            var currentRow = 1;
                            worksheet.Cell(currentRow, 1).Value = "Email Id ";
                            worksheet.Cell(currentRow, 2).Value = "Password";
                            worksheet.Cell(currentRow, 3).Value = "SMTP Host";
                            worksheet.Cell(currentRow, 4).Value = "SMTP Port";
                            worksheet.Cell(currentRow, 5).Value = "IMAP Host ";
                            worksheet.Cell(currentRow, 6).Value = "IMAP Port";
                            worksheet.Cell(currentRow, 7).Value = "POP3 Host";
                            worksheet.Cell(currentRow, 8).Value = "POP3 Port";
                            worksheet.Cell(currentRow, 9).Value = "Oauth Key";
                            worksheet.Cell(currentRow, 10).Value = "SSL Enable";
                            worksheet.Cell(currentRow, 11).Value = "Status";
                            if (dt == null)//(dt.Rows.Count == 0)
                            {
                                TempData["errorMessage"] = "No data found to export!";
                                return Redirect($"/EmailList");
                            }
                            else
                            {
                                for (int index = 1; index <= dt.Rows.Count; index++)
                                {
                                    worksheet.Cell(index + 1, 1).Value = dt.Rows[index - 1]["E_email"].ToString();
                                    worksheet.Cell(index + 1, 2).Value = dt.Rows[index - 1]["E_password"].ToString();
                                    worksheet.Cell(index + 1, 3).Value = dt.Rows[index - 1]["E_smtph"].ToString();
                                    worksheet.Cell(index + 1, 4).Value = dt.Rows[index - 1]["E_smtpp"].ToString();
                                    worksheet.Cell(index + 1, 5).Value = dt.Rows[index - 1]["E_imaph"].ToString();
                                    worksheet.Cell(index + 1, 6).Value = dt.Rows[index - 1]["E_imapp"].ToString();
                                    worksheet.Cell(index + 1, 7).Value = dt.Rows[index - 1]["E_poph"].ToString();
                                    worksheet.Cell(index + 1, 8).Value = dt.Rows[index - 1]["E_popp"].ToString();
                                    worksheet.Cell(index + 1, 9).Value = dt.Rows[index - 1]["E_key"].ToString();
                                    worksheet.Cell(index + 1, 10).Value = dt.Rows[index - 1]["E_issslEnable"].ToString();
                                    worksheet.Cell(index + 1, 11).Value = dt.Rows[index - 1]["E_isactive"].ToString();
                                }
                            }
                            using (var stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                var content = stream.ToArray();
                                return File(
                                    content,
                                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    "EmailList.xlsx");
                            }
                        }
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Failed to retrieve data from the API.";
                    return Redirect($"/EmailList");
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
                EmailList model = new EmailList();
                UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                string data = JsonConvert.SerializeObject(model);
                string url = $"{_httpClient.BaseAddress}/EmailList/GetPdf?UserId={UserId}";
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
                            return File(pdfBytes, "application/pdf", "EmailList.pdf");
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "No data found to export!";
                        return Redirect($"/EmailList");
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Failed to retrieve data from the API.";
                    return Redirect($"/EmailList");
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
