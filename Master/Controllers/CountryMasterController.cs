using Master.API.Repository.Interface;
using Master.API.Entity;
using Microsoft.AspNetCore.Mvc;
using Tokens;
using Common;
using System.Data;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryMasterController : ControllerBase
    {
        private readonly ICountryMasterRepository _countryRepo;
        public CountryMasterController(ICountryMasterRepository countryRepo)
        {
            _countryRepo = countryRepo;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid? UserId, string status)
        {
            try
            {
                CountryMaster user = new CountryMaster();
                user.UserId = UserId;
                user.co_isactive = status;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                var createduser = await _countryRepo.Country(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get(Guid UserId, Guid? co_id)
        {
            try
            {
                CountryMaster user = new CountryMaster();
                user.UserId = UserId;
                user.co_id = co_id;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "Get";
                var createduser = await _countryRepo.Get(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] CountryMaster user)
        {
            try
            {
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                if (user.co_id == null)
                {
                    user.BaseModel.OperationType = "Insert";
                }
                else
                {
                    user.co_updateddate = DateTime.Now;
                    user.BaseModel.OperationType = "Update";
                }
                var createduser = await _countryRepo.Country(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] CountryMaster user)
        {
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.BaseModel.OperationType = "Delete";
            var productDetails = await _countryRepo.Country(user);
            return productDetails;
        }

        [HttpGet("GetExcel")]
        public async Task<IActionResult> GetExcel(Guid UserId, string status)
        {
            try
            {
                CountryMaster user = new CountryMaster();
                user.UserId = UserId;
                user.co_isactive = status;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                dynamic createduser = await _countryRepo.Country(user);
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
                ExportRepository ep = new ExportRepository();
                var result = new Result
                {
                    Data = ep.DataTableToJsonObj(data)
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
                CountryMaster user = new CountryMaster();
                user.UserId = UserId;
                user.co_isactive = status;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                dynamic createdUser = await _countryRepo.Country(user);
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

                string htmlContent = "<div style='margin-top: 5rem; padding-left: 3rem; padding-right: 3rem; margin-bottom: 5rem; border: double;'>";
                htmlContent += "    <div style='text-align: center; line-height: 1; margin-bottom: 2rem;'>";
                htmlContent += "        <h3 style='font-weight: bold;'>Country Master</h3>";
                htmlContent += "    </div>";
                htmlContent += "    <table style='width:100%; border-collapse: collapse; margin-top: 10px'>";
                htmlContent += "        <thead>";
                htmlContent += "            <tr>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Country Code</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Country Name</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Currency Name</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Country Timezone</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Status</th>";
                htmlContent += "            </tr>";
                htmlContent += "        </thead>";
                htmlContent += "        <tbody>";
                foreach (DataRow row in data.Rows)
                {
                    string? countryCode = row["co_country_code"].ToString();
                    string? countryName = row["co_country_name"].ToString();
                    string? currencyName = row["co_currency_name"].ToString();
                    string? timezone = row["co_timezone"].ToString();
                    string? isActive = row["co_isactive"].ToString();
                    htmlContent += "<tr style='border: 1px solid black;'>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + countryCode + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + countryName + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + currencyName + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + timezone + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + isActive + "</td>";
                    htmlContent += "</tr>";
                }
                htmlContent += "        </tbody>";
                htmlContent += "    </table>";
                htmlContent += "</div>";
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
    }
}
