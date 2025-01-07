using Common;
using ibillcraft.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Localization;
using PageSize = iTextSharp.text.PageSize;
using Document = iTextSharp.text.Document;
using ClosedXML.Excel;
using OfficeOpenXml;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json.Linq;
using System.Data;
using Azure;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Amazon.Runtime.SharedInterfaces;
using Microsoft.AspNetCore.Components.Forms;
using System.Net;
using System.IdentityModel.Tokens.Jwt;

namespace ibillcraft.Controllers
{
    [ExampleFilter1]

    public class CustomerMasterController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CustomerMasterController> _logger;
        private readonly IStringLocalizer<CustomerMasterController> _localizer;
        private readonly string _uploadPath;
        public CustomerMasterController(ILogger<CustomerMasterController> logger, IStringLocalizer<CustomerMasterController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);
            _logger = logger;
            _localizer = localizer;
            string basePath = configuration["UploadSettings:BasePath"];
            string folderName = configuration["UploadSettings:FolderName"];
            string uploadPath = Path.Combine(basePath, folderName);

            // Optionally store uploadPath if needed elsewhere in the controller
            _uploadPath = uploadPath;
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
				GetCookies gk = new GetCookies();
				CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);
				// Check if session data is available

				if (string.IsNullOrEmpty(CUtility.comid))
				{
					// Handle missing session data
					return RedirectToAction("Index", "CompanyLoginRegistration");
				}
				ViewBag.Format = CUtility.format;
				Guid UserId = CUtility.rolename == "Admin" ? new Guid(CUtility.comid) : new Guid(CUtility.userid);

				var CustomerDataList = new List<CustomerMasterModel>();
                string url = $"{_httpClient.BaseAddress}/CustomerMaster/GetAll?UserId={UserId}&Server_Value={CUtility.serverValue}&status={status}&CompanyId={CUtility.comid}";
                HttpResponseMessage response = _httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<CustomerMasterModel>() };
                    var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    CustomerDataList = response2.data;
                    if (CustomerDataList != null)
                    {
                        return View("Index", CustomerDataList);
                    }
                    else
                    {
                        var CustomerDataList2 = new List<CustomerMasterModel>();
                        return View("Index", CustomerDataList2);
                    }
                }
                return View("Index", CustomerDataList);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                throw;
            }
        }

    


        public ActionResult EditIndex1(Guid id)
        {
			GetCookies gk = new GetCookies();
			CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);
            // Check if session data is available
            string eurl = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_ParameterValueMaster&sValue=pv_parametervalue&id=pv_id&IsActiveColumn=pv_isactive&sCoulmnName=pv_parameterid&sColumnValue=16aa2b3f-76db-44d2-bbd4-2dc7a4d0e430";
            HttpResponseMessage eresponseView = _httpClient.GetAsync(eurl).Result;
            dynamic edata = eresponseView.Content.ReadAsStringAsync().Result;
            var erootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(edata);
            ViewBag.c_type = erootObject;


            if (string.IsNullOrEmpty(CUtility.comid))
			{
				// Handle missing session data
				return RedirectToAction("Index", "CompanyLoginRegistration");
			}
			ViewBag.Format = CUtility.format;
			Guid UserId = CUtility.rolename == "Admin" ? new Guid(CUtility.comid) : new Guid(CUtility.userid);
           // ViewBag.co_country_code = CUtility.co_country_code;
            CustomerMasterModel model = new CustomerMasterModel();
            try
            {
               
                return View();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpPost]
       
        public JsonResult Create([FromForm]CustomerMasterModel model)
        {
            try
            {
                GetCookies gk = new GetCookies();
                CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);
                // Check if session data is available

                if (string.IsNullOrEmpty(CUtility.comid))
                {
                    // Handle missing session data
                    return Json("Index", "CompanyLoginRegistration");
                }
                ViewBag.Format = CUtility.format;
                Guid UserId = CUtility.rolename == "Admin" ? new Guid(CUtility.comid) : new Guid(CUtility.userid);

                SendMail sendmail = new SendMail();
                DateTime? currentTime = sendmail.findtime(CUtility.co_timezone);

                model.UserId = UserId;
                model.c_com_id = CUtility.comid;
                //model.Server_Value = CUtility.serverValue;
                model.c_createddate = currentTime;
                model.c_updateddate = currentTime;


                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                //HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/Purchase/Insert", content).Result;
                // HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/Purchase", content).Result;
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/CustomerMaster/Insert", content).Result;
                // HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/Purchase", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic responsedata = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(responsedata);
                    dynamic responsemodel = rootObject.outcome;
                    string outcomeDetail = responsemodel.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail;
                    return Json(outcomeDetail);
                }
                TempData["errorMessage"] = response.Headers.ToString();
                return Json("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return Json("Index");
            }
        }



        [HttpPost]
        public JsonResult UploadFile(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    // Define the upload folder path
                   // string uploadPath = Path.Combine(@"E:\inetpub\wwwroot\cityMarine", "upload");

                   
                    // Create the directory if it doesn't exist
                    if (!Directory.Exists(_uploadPath))
                    {
                        Directory.CreateDirectory(_uploadPath);
                    }

                    // Generate the full file path
                    string filePath = Path.Combine(_uploadPath, file.FileName);

                    // Save the file to the server
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    // Return success with the virtual path
                   // string virtualPath = $"E:/inetpub/wwwroot/cityMarine/upload/{file.FileName}";
                   // return Json(new { success = true, filePath = virtualPath });
                    return Json(new { success = true, filePath = Path.Combine(_uploadPath, file.FileName) });
                }

                return Json(new { success = false, message = "No file was uploaded." });
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an error message
                return Json(new { success = false, message = ex.Message });
            }
        }

       

       


        [HttpGet]
        public IActionResult Edit(string? c_id)
        {
            try
            {
                GetCookies gk = new GetCookies();
                CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);
                // Check if session data is available

                if (string.IsNullOrEmpty(CUtility.comid))
                {
                    // Handle missing session data
                    return RedirectToAction("Index", "CompanyLoginRegistration");
                }
                ViewBag.Format = CUtility.format;
                ViewBag.comid = CUtility.comid;
                Guid UserId = CUtility.rolename == "Admin" ? new Guid(CUtility.comid) : new Guid(CUtility.userid);
               // ViewBag.co_country_code = CUtility.co_country_code;
                string eurl = $"{_httpClient.BaseAddress}/ViewBag/GetViewBag?userId&sTableName=tbl_ParameterValueMaster&sValue=pv_parametervalue&id=pv_id&IsActiveColumn=pv_isactive&sCoulmnName=pv_parameterid&sColumnValue=16aa2b3f-76db-44d2-bbd4-2dc7a4d0e430";
                HttpResponseMessage eresponseView = _httpClient.GetAsync(eurl).Result;
                dynamic edata = eresponseView.Content.ReadAsStringAsync().Result;
                var erootObject = JsonConvert.DeserializeObject<List<FillDropdown>>(edata);
                ViewBag.c_type = erootObject;


                string orderurl = $"{_httpClient.BaseAddress}/CustomerMaster/Get?UserId={UserId}&Server_Value={CUtility.serverValue}&c_id={c_id}";
                string detailsurl = $"{_httpClient.BaseAddress}/CustomerMaster/GetDetails?UserId={UserId}&Server_Value={CUtility.serverValue}&c_id={c_id}";
                HttpResponseMessage response = _httpClient.GetAsync(orderurl).Result;
                HttpResponseMessage orderponse = _httpClient.GetAsync(detailsurl).Result;
                var orderModelList = new List<CustomerAttachment>();
                var orderModel = new CustomerMasterModel();
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new CustomerMasterModel() };
                    var responses = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    dynamic cdata = orderponse.Content.ReadAsStringAsync().Result;
                    var cdataObject = new { data = new List<CustomerAttachment>() };
                    var cresponses = JsonConvert.DeserializeAnonymousType(cdata, cdataObject);
                    orderModel = responses.data;
                   
                    orderModelList = cresponses.data;
                    if (orderModelList != null && orderModel != null)
                    {
                        var viewModel = new Tuple<CustomerMasterModel, IEnumerable<CustomerAttachment>>(orderModel, orderModelList);
                        return View("EditIndex1", viewModel);
                    }
                    else
                    {
                        return View("EditIndex1");
                    }
                }
                return View("EditIndex1");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("EditIndex1");
            }
        }


        //[HttpDelete]
        public IActionResult Delete(string c_id)
        {
            try
            {
                CustomerMasterModel model = new CustomerMasterModel();
				GetCookies gk = new GetCookies();
				CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);
				// Check if session data is available

				if (string.IsNullOrEmpty(CUtility.comid))
				{
					// Handle missing session data
					return RedirectToAction("Index", "CompanyLoginRegistration");
				}
				ViewBag.Format = CUtility.format;
				Guid UserId = CUtility.rolename == "Admin" ? new Guid(CUtility.comid) : new Guid(CUtility.userid);

				model.UserId = UserId;
                model.c_id = c_id;
                model.Server_Value = CUtility.serverValue;
                model.c_com_id = CUtility.comid;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/CustomerMaster/Delete", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data2 = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(data2);
                    dynamic model2 = rootObject.outcome;
                    string outcomeDetail = model2.outcomeDetail;
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


        [HttpGet]
        public IActionResult DownloadFile(string filePath)
        {
            // Check if the file path is provided
            if (string.IsNullOrEmpty(filePath))
            {
                return BadRequest("File path cannot be null or empty.");
            }

            // Ensure the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            try
            {
                // Get the file name
                var fileName = Path.GetFileName(filePath);

                // Determine the MIME type based on the file extension
                var contentType = GetMimeType(filePath);

                // Read the file into a byte array
                var fileBytes = System.IO.File.ReadAllBytes(filePath);

                // Return the file as a download
                return File(fileBytes, contentType, fileName);
            }
            catch (Exception ex)
            {
                // Handle errors (e.g., log the error)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Helper method to get the MIME type
        private string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath)?.ToLowerInvariant();

            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".pdf" => "application/pdf",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".txt" => "text/plain",
                ".csv" => "text/csv",
                _ => "application/octet-stream", // Default MIME type for unknown extensions
            };
        }

        [HttpGet]
        public async Task<IActionResult> Excel(string Status)
        {
            try
            {
                var customerDataList = new List<CustomerMasterModel>();
				GetCookies gk = new GetCookies();
				CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);
				// Check if session data is available

				if (string.IsNullOrEmpty(CUtility.comid))
				{
					// Handle missing session data
					return RedirectToAction("Index", "CompanyLoginRegistration");
				}
				ViewBag.Format = CUtility.format;
				Guid UserId = CUtility.rolename == "Admin" ? new Guid(CUtility.comid) : new Guid(CUtility.userid);

				string url = $"{_httpClient.BaseAddress}/CustomerMaster/GetExcel?UserId={UserId}&Server_Value={CUtility.serverValue}&status={Status}&CompanyId={CUtility.comid}";
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
                            var worksheet = workbook.Worksheets.Add("Client Details");
                            var currentRow = 1;
                            worksheet.Cell(currentRow, 1).Value = "Name";
                            worksheet.Cell(currentRow, 2).Value = "Code";
                            worksheet.Cell(currentRow, 3).Value = "Address";
                            worksheet.Cell(currentRow, 4).Value = "Contact";
                            worksheet.Cell(currentRow, 5).Value = "Alternate Contact";
                            worksheet.Cell(currentRow, 6).Value = "DOB";
                            worksheet.Cell(currentRow, 7).Value = "Aniversary Date";
                            worksheet.Cell(currentRow, 8).Value = "Email";
                            worksheet.Cell(currentRow, 9).Value = "Status";

                            if (dt == null)
                            {
                                TempData["Message"] = "No data found to export!";
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                for (int index = 1; index <= dt.Rows.Count; index++)
                                {
                                    worksheet.Cell(index + 1, 1).Value = dt.Rows[index - 1]["c_name"].ToString();
                                    worksheet.Cell(index + 1, 2).Value = dt.Rows[index - 1]["c_ccode"].ToString();
                                    worksheet.Cell(index + 1, 3).Value = dt.Rows[index - 1]["c_address"].ToString();
                                    worksheet.Cell(index + 1, 4).Value = dt.Rows[index - 1]["c_contact"].ToString();
                                    worksheet.Cell(index + 1, 5).Value = dt.Rows[index - 1]["c_contact2"].ToString();
                                    worksheet.Cell(index + 1, 6).Value = dt.Rows[index - 1]["c_dob"].ToString();
                                    worksheet.Cell(index + 1, 7).Value = dt.Rows[index - 1]["c_anidate"].ToString();
                                    worksheet.Cell(index + 1, 8).Value = dt.Rows[index - 1]["c_email"].ToString();
                                  
                                    worksheet.Cell(index + 1, 9).Value = dt.Rows[index - 1]["c_isactive"].ToString();
                                }
                            }
                            using (var stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                var content = stream.ToArray();
                                return File(
                                    content,
                                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    "Client Details.xlsx");

                                

                            }
                           
                        }

                    }
                }

                else
                {
                    TempData["errorMessage"] = "Failed to retrieve data from the API.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Pdf(string Status)
        {
            MemoryStream ms1 = new MemoryStream();
            try
            {
                CustomerMasterModel model = new CustomerMasterModel();
				GetCookies gk = new GetCookies();
				CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);
				// Check if session data is available

				if (string.IsNullOrEmpty(CUtility.comid))
				{
					// Handle missing session data
					return RedirectToAction("Index", "CompanyLoginRegistration");
				}
				ViewBag.Format = CUtility.format;
				Guid UserId = CUtility.rolename == "Admin" ? new Guid(CUtility.comid) : new Guid(CUtility.userid);

				model.UserId = UserId;
                model.c_isactive = Status;
                model.c_com_id = CUtility.comid;
                string data = JsonConvert.SerializeObject(model);
                string url = $"{_httpClient.BaseAddress}/CustomerMaster/GetPdf?UserId={UserId}&Server_Value={CUtility.serverValue}&status={Status}&CompanyId={CUtility.comid}";
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
                                return File(pdfBytes, "application/pdf", "Client Details.pdf");
                            }
                        }
                        else
                        {
                            TempData["errorMessage"] = "No data found to export!";
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to retrieve data from the API.";
                        return RedirectToAction("Index");
                    }

            }

            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            return new FileStreamResult(ms1, "application/pdf");
        }

        //[HttpPost]
        //public IActionResult UpdateStatus(string c_id, string c_isactive)
        //{
        //    try
        //    {
        //        CustomerMasterModel model = new CustomerMasterModel();
        //        GetCookies gk = new GetCookies();
        //        CookiesUtility CUtility = gk.GetCookiesvalue(Request.Cookies["jwtToken"]);
        //        // Check if session data is available

        //        if (string.IsNullOrEmpty(CUtility.comid))
        //        {
        //            // Handle missing session data
        //            return RedirectToAction("Index", "CompanyLoginRegistration");
        //        }
        //        ViewBag.Format = CUtility.format;
        //        Guid UserId = CUtility.rolename == "Admin" ? new Guid(CUtility.comid) : new Guid(CUtility.userid);

        //        model.c_com_id = CUtility.comid;
        //        model.UserId = UserId;
        //        model.c_id = c_id;
        //        model.c_isactive = c_isactive;
        //        model.c_updateddate = DateTime.Now;
        //        model.Server_Value = CUtility.serverValue;
        //        string data = JsonConvert.SerializeObject(model);
        //        StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
        //        HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/CustomerMaster/Insert", content).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            dynamic data2 = response.Content.ReadAsStringAsync().Result;
        //            dynamic rootObject = JsonConvert.DeserializeObject<object>(data2);
        //            dynamic model2 = rootObject.outcome;
        //            string outcomeDetail = model2.outcomeDetail;
        //            TempData["successMessage"] = outcomeDetail;
        //            return Ok(outcomeDetail);
        //        }
        //        TempData["errorMessage"] = response.Headers.ToString();
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["errorMessage"] = ex.Message;
        //        return RedirectToAction("Index");
        //    }
        //}




    }
}
