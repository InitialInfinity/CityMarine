using Common;
using Master.API.Entity;
using Master.API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text;
using Tokens;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateMasterController : ControllerBase
    {
        private readonly IStateMasterRepository _countryRepo;
        public StateMasterController(IStateMasterRepository countryRepo)
        {
            _countryRepo = countryRepo;
        }
        [HttpGet("GetState")]
        public async Task<IActionResult> GetState(Guid UserId, string? CountryId)
        {
            try
            {
                StateMaster user = new StateMaster();
                user.UserId = UserId;
                user.s_country_id = CountryId;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                var createduser = await _countryRepo.State(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetStateById")]
        public async Task<IActionResult> GetStateById(Guid UserId, Guid? s_id)
        {
            try
            {
                StateMaster user = new StateMaster();
                user.UserId = UserId;
                user.s_id = s_id;
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
        public async Task<IActionResult> InsertState([FromBody] StateMaster user)
        {
            try
            {
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                if (user.s_id == null)
                {
                    user.BaseModel.OperationType = "Insert";
                }
                else
                {
                    user.BaseModel.OperationType = "Update";
                }
                var createduser = await _countryRepo.State(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("DeleteState")]
        public async Task<IActionResult> Delete([FromBody] StateMaster user)
        {
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.BaseModel.OperationType = "Delete";
            var productDetails = await _countryRepo.State(user);
            return productDetails;
        }

        [HttpGet("GetStateMasterByCoId")]
        public async Task<IActionResult> GetStateMasterByCoId(Guid UserId, string CountryId, string status)
        {
            try
            {
                StateMaster user = new StateMaster();
                user.UserId = UserId;
                user.s_country_id = CountryId;
                user.s_isactive = status;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetStateMasterByCoId";
                var createduser = await _countryRepo.State(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetExcel")]

        public async Task<IActionResult> GetExcel(Guid UserId,string co_id, string status)
        {
            try
            {
                StateMaster user = new StateMaster();
                user.UserId = UserId;
                user.s_country_id= co_id;
                user.s_isactive= status;

                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetStateMasterByCoId";
                dynamic createduser = await _countryRepo.State(user);
                dynamic data1 = ((Tokens.Result)((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value).Data;
                DataTable data = new DataTable();
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
        public async Task<IActionResult> GetPdf(Guid? UserId, string co_id, string status)
        {
            try
            {
                StateMaster user = new StateMaster();
                user.UserId = UserId;
                user.s_country_id = co_id;
                user.s_isactive = status;
                // Fetch data from the repository
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetStateMasterByCoId";
                dynamic createdUser = await _countryRepo.State(user);
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
                string htmlContent = "<div style='margin-top: 5rem; padding-left: 3rem; padding-right: 3rem; margin-bottom: 5rem; border: double;'>";
                htmlContent += "    <div style='text-align: center; line-height: 1; margin-bottom: 2rem;'>";
                htmlContent += "        <h3 style='font-weight: bold;'>State Master</h3>";
                htmlContent += "    </div>";
                htmlContent += "    <table style='width:100%; border-collapse: collapse; margin-top: 10px'>";
                htmlContent += "        <thead>";
                htmlContent += "            <tr>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Country Name</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'> Code</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'> Name</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Status</th>";
                htmlContent += "            </tr>";
                htmlContent += "        </thead>";
                htmlContent += "        <tbody>";
                foreach (DataRow row in data.Rows)
                {
                    string? s_country_name = row["s_country_name"].ToString();
                    string? s_state_name = row["s_state_name"].ToString();
                    string? s_state_code = row["s_state_code"].ToString();
                    string? isActive = row["s_isactive"].ToString();
                    htmlContent += "<tr style='border: 1px solid black;'>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + s_country_name + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + s_state_code + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + s_state_name + "</td>";
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
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
