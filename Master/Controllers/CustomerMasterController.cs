using Master.API.Entity;

using Master.API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Tokens;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using Common;
using System.Text;
using System.Reflection.Metadata;
using System;
using System.ComponentModel.Design;
using Master.Repository.Interface;
using Master.Entity;

namespace Master.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CustomerMasterController : Controller
    {
        public readonly ICustomerMasterRepository _customerRepo;

        public CustomerMasterController(ICustomerMasterRepository customerRepo)
        {
            _customerRepo = customerRepo;
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetCustomer(Guid UserId, string Server_Value, string status,string CompanyId)
        {
            try
            {
                CustomerMaster user = new CustomerMaster();
                user.UserId = UserId;
                user.c_isactive = status;
                user.c_com_id = CompanyId;

                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.Server_Value = Server_Value;
                user.BaseModel.OperationType = "GetAll";

                var createduser = await _customerRepo.Customer(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[HttpGet("GetCustomerById")]
        //public async Task<IActionResult> GetCustomerById(Guid? UserId, string c_id, string? Server_Value, string CompanyId)
        //{

        //    CustomerMaster user = new CustomerMaster();
        //    if (user.BaseModel == null)
        //    {
        //        user.BaseModel = new BaseModel();
        //    }
        //    user.UserId = UserId;
        //    user.BaseModel.Server_Value = Server_Value;
        //    user.c_id = c_id;
        //    user.BaseModel.OperationType = "GetCustomerById";
        //    user.c_com_id = CompanyId;
        //    try
        //    {
        //        var parameter = await _customerRepo.Get(user);
        //        return parameter;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> InsertCustomer([FromBody] CustomerMaster user)
        //{
        //    try
        //    {


        //        if (user.BaseModel == null)
        //        {
        //            user.BaseModel = new BaseModel();
        //        }

        //        if (user.c_id == null)
        //        {
        //            user.BaseModel.OperationType = "InsertCustomer";
        //        }
        //        else if (user.c_id != null && user.CustomerImage != null)
        //        {
        //            user.BaseModel.OperationType = "UpdatePhoto";
        //        }
        //        else if (user.c_name == null)
        //        {
        //            user.BaseModel.OperationType = "UpdateStatus";
        //        }

        //        else if (user.c_id != null && user.c_name != null)
        //        {
        //            user.BaseModel.OperationType = "UpdateCustomer";
        //        }




        //        user.BaseModel.Server_Value = user.Server_Value;




        //        var createduser = await _customerRepo.Customer(user);
        //        return createduser;
        //    }
        //    catch (Exception)
        //    {


        //        throw;
        //    }
        //}


        [HttpPost("Insert")]
        public async Task<IActionResult> InsertCustomer([FromBody] CustomerMaster user)
        {
            try
            {
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.Server_Value = user.Server_Value;

                //user.BaseModel.OperationType = "Insert";

                user.BaseModel.Server_Value = user.Server_Value;
                if (user.c_id == null)
                {
                    user.BaseModel.OperationType = "InsertCustomer";
                }
                //else if (user.c_name == null && user.c_id != null)
                //{
                //    user.BaseModel.OperationType = "UpdateStatus";
                //}
                else
                {
                    user.BaseModel.OperationType = "Update";
                }




                DataTable dataTable = new DataTable();

                dataTable.Columns.Add("c_type_id", typeof(string));
                dataTable.Columns.Add("name", typeof(string));
                dataTable.Columns.Add("c_type", typeof(string));

                dataTable.Columns.Add("attachment", typeof(string));
                dataTable.Columns.Add("mdate", typeof(string));

                dataTable.Columns.Add("idate", typeof(string));
                dataTable.Columns.Add("vdate", typeof(string));

                if (user.CustomerAttachment != null)
                {

                    foreach (var privilage in user.CustomerAttachment)
                    {
                        dataTable.Rows.Add(
                            privilage.c_type_id,

                            privilage.name,
                            privilage.c_type,
                            privilage.attachment,
                            privilage.mdate,
                            privilage.idate,
                            privilage.vdate



                        );
                    }
                }
                // user.OrderDetails = null;
                user.DataTable = dataTable;
                dynamic createduser = await _customerRepo.Customer(user);
                var outcomeidvalue = createduser.Value.Outcome.OutcomeId;
               
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpGet("Get")]
        public async Task<IActionResult> Get(Guid? UserId, string Server_Value, string? c_id)
        {
            try
            {
                CustomerMaster user = new CustomerMaster();
                user.UserId = UserId;
                user.c_id = c_id;
               
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.Server_Value = Server_Value;
                user.BaseModel.OperationType = "Get";

                var createduser = await _customerRepo.Get(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetDetails")]
        public async Task<IActionResult> GetDetails(Guid? UserId, string Server_Value, string? c_id)
        {
            try
            {
                CustomerMaster user = new CustomerMaster();
                user.UserId = UserId;
                user.c_id = c_id;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.Server_Value = Server_Value;
                user.BaseModel.OperationType = "GetDetails";

                var createduser = await _customerRepo.Customer(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteCustomer([FromBody] CustomerMaster user)
        {

            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.BaseModel.Server_Value = user.Server_Value;
            user.BaseModel.OperationType = "DeleteCustomer";
            //user.UserId = Id;

            var productDetails = await _customerRepo.Customer(user);
            return productDetails;
        }


        [HttpGet("GetExcel")]

        public async Task<IActionResult> GetExcel(Guid UserId, string? Server_Value, string status, string CompanyId)
        {
            try
            {
                CustomerMaster user = new CustomerMaster();
                user.UserId = UserId;
                user.c_isactive = status;
                user.c_com_id = CompanyId;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.Server_Value = Server_Value;
                user.BaseModel.OperationType = "GetAll";

                // Fetch data from the repository
                dynamic createduser = await _customerRepo.Customer(user);
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
                    ExportRepository exportRepository = new ExportRepository();
                    // Generate the Excel file
                    byte[] excelfile = exportRepository.ExportToExcelAsync(data);
                    // Generate the Excel file 

                    // Convert the Excel file to Base64
                    if (excelfile != null)
                    {
                        base64Pdf = Convert.ToBase64String(excelfile);
                    }
                }

                // Return the Base64 string as the response
                var result = new Result
                {

                    Data = DataTableToJsonObj(data)
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle the exception gracefully (e.g., log it and return an error response)
                return StatusCode(500, new { error = ex.Message });
            }
        }


        public string DataTableToJsonObj(DataTable dt)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt);
            StringBuilder JsonString = new StringBuilder();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        if (j < ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\",");
                        }
                        else if (j == ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == ds.Tables[0].Rows.Count - 1)
                    {
                        JsonString.Append("}");
                    }
                    else
                    {
                        JsonString.Append("},");
                    }
                }
                JsonString.Append("]");
                return JsonString.ToString();
            }
            else
            {
                return null;
            }
        }

        [HttpGet("GetPdf")]
        public async Task<IActionResult> GetPdf(Guid? UserId, string? Server_Value, string status, string CompanyId)
        {

            var document = new Document();

            CustomerMaster user = new CustomerMaster();
            user.UserId = UserId;
            user.c_isactive = status;
            user.c_com_id = CompanyId;

            string base64Pdf = null;
            // Fetch data from the repository
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.BaseModel.Server_Value = Server_Value;
            user.BaseModel.OperationType = "GetAll";

            dynamic createdUser = await _customerRepo.Customer(user);
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
                ExportRepository exportRepository = new ExportRepository();
                // Generate the Excel file
                byte[] excelfile = exportRepository.ExportToPdfAsync(data);

                // Convert the Excel file to Base64
                if (excelfile != null)
                {
                    base64Pdf = Convert.ToBase64String(excelfile);
                }
            }



            //string htmlContent = "<div style='margin-top: 5rem; padding-left: 3rem; padding-right: 3rem; margin-bottom: 5rem; border: double;'>";
            //htmlContent += "    <div style='text-align: center; line-height: 1; margin-bottom: 2rem;'>";
            //htmlContent += "        <h3 style='font-weight: bold;'>Customer Details</h3>";
            //htmlContent += "    </div>";
            //htmlContent += "    <table style='width:100%; border-collapse: collapse; margin-top: 10px'>";
            //htmlContent += "        <thead>";
            //htmlContent += "            <tr>";
            //htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Name</th>";
            //htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Address</th>";
            //htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Contact</th>";

            //htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>DOB</th>";

            //htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Email</th>";
            //htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>GST No.</th>";

            //htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Openinng Balance</th>";
            //htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Status</th>";
            //htmlContent += "            </tr>";
            //htmlContent += "        </thead>";
            //htmlContent += "        <tbody>";


            string htmlContent = "<div style='margin-top: 5rem; padding-left: 3rem; padding-right: 3rem; margin-bottom: 5rem; '>";
            htmlContent += "    <div style='text-align: center; line-height: 1; margin-bottom: 2rem;'>";
            htmlContent += "        <h3 style='font-weight: bold;'>Customer Details</h3>";
            htmlContent += "    </div>";
            htmlContent += "    <div class='table-responsive'>";
            htmlContent += "    <div class='table-container' style='max-width: 100%; overflow-x: auto;'>";
           htmlContent += "        <table class='responsive-table' style='width: 100%;'>";
            htmlContent += "            <thead>";
            htmlContent += "            <tr>";
            htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;width: 30%;'>Name</th>";
            htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;width: 20%;'>Code</th>";
            htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;width: 20%;'>Address</th>";
            htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;width: 20%;'>Contact</th>";
            //htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Alternate Contact</th>";
            htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;width: 10%;'>DOB</th>";

            htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;width: 20%;'>Email</th>";

            htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;width: 10%;'>Status</th>";
            htmlContent += "            </tr>";
            htmlContent += "        </thead>";
            htmlContent += "        <tbody style='max-height: 300px; overflow-y: auto; display: block;'>";






            foreach (DataRow row in data.Rows)
            {
                string name = row["c_name"].ToString();
                string c_ccode = row["c_ccode"].ToString();
                string address = row["c_address"].ToString();
                string contact = row["c_contact"].ToString();
                string alternatecontact = row["c_contact2"].ToString();
                string DOB = row["c_dob"].ToString();
                string aniversarydate = row["c_anidate"].ToString();
                string email = row["c_email"].ToString();
               
                string activestatus = row["c_isactive"].ToString();




                htmlContent += "<tr style='border: 0px solid black;'>";
                htmlContent += "    <td style='border: 1px solid black; padding: 8px; white-space: normal;'>" + name + "</td>";
                htmlContent += "    <td style='border: 1px solid black; padding: 8px; white-space: normal;'>" + c_ccode + "</td>";
                htmlContent += "    <td style='border: 1px solid black; padding: 8px;white-space: normal;'>" + address + "</td>";
                htmlContent += "    <td style='border: 1px solid black; padding: 8px;white-space: normal;'>" + contact + "</td>";
                //htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + alternatecontact + "</td>";
                htmlContent += "    <td style='border: 1px solid black; padding: 8px;white-space: normal;'>" + DOB + "</td>";
             
                htmlContent += "    <td style='border: 1px solid black; padding: 8px;white-space: normal;'>" + email + "</td>";
              
                htmlContent += "    <td style='border: 1px solid black; padding: 8px;white-space: normal;'>" + activestatus + "</td>";
                htmlContent += "</tr>";
            }


            htmlContent += "        </tbody>";

            htmlContent += "    </table>";
            htmlContent += "</div>";
            htmlContent += "</div>";
            htmlContent += "</div>";

            string date = DateTime.Now.ToString("dd-MM-yyyy--HH-mm");

            return Ok(htmlContent);

        }
    }
}
