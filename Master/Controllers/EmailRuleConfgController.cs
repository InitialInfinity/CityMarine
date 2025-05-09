using Common;
using Master.API.Entity;
using Master.API.Repository.Interface;
using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Org.BouncyCastle.Utilities;
using System.Data;
using System.Reflection.Metadata;
using System.Text;
using Tokens;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Web;

namespace Master.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailRuleConfgController : Controller
    {
        private readonly IEmailRuleConfgRepository _emailRuleConfgRepo;

        public EmailRuleConfgController(IEmailRuleConfgRepository emailRuleConfgRepo)
        {
            _emailRuleConfgRepo = emailRuleConfgRepo;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid user_id, string status)
        {
            try
            {
                EmailRuleConfg user = new EmailRuleConfg();
                user.UserId = user_id;
                user.E_isactive = status;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                var createduser = await _emailRuleConfgRepo.EmailRuleConfg(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get(Guid? user_id, Guid E_id)
        {
            EmailRuleConfg user = new EmailRuleConfg();
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.UserId = user_id;
            user.E_id = E_id;
            user.BaseModel.OperationType = "Get";
            try
            {
                var parameter = await _emailRuleConfgRepo.Get(user);
                return parameter;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] EmailRuleConfg user)
        {
            try
            {
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                if (user.E_id == null)
                {
                    user.BaseModel.OperationType = "Insert";
                }
                else
                {
                    user.E_updateddate = DateTime.Now;
                    user.BaseModel.OperationType = "Update";
                }
                var createduser = await _emailRuleConfgRepo.Get(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteDepartment([FromBody] EmailRuleConfg user)
        {
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.BaseModel.OperationType = "Delete";
            var productDetails = await _emailRuleConfgRepo.Get(user);
            return productDetails;
        }

        [HttpGet("GetExcel")]
        public async Task<IActionResult> GetExcel(Guid UserId, string status)
        {
            try
            {
                EmailRuleConfg user = new EmailRuleConfg();
                user.UserId = UserId;
                user.E_isactive = status;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                dynamic createduser = await _emailRuleConfgRepo.EmailRuleConfg(user);
                dynamic data1 = ((Tokens.Result)((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value).Data;
                DataTable data = new DataTable();
                if (data1 is List<object> dataList)
                {
                    if (dataList.Count > 0)
                    {
                        var firstItem = dataList[0] as IDictionary<string, object>;
                        if (firstItem != null)
                        {
                            foreach (var kvp in firstItem)
                            {
                                data.Columns.Add(kvp.Key);
                            }
                        }
                        foreach (var item in dataList)
                        {
                            var values = item as IDictionary<string, object>;
                            if (values != null)
                            {
                                var row = data.NewRow();
                                foreach (var kvp in values)
                                {
                                    row[kvp.Key] = kvp.Value;
                                }
                                data.Rows.Add(row);
                            }
                        }
                    }
                }
                ExportRepository export = new ExportRepository();
                var result = new Result
                {
                    Data = export.DataTableToJsonObj(data)
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("GetPdf")]
        public async Task<IActionResult> GetPdf(Guid? UserId, string status)
        {
            try
            {
                EmailRuleConfg user = new EmailRuleConfg();
                user.UserId = UserId;
                user.E_isactive = status;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                dynamic createdUser = await _emailRuleConfgRepo.EmailRuleConfg(user);
                dynamic data12 = ((Tokens.Result)((Microsoft.AspNetCore.Mvc.ObjectResult)createdUser).Value).Data;
                DataTable data = new DataTable();
                if (data12 is List<object> dataList)
                {
                    if (dataList.Count > 0)
                    {
                        // Assuming the objects in the list have the same structure, use the first object to create columns
                        var firstItem = dataList[0] as IDictionary<string, object>;
                        if (firstItem != null)
                        {
                            foreach (var kvp in firstItem)
                            {
                                data.Columns.Add(kvp.Key);
                            }
                        }
                        // Populate the DataTable with data from the list
                        foreach (var item in dataList)
                        {
                            var values = item as IDictionary<string, object>;
                            if (values != null)
                            {
                                var row = data.NewRow();
                                foreach (var kvp in values)
                                {
                                    row[kvp.Key] = kvp.Value;
                                }
                                data.Rows.Add(row);
                            }
                        }
                    }
                }
                //string htmlContent = "<div style='margin-top: 5rem;padding-left: 3rem;padding-right: 3rem; margin-bottom: 5rem;border: double;'>";
                //htmlContent += "    <div style='text-align: center; line-height: 1; margin-bottom: 2rem;'>";
                //htmlContent += "        <h6 style='font-weight: bold;'>Country Master</h6>";
                //htmlContent += "        <span style='font-weight: normal;'>Country Master</span>";
                //htmlContent += "    </div>";

                //htmlContent += " <table style='width:100%'><tbody style='display: flex; flex-wrap: wrap;'>";

                //foreach (DataRow row in data.Rows)
                //{
                //    string countryCode = row["co_country_code"].ToString();
                //    string countryName = row["co_country_name"].ToString();
                //    string currencyName = row["co_currency_name"].ToString();
                //    string timezone = row["co_timezone"].ToString();
                //    string isActive = row["co_isactive"].ToString();

                //    htmlContent += "<tr style='width:100%'>";
                //    htmlContent += "    <td style='width: 85%;text-align:center'>";
                //    htmlContent += "        <div>";
                //    htmlContent += "            <span style='font-weight: bold;'>Code:</span> ";
                //    htmlContent += "            <small style='margin-left: 0.75rem;'>" + countryCode + "</small>";
                //    htmlContent += "        </div>";
                //    htmlContent += "        <div>";
                //    htmlContent += "            <span style='font-weight: bold;'>Name:</span>";
                //    htmlContent += "            <small style='margin-left: 0.75rem;'>" + countryName + "</small>";
                //    htmlContent += "        </div>";
                //    htmlContent += "        <div>";
                //    htmlContent += "            <span style='font-weight: bold;'>Currency:</span> ";
                //    htmlContent += "            <small style='margin-left: 0.75rem;'>" + currencyName + "</small>";
                //    htmlContent += "        </div>";
                //    htmlContent += "        <div>";
                //    htmlContent += "            <span style='font-weight: bold;'>Timezone:</span> ";
                //    htmlContent += "            <small style='margin-left: 0.75rem;'>" + timezone + "</small>";
                //    htmlContent += "        </div>";
                //    htmlContent += "        <div>";
                //    htmlContent += "            <span style='font-weight: bold;'>Is Active:</span> ";
                //    htmlContent += "            <small style='margin-left: 0.75rem;'>" + isActive + "</small>";
                //    htmlContent += "        </div>";
                //    htmlContent += "    </td>";
                //    htmlContent += "</tr>";
                //}

                //htmlContent += "</tbody></table></div>";

                //string htmlContent = "<div style='margin-top: 5rem; padding-left: 3rem; padding-right: 3rem; margin-bottom: 5rem; border: double;'>";
                //htmlContent += "    <div style='text-align: center; line-height: 1; margin-bottom: 2rem;'>";
                //htmlContent += "        <h3 style='font-weight: bold;'>Country Master</h3>";
                //htmlContent += "    </div>";
                //htmlContent += "    <table style='width:100%; border-collapse: collapse; margin-top: 10px'>";
                //htmlContent += "        <thead>";
                //htmlContent += "            <tr>";
                //htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Parameter</th>";
                //htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Condition</th>";
                //htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Category</th>";
                //htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Value</th>";
                //htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Status</th>";
                //htmlContent += "            </tr>";
                //htmlContent += "        </thead>";
                //htmlContent += "        <tbody>";
                //foreach (DataRow row in data.Rows)
                //{
                //    string? E_parameterName = row["E_parameterName"].ToString();
                //    string? E_conditionName = row["E_conditionName"].ToString();
                //    string? E_categoryName = row["E_categoryName"].ToString();
                //    string? E_value = row["E_value"].ToString();
                //    string? isActive = row["E_isactive"].ToString();
                //    htmlContent += "<tr style='border: 1px solid black;'>";
                //    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + E_parameterName + "</td>";
                //    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + E_conditionName + "</td>";
                //    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + E_categoryName + "</td>";
                //    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + E_value + "</td>";
                //    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + isActive + "</td>";
                //    htmlContent += "</tr>";
                //}
                //htmlContent += "        </tbody>";
                //htmlContent += "    </table>";
                //htmlContent += "</div>";
                //string date = DateTime.Now.ToString("dd-MM-yyyy--HH-mm");
                //return Ok(htmlContent);

                string htmlContent = "<div style='margin-top: 5rem; padding-left: 3rem; padding-right: 3rem; margin-bottom: 5rem; border: double;'>";
                htmlContent += "    <div style='text-align: center; line-height: 1; margin-bottom: 2rem;'>";
                htmlContent += "        <h3 style='font-weight: bold;'>Country Master</h3>";
                htmlContent += "    </div>";
                htmlContent += "    <table style='width:100%; border-collapse: collapse; margin-top: 10px;'>";
                htmlContent += "        <thead>";
                htmlContent += "            <tr>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Parameter</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Condition</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Category</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Value</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Status</th>";
                htmlContent += "            </tr>";
                htmlContent += "        </thead>";
                htmlContent += "        <tbody>";

                // Loop through the data and add rows
                foreach (DataRow row in data.Rows)
                {
                    // Retrieve the data and ensure we don't insert null values into the table
                    string? E_parameterName = row["E_parameterName"]?.ToString() ?? string.Empty;
                    string? E_conditionName = row["E_conditionName"]?.ToString() ?? string.Empty;
                    string? E_categoryName = row["E_categoryName"]?.ToString() ?? string.Empty;
                    string? E_value = HttpUtility.HtmlEncode(row["E_value"]?.ToString() ?? string.Empty);
                    string? isActive = row["E_isactive"]?.ToString() ?? string.Empty;

                    // Ensure that we only open a <tr> tag if we are within the table
                    htmlContent += "<tr style='border: 1px solid black;'>";
                    htmlContent += $"    <td style='border: 1px solid black; padding: 8px;'>{E_parameterName}</td>";
                    htmlContent += $"    <td style='border: 1px solid black; padding: 8px;'>{E_conditionName}</td>";
                    htmlContent += $"    <td style='border: 1px solid black; padding: 8px;'>{E_categoryName}</td>";
                    htmlContent += $"    <td style='border: 1px solid black; padding: 8px;'>{E_value}</td>";
                    htmlContent += $"    <td style='border: 1px solid black; padding: 8px;'>{isActive}</td>";
                    htmlContent += "</tr>";
                }

                htmlContent += "        </tbody>";
                htmlContent += "    </table>";
                htmlContent += "</div>";

                // Add date to filename or elsewhere if needed
                string date = DateTime.Now.ToString("dd-MM-yyyy--HH-mm");
                return Ok(htmlContent);


            }
            catch (Exception ex)
            {
                // Handle the exception gracefully (e.g., log it and return an error response)
                return StatusCode(500, new
                {
                    error = ex.Message
                });
            }
        }

        [HttpGet("download")]
        public IActionResult DownloadExcel()
        {
            // Define the column names for the Excel file
            string[] columnNames = {
        "Parameter", "Condition", "Category", "Value", "isActive"

    };

            // Set EPPlus LicenseContext to NonCommercial (or Commercial if applicable)
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Create a new Excel package
            using (var package = new ExcelPackage())
            {
                // Add a new worksheet
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Add the column names to the first row
                for (int col = 0; col < columnNames.Length; col++)
                {
                    worksheet.Cells[1, col + 1].Value = columnNames[col];
                }

                // Save the package to a memory stream
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                // Define the file name
                string excelName = $"Template-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                // Return the file as a downloadable response
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> upload(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            EmailRuleConfg user = new EmailRuleConfg { BaseModel = new BaseModel { OperationType = "InsertData" } };
           
           
            if (file == null || file.Length == 0)
            {
                return Ok(new Outcome { OutcomeId = 0, OutcomeDetail = "No data in the excel!" });
            }

            string[] allowedFileExtensions = { ".xls", ".xlsx", ".xlsm", ".csv" };
            if (!allowedFileExtensions.Contains(Path.GetExtension(file.FileName)))
            {
                ModelState.AddModelError("File", "Please upload a file of type: " + string.Join(", ", allowedFileExtensions));
                return BadRequest(ModelState);
            }

            DataTable dataTable2 = new DataTable();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;

                MemoryStream convertedStream = new MemoryStream();
                if (Path.GetExtension(file.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                {
                    FileConverter.ConvertCsvToXlsx(stream, convertedStream);
                }
                else if (Path.GetExtension(file.FileName).Equals(".xls", StringComparison.OrdinalIgnoreCase))
                {
                    FileConverter.ConvertXlsToXlsx(stream, convertedStream);
                }

                MemoryStream newStream = convertedStream.Length > 0 ? convertedStream : stream;
                newStream.Position = 0;

                using (var package = new ExcelPackage(newStream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    if (rowCount == 1)
                    {
                        return Ok(new Outcome { OutcomeId = 0, OutcomeDetail = "No data in the excel!" });
                    }

                    // Adding columns to DataTable based on Excel header row (first row)
                    for (int col = 1; col <= colCount; col++)
                    {
                        string columnName = worksheet.Cells[1, col].Value?.ToString();
                        if (!string.IsNullOrEmpty(columnName))
                        {
                            dataTable2.Columns.Add(new DataColumn(columnName, typeof(string)));
                        }
                    }

                    // Adding rows to DataTable from Excel data
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var dataRow = dataTable2.NewRow();
                        for (int col = 1; col <= colCount; col++)
                        {
                            var cellValue = worksheet.Cells[row, col].Value?.ToString();
                            if (DateTime.TryParse(cellValue, out DateTime parsedDate))
                            {
                                dataRow[col - 1] = parsedDate.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                dataRow[col - 1] = cellValue;
                            }
                        }
                        dataTable2.Rows.Add(dataRow);
                    }
                }
            }

            user.DataTable2 = dataTable2;
            var parameter = await _emailRuleConfgRepo.EmailRuleConfg(user);
            return parameter;
        }


        public static class FileConverter
        {
            public static void ConvertCsvToXlsx(Stream inputStream, Stream outputStream)
            {
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                    using (var reader = new StreamReader(inputStream, Encoding.UTF8))
                    {
                        int row = 1;
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var values = line.Split(',');

                            for (int col = 0; col < values.Length; col++)
                            {
                                worksheet.Cells[row, col + 1].Value = values[col];
                            }

                            row++;
                        }
                    }

                    package.SaveAs(outputStream);
                }
            }

            public static void ConvertXlsToXlsx(Stream inputStream, Stream outputStream)
            {
                using (var spreadsheetDocument = SpreadsheetDocument.Open(inputStream, false))
                {
                    var workbookPart = spreadsheetDocument.WorkbookPart;
                    using (var memoryStream = new MemoryStream())
                    {
                        var newSpreadsheetDocument = SpreadsheetDocument.Create(memoryStream, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook);
                        var newWorkbookPart = newSpreadsheetDocument.AddWorkbookPart();
                        newWorkbookPart.Workbook = new Workbook();
                        newWorkbookPart.Workbook.Sheets = new Sheets();

                        uint sheetId = 1;
                        foreach (var sheet in workbookPart.Workbook.Sheets.OfType<Sheet>())
                        {
                            var oldSheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
                            var newSheetPart = newWorkbookPart.AddNewPart<WorksheetPart>();

                            newSheetPart.FeedData(oldSheetPart.GetStream());
                            var newSheet = new Sheet { Id = newWorkbookPart.GetIdOfPart(newSheetPart), SheetId = sheetId++, Name = sheet.Name };
                            newWorkbookPart.Workbook.Sheets.Append(newSheet);
                        }

                        newWorkbookPart.Workbook.Save();
                        newSpreadsheetDocument.Clone();

                        memoryStream.Position = 0;
                        memoryStream.CopyTo(outputStream);
                    }
                }
            }
        }

    }
}
