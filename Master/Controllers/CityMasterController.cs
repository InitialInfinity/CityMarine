using Common;
using Master.API.Entity;
using Master.API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tokens;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityMasterController : ControllerBase
    {
        private readonly ICityMasterRepository _cityRepo;
        public CityMasterController(ICityMasterRepository cityRepo)
        {
            _cityRepo = cityRepo;
        }
        [HttpGet("GetCityById")]
        public async Task<IActionResult> GetCityById(Guid UserId, Guid? ci_id, string ci_country_name, string ci_state_id, string ci_state_name)
        {
            try
            {
                CityMaster user = new CityMaster();
                user.UserId = UserId;
                user.ci_id = ci_id;
                user.ci_country_name = ci_country_name;                 
                user.ci_state_id = ci_state_id;
                user.ci_state_name = ci_state_name;              
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetCityMasterById";
                var createduser = await _cityRepo.Get(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetCity")]
        public async Task<IActionResult> GetCity(Guid UserId, string CountryId, string StateId, string status)
        {
            try
            {
                CityMaster user = new CityMaster();
                user.UserId = UserId;
                user.ci_state_id = StateId;
                user.ci_country_id = CountryId;
                user.ci_isactive = status;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }

                user.BaseModel.OperationType = "GetCitybyStateId";

                var createduser = await _cityRepo.City(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertCity([FromBody] CityMaster user)
        {
            try
            {
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                if (user.ci_id == null)
                {
                    user.BaseModel.OperationType = "InsertCityMaster";
                }
                else
                {
                    user.BaseModel.OperationType = "UpdateCityMaster";
                }
                var createduser = await _cityRepo.City(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("DeleteCity")]
        public async Task<IActionResult> DeleteCity([FromBody] CityMaster user)
        {

            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.BaseModel.OperationType = "DeleteCityMaster";
            var productDetails = await _cityRepo.City(user);
            return productDetails;
        }
        [HttpGet("GetExcel")]

        public async Task<IActionResult> GetExcel(Guid UserId, string status, string CountryId, string CountryName, string StateId, string StateName)
        {
            try
            {
                CityMaster user = new CityMaster();
                user.UserId = UserId;
                user.ci_isactive = status;
                user.ci_country_id = CountryId;
                user.ci_country_name = CountryName;
                user.ci_state_id = StateId;
                user.ci_state_name = StateName;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetCitybyStateId";

                // Fetch data from the repository
                dynamic createduser = await _cityRepo.City(user);
                dynamic data1 = ((Tokens.Result)((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value).Data;
                DataTable data = new DataTable();
                string base64Pdf = null;

                if (data1 is List<object> dataList)
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
                ExportRepository ep = new ExportRepository();
                // Return the Base64 string as the response
                var result = new Result
                {
                    Data = ep.DataTableToJsonObj(data)
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle the exception gracefully (e.g., log it and return an error response)
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("GetPdf")]
        public async Task<IActionResult> GetPdf(Guid? UserId, string status, string CountryId, string CountryName, string StateId, string StateName)
        {
            try
            {
            CityMaster user = new CityMaster();
            user.UserId = UserId;
            user.ci_isactive = status;
            user.ci_country_id = CountryId;//Vinit
            user.ci_country_name = CountryName;
            user.ci_state_id = StateId;
            user.ci_state_name = StateName;
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.BaseModel.OperationType = "GetCitybyStateId";
            dynamic createdUser = await _cityRepo.City(user);
            dynamic data12 = ((Tokens.Result)((Microsoft.AspNetCore.Mvc.ObjectResult)createdUser).Value).Data;
            DataTable data = new DataTable();
            if (data12 is List<object> dataList)
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
                string htmlContent = "<div style='margin-top: 5rem; padding-left: 3rem; padding-right: 3rem; margin-bottom: 5rem; border: double;'>";
                htmlContent += "    <div style='text-align: center; line-height: 1; margin-bottom: 2rem;'>";
                htmlContent += "        <h3 style='font-weight: bold;'>City Master</h3>";
                htmlContent += "    </div>";
                htmlContent += "    <table style='width:100%; border-collapse: collapse; margin-top: 10px'>";
                htmlContent += "        <thead>";
                htmlContent += "            <tr>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Country Name</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>State Name</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Code</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Name</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Status</th>";
                htmlContent += "            </tr>";
                htmlContent += "        </thead>";
                htmlContent += "        <tbody>";
                foreach (DataRow row in data.Rows)
                {
                    string? ci_country_name = row["ci_country_name"].ToString();
                    string? ci_state_name = row["ci_state_name"].ToString();
                    string? ci_city_code = row["ci_city_code"].ToString();
                    string? ci_city_name = row["ci_city_name"].ToString();
                    string? isActive = row["ci_isactive"].ToString();
                    htmlContent += "<tr style='border: 1px solid black;'>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + ci_country_name + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + ci_state_name + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + ci_city_code + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + ci_city_name + "</td>";
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
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
