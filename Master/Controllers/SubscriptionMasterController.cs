using Common;
using Master.API.Entity;
using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tokens;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionMasterController : ControllerBase
    {
        private readonly ISubscriptionMasterRepository _subRepo;
        public SubscriptionMasterController(ISubscriptionMasterRepository subRepo)
        {
            _subRepo = subRepo;

        }
        [HttpGet("GetSubscription")]
        public async Task<IActionResult> GetSubscription(Guid? UserId, string status)
        {
            try
            {
                Subscription user = new Subscription();
                user.UserId = UserId;
                user.com_isactive = status;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }

                user.BaseModel.OperationType = "GetAll";

                var createduser = await _subRepo.Subscription(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetSubscriptionById")]
        public async Task<IActionResult> GetSubscriptionById(Guid UserId, Guid? sm_id)
        {
            try
            {
                Subscription user = new Subscription();
                user.UserId = UserId;
                user.sm_id = sm_id;

                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }

                user.BaseModel.OperationType = "Get";

                var createduser = await _subRepo.Get(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertSubscription([FromBody] Subscription user)
        {
            try
            {


                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                if (user.sm_id == null)
                {
                    user.BaseModel.OperationType = "Insert";
                }
                else
                {
                    user.BaseModel.OperationType = "Update";
                }

                var createduser = await _subRepo.Subscription(user);
                return createduser;
            }
            catch (Exception)
            {


                throw;
            }
        }

        [HttpPost("DeleteSubscription")]
        public async Task<IActionResult> DeleteSubscription([FromBody] Subscription user)
        {

            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.BaseModel.OperationType = "Delete";

            var productDetails = await _subRepo.Subscription(user);
            return productDetails;
        }

        [HttpGet("GetExcel")]

        public async Task<IActionResult> GetExcel(Guid UserId, string status)
        {
            try
            {
                Subscription user = new Subscription();
                user.UserId = UserId;
                user.com_isactive = status;


                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }

                user.BaseModel.OperationType = "GetAll";

                // Fetch data from the repository
                dynamic createduser = await _subRepo.Subscription(user);
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
        public async Task<IActionResult> GetPdf(Guid? UserId, string status)
        {
            try
            {
                Subscription user = new Subscription();
                user.UserId = UserId;
                user.com_isactive = status;
                string base64Pdf = null;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                dynamic createdUser = await _subRepo.Subscription(user);
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
                htmlContent += "        <h3 style='font-weight: bold;'>Subscription Master</h3>";
                htmlContent += "    </div>";
                htmlContent += "    <table style='width:100%; border-collapse: collapse; margin-top: 10px'>";
                htmlContent += "        <thead>";
                htmlContent += "            <tr>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Package Name</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Service</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Amount</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Discount</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Final Amount</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Duration in month</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Invoice</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Quotation</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Expence</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Cash Order</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Status</th>";
                htmlContent += "            </tr>";
                htmlContent += "        </thead>";
                htmlContent += "        <tbody>";
                foreach (DataRow row in data.Rows)
                {
                    string? package_name = row["package_name"].ToString();
                    string? sm_service = row["sm_service"].ToString();
                    string? subAmount = row["subAmount"].ToString();
                    string? subDiscount = row["subDiscount"].ToString();
                    string? final_Amount = row["final_Amount"].ToString();
                    string? sub_duration = row["sub_duration"].ToString();
                    string? Invoice = row["Invoice"].ToString();
                    string? Quotation = row["Quotation"].ToString();
                    string? Expence = row["Expence"].ToString();
                    string? cash_order = row["cash_order"].ToString();
                    string? com_isactive = row["com_isactive"].ToString();
                    htmlContent += "<tr style='border: 1px solid black;'>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + package_name + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + sm_service + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + subAmount + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + subDiscount + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + final_Amount + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + sub_duration + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + Invoice + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + Quotation + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + Expence + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + cash_order + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + com_isactive + "</td>";
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
