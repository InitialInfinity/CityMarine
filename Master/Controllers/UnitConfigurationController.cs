using Master.Repository.Interface;
using Master.Entity;
using Microsoft.AspNetCore.Mvc;
using Tokens;
using Common;
using System.Data;
using System.Reflection.Metadata;
using System.Text;
using System.Drawing;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitConfigurationController : ControllerBase
    {
        private readonly IUnitConfigurationRepository _unitRepo;
        public UnitConfigurationController(IUnitConfigurationRepository unitRepo)
        {
            _unitRepo = unitRepo;

        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetUnitConfiguration(Guid? UserId, string status)
        {
            try
            {
                UnitConfiguration user = new UnitConfiguration();
                user.UserId = UserId;
                user.u_isactive = status;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                var createduser = await _unitRepo.UnitConfiguration(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetUnitConfiguration(Guid UserId, Guid? u_id)
        {
            try
            {
                UnitConfiguration user = new UnitConfiguration();
                user.UserId = UserId;
                user.u_id = u_id;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "Get";
                var createduser = await _unitRepo.Get(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertUnitConfiguration([FromBody] UnitConfiguration user)
        {
            try
            {
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                if (user.u_id == null)
                {
                    user.BaseModel.OperationType = "Insert";
                }
                else
                {
                    user.BaseModel.OperationType = "Update";
                }
                var createduser = await _unitRepo.UnitConfiguration(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteUnitConfiguration([FromBody] UnitConfiguration user)
        {

            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.BaseModel.OperationType = "Delete";
            var productDetails = await _unitRepo.UnitConfiguration(user);
            return productDetails;
        }
        [HttpGet("GetExcel")]
        public async Task<IActionResult> GetExcel(Guid UserId, string status)
        {
            try
            {
                UnitConfiguration user = new UnitConfiguration();
                user.UserId = UserId;
                user.u_isactive = status;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                // Fetch data from the repository
                dynamic createduser = await _unitRepo.UnitConfiguration(user);
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
        public async Task<IActionResult> GetPdf(Guid? UserId, string status)
        {
            try
            {
                UnitConfiguration user = new UnitConfiguration();
                user.UserId = UserId;
                user.u_isactive = status;
                // Fetch data from the repository
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                dynamic createdUser = await _unitRepo.UnitConfiguration(user);
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
                htmlContent += "        <h3 style='font-weight: bold;'>Unit Configuration</h3>";
                htmlContent += "    </div>";
                htmlContent += "    <table style='width:100%; border-collapse: collapse; margin-top: 10px'>";
                htmlContent += "        <thead>";
                htmlContent += "            <tr>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Unit Name</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Height</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Amount</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Size</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Width</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Status</th>";
                htmlContent += "            </tr>";
                htmlContent += "        </thead>";
                htmlContent += "        <tbody>";
                foreach (DataRow row in data.Rows)
                {
                    string? u_unit = row["u_unit"].ToString();
                    string? u_height = row["u_height"].ToString();
                    string? u_amount = row["u_amount"].ToString();
                    string? u_size = row["u_size"].ToString();
                    string? u_width = row["u_width"].ToString();
                    string? u_isactive = row["u_isactive"].ToString();
                    htmlContent += "<tr style='border: 1px solid black;'>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + u_unit + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + u_height + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + u_amount + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + u_size + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + u_width + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + u_isactive + "</td>";
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
