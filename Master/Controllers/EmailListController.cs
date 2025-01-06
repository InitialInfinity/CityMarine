using Common;
using Master.API.Entity;
using Master.API.Repository.Interface;
using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Reflection.Metadata;
using System.Text;
using Tokens;

namespace Master.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailListController : Controller
    {
        private readonly IEmailListRepository _EmailListRepo;

        public EmailListController(IEmailListRepository EmailListRepo)
        {
            _EmailListRepo = EmailListRepo;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid user_id)
        {
            try
            {
                EmailList user = new EmailList();
                user.UserId = user_id;
               
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                var createduser = await _EmailListRepo.EmailList(user);
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
            EmailList user = new EmailList();
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.UserId = user_id;
            user.E_id = E_id;
            user.BaseModel.OperationType = "Get";
            try
            {
                var parameter = await _EmailListRepo.Get(user);
                return parameter;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] EmailList user)
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
                var createduser = await _EmailListRepo.Get(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteDepartment([FromBody] EmailList user)
        {
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.BaseModel.OperationType = "Delete";
            var productDetails = await _EmailListRepo.Get(user);
            return productDetails;
        }

        [HttpGet("GetExcel")]
        public async Task<IActionResult> GetExcel(Guid UserId)
        {
            try
            {
                EmailList user = new EmailList();
                user.UserId = UserId;
               
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                dynamic createduser = await _EmailListRepo.EmailList(user);
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
        public async Task<IActionResult> GetPdf(Guid? UserId)
        {
            try
            {
                EmailList user = new EmailList();
                user.UserId = UserId;
              
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                dynamic createdUser = await _EmailListRepo.EmailList(user);
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
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Email Id</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Password</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>SMTP Host</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>SMTP Port</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>IMAP Host</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>IMAP Port</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>POP3 Host</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>POP3 Port</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Oauth Key</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>SSL Enable</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Status</th>";
                htmlContent += "            </tr>";
                htmlContent += "        </thead>";
                htmlContent += "        <tbody>";
               
                foreach (DataRow row in data.Rows)
                {
                    string? E_email = row["E_email"].ToString();
                    string? E_password = row["E_password"].ToString();
                    string? E_smtph = row["E_smtph"].ToString();
                    string? E_smtpp = row["E_smtpp"].ToString();
                    string? E_imaph = row["E_imaph"].ToString();
                    string? E_imapp = row["E_imapp"].ToString();
                    string? E_poph = row["E_poph"].ToString();
                    string? E_popp = row["E_popp"].ToString();
                    string? E_key = row["E_key"].ToString();
                    string? E_issslEnable = row["E_issslEnable"].ToString();
                    string? isActive = row["E_isactive"].ToString();
                    htmlContent += "<tr style='border: 1px solid black;'>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + E_email + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + E_password + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + E_smtph + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + E_smtpp + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + E_imaph + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + E_imapp + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + E_poph + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + E_popp + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + E_key + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + E_issslEnable + "</td>";
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
