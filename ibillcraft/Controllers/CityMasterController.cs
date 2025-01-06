using ibillcraft.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Localization;
using ClosedXML.Excel;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System.Data;
using PageSize = iTextSharp.text.PageSize;
using Document = iTextSharp.text.Document;
using DocumentFormat.OpenXml.Bibliography;

namespace ibillcraft.Controllers
{
    public class CityMasterController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CityMasterController> _logger;
        private readonly IStringLocalizer<CityMasterController> _localizer;

        public CityMasterController(ILogger<CityMasterController> logger, IStringLocalizer<CityMasterController> localizer, IConfiguration configuration)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(configuration.GetSection("Server:Master").Value);
            _logger = logger;
            _localizer = localizer;
        }
        public IActionResult Index(string CountryId, string CountryName, string StateId, string StateName, string status)
        {
            try
            {
                if (status == null)
                {
                    status = "1";
                }
                var cityDataList = new List<CityMasterModel>();
                Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                ViewBag.ci_country_id = CountryId;
                ViewBag.ci_country_name = CountryName;
                ViewBag.ci_state_id = StateId;
                ViewBag.ci_state_name = StateName;
                string url = $"{_httpClient.BaseAddress}/CityMaster/GetCity?UserId={UserId}&status={status}&CountryId={CountryId}&StateId={StateId}";
                HttpResponseMessage responseView = _httpClient.GetAsync(url).Result;
                List<CityMasterModel> citylist = new List<CityMasterModel>();
                HttpResponseMessage response = _httpClient.GetAsync(url).Result;//Vinit

                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var dataObject = new { data = new List<CityMasterModel>() };
                    var response2 = JsonConvert.DeserializeAnonymousType(data, dataObject);
                    cityDataList = response2.data;
                    if (cityDataList != null)
                    {
                        return View(cityDataList);
                    }
                    else
                    {
                        var cityDataList2 = new List<CityMasterModel>();
                        return View(cityDataList2);
                    }
                }
                return View(cityDataList);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }
        }

        [HttpPost]
        public IActionResult Create(CityMasterModel model)
        {
            try
            {
                Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                model.ci_createddate = DateTime.Now;
                model.ci_updateddate = DateTime.Now;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/CityMaster", content).Result;
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
        public IActionResult Edit(Guid? ci_id)
        {
            Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
            CityMasterModel model = new CityMasterModel();
            try
            {
                string url = $"{_httpClient.BaseAddress}/CityMaster/GetCityById?ci_id={ci_id}&UserId={UserId}";
                HttpResponseMessage response = _httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data = response.Content.ReadAsStringAsync().Result;
                    var rootObject = JsonConvert.DeserializeObject<City>(data);
                    model = rootObject.data;
                }
                return Ok(model);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("ErrorIndex", "Home");
            }
        }
        public IActionResult Delete(Guid? ci_id, string ci_country_id, string ci_country_name, string ci_state_id, string ci_state_name)
        {
            try
            {
                CityMasterModel model = new CityMasterModel();
                Guid? UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                model.ci_id = ci_id;
                model.ci_country_id = ci_country_id;
                model.ci_country_name = ci_country_name;
                model.ci_state_name = ci_state_name;
                model.ci_state_id = ci_state_id;
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/CityMaster/DeleteCity", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic data2 = response.Content.ReadAsStringAsync().Result;
                    dynamic rootObject = JsonConvert.DeserializeObject<object>(data2);
                    dynamic model2 = rootObject.outcome;
                    string outcomeDetail = model2.outcomeDetail;
                    TempData["successMessage"] = outcomeDetail;//.outcome.outcomeid;
                    TempData["CountryId"] = ci_country_id;
                    TempData["CountryName"] = ci_country_name;
                    TempData["StateId"] = ci_state_id;
                    TempData["StateName"] = ci_state_name;
                    return Redirect($"/CityMaster?CountryId={ci_country_id}&CountryName={ci_country_name}&StateId={ci_state_id}&StateName={ci_state_name}");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
        public async Task<IActionResult> Excel(Guid? UserId, string status, string CountryId, string CountryName, string StateId, string StateName)
        {
            try
            {
                var cityDataList = new List<CityMasterModel>();
                UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                string url = $"{_httpClient.BaseAddress}/CityMaster/GetExcel?UserId={UserId}&status={status}&CountryId={CountryId}&CountryName={CountryName}&StateId={StateId}&StateName={StateName}";
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
                            var worksheet = workbook.Worksheets.Add("City Master");
                            var currentRow = 1;
                            worksheet.Cell(currentRow, 1).Value = "Country Name";
                            worksheet.Cell(currentRow, 2).Value = "State Name";
                            worksheet.Cell(currentRow, 3).Value = " code";
                            worksheet.Cell(currentRow, 4).Value = " Name";
                            worksheet.Cell(currentRow, 5).Value = "Status";
                            if (dt == null)//(dt.Rows.Count == 0)
                            {
                                TempData["errorMessage"] = "No data found to export!";
                                return Redirect($"/CityMaster?CountryId={CountryId}&CountryName={CountryName}&StateId={StateId}&StateName={StateName}&status={status}");
                            }
                            else
                            {
                                for (int index = 1; index <= dt.Rows.Count; index++)
                                {
                                    worksheet.Cell(index + 1, 1).Value = dt.Rows[index - 1]["ci_country_name"].ToString();
                                    worksheet.Cell(index + 1, 2).Value = dt.Rows[index - 1]["ci_state_name"].ToString();
                                    worksheet.Cell(index + 1, 3).Value = dt.Rows[index - 1]["ci_city_code"].ToString();
                                    worksheet.Cell(index + 1, 4).Value = dt.Rows[index - 1]["ci_city_name"].ToString();
                                    worksheet.Cell(index + 1, 5).Value = dt.Rows[index - 1]["ci_isactive"].ToString();
                                }
                            }
                            using (var stream = new MemoryStream())
                            {
                                workbook.SaveAs(stream);
                                var content = stream.ToArray();
                                return File(
                                    content,
                                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    "City Master.xlsx");
                            }
                        }
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Failed to retrieve data from the API.";
                    return Redirect($"/CityMaster?CountryId={CountryId}&CountryName={CountryName}&StateId={StateId}&StateName={StateName}&status={status}");
                }
            }

            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return BadRequest(ex.Message);
            }


        }

        public async Task<IActionResult> Pdf(Guid? UserId, string status, string CountryId, string CountryName, string StateId, string StateName)
        {
            MemoryStream ms1 = new MemoryStream();
            try
            {
                CityMasterModel model = new CityMasterModel();
                UserId = new Guid("990fcade-b7ba-4787-ad03-7dcf03a0ab05");
                model.UserId = UserId;
                string data = JsonConvert.SerializeObject(model);
                string url = $"{_httpClient.BaseAddress}/CityMaster/GetPdf?UserId={UserId}&status={status}&CountryId={CountryId}&CountryName={CountryName}&StateId={StateId}&StateName={StateName}";
                    HttpResponseMessage response = await _httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        //// return File(content, "application/pdf", "StateReport.pdf");
                        // return View(content);
                        string htmlContent = content.ToString();
                        if (htmlContent.Contains("<td"))
                        {
                            //var stream = new MemoryStream(System.IO.File.ReadAllBytes(path));
                            MemoryStream ms = new MemoryStream();
                            //{
                            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, ms);

                            pdfDoc.Open();

                            // Parse the HTML string and add it to the PDF
                            TextReader sr = new StringReader(htmlContent);
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);

                            pdfDoc.Close();

                            byte[] pdfBytes = ms.ToArray();

                            // Convert the byte array to base64
                            string base64Pdf = Convert.ToBase64String(pdfBytes);

                            ms1 = ms;
                            using (var stream = new MemoryStream())
                            {
                                //workbook.SaveAs(stream);

                                return File(pdfBytes, "application/pdf", "CityMaster.pdf");
                            }
                        }
                        else
                        {
                            // Handle unsuccessful API response
                            TempData["errorMessage"] = "No data found to export!";
                            return Redirect($"/CityMaster?CountryId={CountryId}&CountryName={CountryName}&StateId={StateId}&StateName={StateName}&status={status}");
                        }
                    }
                    else
                    {
                        TempData["errorMessage"] = "Failed to retrieve data from the API.";
                        return Redirect($"/CityMaster?CountryId={CountryId}&CountryName={CountryName}&StateId={StateId}&StateName={StateName}&status={status}");
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
