using ClosedXML.Excel;
using Common;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ibillcraft.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System.Data;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Tokens;

namespace ibillcraft.Controllers
{
	public class EmailRuleConfgController : Controller
	{
        private readonly HttpClient _httpClient;
        private readonly ILogger<EmailRuleConfgController> _logger;
        private readonly IStringLocalizer<EmailRuleConfgController> _localizer;
        public EmailRuleConfgController(ILogger<EmailRuleConfgController> logger, IStringLocalizer<EmailRuleConfgController> localizer, IConfiguration configuration)
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

            string geurl = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_ParameterValueMaster&sValue=pv_parametervalue&id=pv_id&IsActiveColumn=pv_isactive&sCoulmnName=pv_parameterid&sColumnValue=689388ff-67e4-4060-88dc-4bf68d4e069f";
            HttpResponseMessage geresponseView = _httpClient.GetAsync(geurl).Result;
            dynamic gedata = geresponseView.Content.ReadAsStringAsync().Result;
            var gerootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(gedata);
            ViewBag.E_parameter = gerootObject;

            string ceurl = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_ParameterValueMaster&sValue=pv_parametervalue&id=pv_id&IsActiveColumn=pv_isactive&sCoulmnName=pv_parameterid&sColumnValue=cb9d169a-6327-416e-b8dd-ce10a805bd82";
            HttpResponseMessage ceresponseView = _httpClient.GetAsync(ceurl).Result;
            dynamic cedata = ceresponseView.Content.ReadAsStringAsync().Result;
            var cerootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(cedata);
            ViewBag.E_condition = cerootObject;

            string eurl = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_ParameterValueMaster&sValue=pv_parametervalue&id=pv_id&IsActiveColumn=pv_isactive&sCoulmnName=pv_parameterid&sColumnValue=a7c24a46-2a70-43f4-bd80-fb652b28b71d";
            HttpResponseMessage eresponseView = _httpClient.GetAsync(eurl).Result;
            dynamic edata = eresponseView.Content.ReadAsStringAsync().Result;
            var erootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(edata);
            ViewBag.E_category = erootObject;


            try
            {
                if (status == null)
                {
                    status = "1";
                }

                var ParameterDataList = new List<EmailRuleConfg>(); ;
                Guid? UserId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6");
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/EmailRuleConfg/GetAll?UserId=" + UserId + "&status=" + status).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<EmailRuleConfg>() };
                    var suresponse = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    ParameterDataList = suresponse.data;
                    if (ParameterDataList != null)
                    {
                        return View(ParameterDataList);
                    }
                    else
                    {
                        var ParameterDataListse = new List<EmailRuleConfg>();
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
        public IActionResult Create(EmailRuleConfg model)
        {
            try
            {
                Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                model.E_createddate = DateTime.Now;
                model.E_updateddate = DateTime.Now;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/EmailRuleConfg", content).Result;
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
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/EmailRuleConfg/Delete", content).Result;
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
                var parameterDataList = new List<EmailRuleConfg>();
                UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                string url = $"{_httpClient.BaseAddress}/EmailRuleConfg/GetExcel?UserId={UserId}&status={status}";
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
                            var worksheet = workbook.Worksheets.Add("E-mail Rule Configuration");
                            var currentRow = 1;
                            worksheet.Cell(currentRow, 1).Value = "Parameter ";
                            worksheet.Cell(currentRow, 2).Value = "Condition";
                            worksheet.Cell(currentRow, 3).Value = "Category";
                            worksheet.Cell(currentRow, 4).Value = "Status";
                            if (dt == null)//(dt.Rows.Count == 0)
                            {
                                TempData["errorMessage"] = "No data found to export!";
                                return Redirect($"/EmailRuleConfg?status={status}");
                            }
                            else
                            {
                                for (int index = 1; index <= dt.Rows.Count; index++)
                                {
                                    worksheet.Cell(index + 1, 1).Value = dt.Rows[index - 1]["E_parameterName"].ToString();
                                    worksheet.Cell(index + 1, 2).Value = dt.Rows[index - 1]["E_conditionName"].ToString();
                                    worksheet.Cell(index + 1, 3).Value = dt.Rows[index - 1]["E_categoryName"].ToString();
                                    worksheet.Cell(index + 1, 4).Value = dt.Rows[index - 1]["E_isactive"].ToString();
                                }
                            }
                            using (var stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                var content = stream.ToArray();
                                return File(
                                    content,
                                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    "EmailRuleConfiguration.xlsx");
                            }
                        }
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Failed to retrieve data from the API.";
                    return Redirect($"/EmailRuleConfg?status={status}");
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
                EmailRuleConfg model = new EmailRuleConfg();
                UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                string data = JsonConvert.SerializeObject(model);
                string url = $"{_httpClient.BaseAddress}/EmailRuleConfg/GetPdf?UserId={UserId}&status={status}";
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
                            return File(pdfBytes, "application/pdf", "EmailRuleConfiguration.pdf");
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "No data found to export!";
                        return Redirect($"/EmailRuleConfg?status={status}");
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Failed to retrieve data from the API.";
                    return Redirect($"/EmailRuleConfg?status={status}");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            return new FileStreamResult(ms1, "application/pdf");
        }

        [HttpGet]
        public async Task<IActionResult> Download()
        {
            
            EmailRuleConfg model = new EmailRuleConfg();
            
            string data = JsonConvert.SerializeObject(model);
            string url = $"{_httpClient.BaseAddress}/EmailRuleConfg/download";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsByteArrayAsync();
                string fileName = $"Template-{DateTime.Now:yyyyMMddHHmmssfff}.xlsx";
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            else
            {
                return BadRequest("Failed to download Excel file from the API.");
            }
        }


        //[HttpPost]
        //public JsonResult UploadFile(IFormFile file, [FromServices] IConfiguration configuration)
        //{
        //    if (file == null || file.Length == 0)
        //    {
        //        return Json(new { success = false, message = "Please select a file to upload." });
        //    }

        //    //Guid userId = Guid.NewGuid(); // Replace with actual logic

        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        using (var formData = new MultipartFormDataContent())
        //        {
        //            using (var fileStream = file.OpenReadStream())
        //            {
        //                var fileContent = new StreamContent(fileStream);
        //                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
        //                formData.Add(fileContent, "file", file.FileName);
        //            }

        //            //formData.Add(new StringContent(userId.ToString()), "userId");

        //            try
        //            {
        //                var response = client.PostAsync("EmailRuleConfg/upload", formData).Result;

        //                if (response.IsSuccessStatusCode)
        //                {
        //                    var result = response.Content.ReadAsStringAsync().Result;
        //                    return Json(new { success = true, message = "File uploaded successfully!", data = result });
        //                }
        //                else
        //                {
        //                    var errorMessage = response.Content.ReadAsStringAsync().Result;
        //                    return Json(new { success = false, message = $"API Error: {errorMessage}" });
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { success = false, message = $"Error: {ex.Message}" });
        //            }
        //        }
        //    }
        //}


        //[HttpPost]
        //public async Task<string> UploadFile(IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //    {
        //        throw new ArgumentException("File cannot be null or empty.");
        //    }

        //    string url = $"{_httpClient.BaseAddress}/EmailRuleConfg/upload";

        //    // Create MultipartFormDataContent
        //    var formData = new MultipartFormDataContent();

        //    try
        //    {
        //        // Add the file to the form data
        //        var fileStream = file.OpenReadStream();
        //        var fileContent = new StreamContent(fileStream);

        //        // Set the content type
        //        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);

        //        // Add file content to form data
        //        formData.Add(fileContent, "file", file.FileName);

        //        // Add additional form data
              

        //        // Send the POST request
        //        HttpResponseMessage response = await _httpClient.PostAsync(url, formData);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            return await response.Content.ReadAsStringAsync();
        //        }
        //        else
        //        {
        //            string error = await response.Content.ReadAsStringAsync();
        //            throw new Exception($"API Error: {response.StatusCode} - {error}");
        //        }
        //    }
        //    finally
        //    {
        //        // Dispose of resources explicitly
        //        formData.Dispose();
        //    }
        //}

        [HttpPost]
        public async Task<string> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File cannot be null or empty.");
            }

            // Allowed file extensions
            var allowedExtensions = new[] { ".xls", ".xlsx", ".xlsm", ".csv" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                TempData["errorMessage"] = "Invalid file type";
               throw new ArgumentException($"Invalid file type. Only the following extensions are allowed: {string.Join(", ", allowedExtensions)}");
            }

            string url = $"{_httpClient.BaseAddress}/EmailRuleConfg/upload";

            // Create MultipartFormDataContent
            var formData = new MultipartFormDataContent();

            try
            {
                // Add the file to the form data
                var fileStream = file.OpenReadStream();
                var fileContent = new StreamContent(fileStream);

                // Set the content type
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);

                // Add file content to form data
                formData.Add(fileContent, "file", file.FileName);

                // Send the POST request
                HttpResponseMessage response = await _httpClient.PostAsync(url, formData);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API Error: {response.StatusCode} - {error}");
                }
            }
            finally
            {
                // Dispose of resources explicitly
                formData.Dispose();
            }
        }



    }
}
