using Common;
using Master.API.Entity;
using Master.API.Repository.Interface;
using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection.Metadata;
using System.Text;
using Tokens;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeEmailMgmtController : ControllerBase
    {
        private readonly IEmployeeEmailMgmtRepository _emplorepo;

        public EmployeeEmailMgmtController(IEmployeeEmailMgmtRepository emplorepo)
        {
            _emplorepo = emplorepo;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid user_id, string status)
        {
            try
            {
                EmployeeEmailMgmtModel user = new EmployeeEmailMgmtModel();
                user.UserId = user_id;
                user.e_isactive = status;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                var createduser = await _emplorepo.EmployeeEmailMgmt(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetEmail")]
        public async Task<IActionResult> GetEmail(Guid user_id)
        {
            try
            {
                EmployeeEmailMgmtModel user = new EmployeeEmailMgmtModel();
                user.UserId = user_id;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetEmail";
                var createduser = await _emplorepo.EmployeeEmailMgmt(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] EmployeeEmailMgmtModel user)
        {
            try
            {
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                if (user.e_id == null)
                {
                    user.BaseModel.OperationType = "Insert";
                }
                else
                {
                    user.e_updateddate = DateTime.Now;
                    user.BaseModel.OperationType = "Update";
                }
                var createduser = await _emplorepo.Get(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] EmployeeEmailMgmtModel user)
        {
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.BaseModel.OperationType = "Delete";
            var employeemailDetails = await _emplorepo.Get(user);
            return employeemailDetails;
        }

        [HttpGet("GetExcel")]
        public async Task<IActionResult> GetExcel(Guid UserId, string status)
        {
            try
            {
                EmployeeEmailMgmtModel user = new EmployeeEmailMgmtModel();
                user.UserId = UserId;
                user.e_isactive = status;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                dynamic createduser = await _emplorepo.EmployeeEmailMgmt(user);
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
                EmployeeEmailMgmtModel user = new EmployeeEmailMgmtModel();
                user.UserId = UserId;
                user.e_isactive = status;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                dynamic createdUser = await _emplorepo.EmployeeEmailMgmt(user);
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
                htmlContent += "        <h3 style='font-weight: bold;'>Employee Email Management</h3>";
                htmlContent += "    </div>";
                htmlContent += "    <table style='width:100%; border-collapse: collapse; margin-top: 10px'>";
                htmlContent += "        <thead>";
                htmlContent += "            <tr>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Sr.No</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Email</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Employees</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Is Active</th>";
                htmlContent += "            </tr>";
                htmlContent += "        </thead>";
                htmlContent += "        <tbody>";
                int a = 0;
                foreach (DataRow row in data.Rows)
                {
                    a++;
                    htmlContent += "<tr style='border: 1px solid black;'>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + a + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + row["e_email"].ToString() + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + row["e_staffname"].ToString() + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + row["e_isactive"].ToString() + "</td>";
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
