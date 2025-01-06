using Common.Token;
using Master.API.Entity;
using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using MimeKit;
using System.Text;
using Tokens;
using System.Data;
using Common;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerAllocationController : ControllerBase
    {
        private readonly IServerAllocation _cRepo;
        private readonly IConfiguration _configuration;
        public ServerAllocationController(IServerAllocation cRepo, IConfiguration configuration)
        {
            _cRepo = cRepo;
            _configuration = configuration;
        }
        [HttpGet("GetCompany")]
        public async Task<IActionResult> GetComp(Guid? UserId)
        {
            try
            {
                CompanyDetails user = new CompanyDetails();
                user.UserId = UserId;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetSubCompany";
                var createduser = await _cRepo.GetAll(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("getServers")]
        public IActionResult GetServers()
        {
            var serverSettings = _configuration.GetSection("server").GetChildren();
            var serverOptions = serverSettings.Select(option => new ServerOption
            {
                Key = option.Value,
                Value = option.Key
            }).ToList();

            return Ok(serverOptions);
        }

        [HttpGet("GetServerValue")]
        public async Task<IActionResult> GetServerValue(Guid? UserId, Guid? com_id)
        {
            try
            {
                CompanyDetails user = new CompanyDetails();
                user.UserId = UserId;
                user.com_id = com_id;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetServerValue";
                var createduser = await _cRepo.Allocation(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertServerDetails([FromBody] CompanyDetails user)
        {
            try
            {
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "InsertServerDetails";
                 dynamic createduser = await _cRepo.Allocation(user);
                var outcomes = createduser.Value.Outcome;
                if (outcomes.OutcomeId == 1 && outcomes.OutcomeDetail== "Server allocated successfully!")
                {
                    LoginDetails login = new LoginDetails();
                    if (login.BaseModel == null)
                    {
                        login.BaseModel = new BaseModel();
                    }
                    login.BaseModel.OperationType = "InsertloginDetails";
                    const string CharSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    const int PasswordLength = 8; 
                    Random random = new Random();
                    StringBuilder passwordBuilder = new StringBuilder();
                    for (int i = 0; i < PasswordLength; i++)
                    {
                        int index = random.Next(0, CharSet.Length);
                        char randomChar = CharSet[index];
                        passwordBuilder.Append(randomChar);
                    }
                    string password = passwordBuilder.ToString();
                    login.com_password = password;
                    login.Contact_no = user.com_contact;
                    login.com_id = user.com_id.ToString();
                    login.EmailId = user.com_email;
                    login.CountryId = user.CountryId;
                    login.com_company_name = user.com_company_name;
                    login.CreatedDate = DateTime.Now;
                    login.UserId = user.UserId;
                    login.com_code = user.com_code;
                    login.BaseModel.Server_Value = user.Server_Value;
                    dynamic loginuser = await _cRepo.AllocationCompany(login);
                    var outcomes2 = loginuser.Value.Outcome;
                    var outdata= loginuser.Value.Data;
                    if (outcomes2.OutcomeId == 1)
                    {
                        login.com_password = outdata.OutcomeDetail;
                        var email = sendEmail(login);
                    }
                }
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult sendEmail(LoginDetails user)
        {
            string htmlContent = "<div style=\"font-family: Arial, sans-serif; background-color: #f9f9f9; margin: 0; padding: 0;\">\r\n" +
                            "<div style=\"max-width: 600px; margin: 0 auto; padding: 20px; background-color: #ffffff; border-radius: 5px; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);\">" +
                            "<div style=\"text-align: center; margin-bottom: 20px;\">" +
                            "<a style=\"color: #00466a; text-decoration: none; font-size: 1.4em; font-weight: 600;\">Registration Completed</a>" +
                            "</div>" +
                            "<div style=\"font-size: 1.1em; margin-bottom: 20px;\">" +
                            "<p>Hi,</p>" +
                            "<p>Please use the following Company Code, ContactNo and Password to login on Application.  Company Code is :</p>" +
                             "<h2 style=\"background-color: #00466a; color: #ffffff; margin: 0 auto; width: max-content; padding: 0 10px; border-radius: 4px;\">" +
                             user.com_code +
                            "</h2>" +
                            "<p>ContactNo is :</p>" +
                            "<h2 style=\"background-color: #00466a; color: #ffffff; margin: 0 auto; width: max-content; padding: 0 10px; border-radius: 4px;\">" +
                             user.Contact_no +
                            "</h2>" +
                            "<p>Password is :</ p > " +
                            "<h2 style=\"background-color: #00466a; color: #ffffff; margin: 0 auto; width: max-content; padding: 0 10px; border-radius: 4px;\">" +
                             user.com_password +
                            "</h2>" +
                            "<p>Regards,<br>iBillCraft</p>" +
                            "</div>" +
                            "</div>" +
                            "</div>";
            // Create a new message and set the HTML content
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_configuration["email:CompanyName"], _configuration["email:EmailId"])); // set your email
            message.To.Add(new MailboxAddress("Hello User", user.EmailId)); // recipient email
            message.Subject = "Registration Completed";
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = htmlContent;
            message.Body = bodyBuilder.ToMessageBody();
            int smtpPort = int.Parse(_configuration["email:SMTPPort"]);
            using (var client = new SmtpClient())
            {
                client.Connect("smtpout.secureserver.net", 465, true); // SMTP server and port
                client.Authenticate("info@initialInfinity.com", "Feb#20244"); // Your email address and password
                client.Send(message);
                client.Disconnect(true);
            }
            return Ok();
        }

        [HttpGet("GetExcel")]

        public async Task<IActionResult> GetExcel(Guid UserId)
        {
            try
            {
                CompanyDetails user = new CompanyDetails();
                user.UserId = UserId;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                dynamic createduser = await _cRepo.GetAll(user);
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
        public async Task<IActionResult> GetPdf(Guid? UserId)
        {
            try { 
            CompanyDetails user = new CompanyDetails();
            user.UserId = UserId;
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.BaseModel.OperationType = "GetAll";
            dynamic createdUser = await _cRepo.GetAll(user);
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
                htmlContent += "        <h3 style='font-weight: bold;'>Server Allocation Details</h3>";
                htmlContent += "    </div>";
                htmlContent += "    <table style='width:100%; border-collapse: collapse; margin-top: 10px'>";
                htmlContent += "        <thead>";
                htmlContent += "            <tr>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Company Name</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Company Code</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Contact No.</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>EmailId</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Staff No.</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Country</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Package Name</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Package Amount</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Subscription Start Date</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Subscription End Date</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Payment Mode</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Server Key</th>";
                htmlContent += "                <th style='border: 1px solid black; background-color: gray; color: white; padding: 8px;'>Server Allotted Date</th>";
                htmlContent += "            </tr>";
                htmlContent += "        </thead>";
                htmlContent += "        <tbody>";
                foreach (DataRow row in data.Rows)
                {
                    string? com_company_name = row["com_company_name"].ToString();
                    string? com_code = row["com_code"].ToString();
                    string? com_contact = row["com_contact"].ToString();
                    string? com_email = row["com_email"].ToString();
                    string? com_staff_no = row["com_staff_no"].ToString();
                    string? CountryId = row["CountryId"].ToString();
                    string? package_name = row["package_name"].ToString();
                    string? final_Amount = row["final_Amount"].ToString();
                    string? StartDate = row["StartDate"].ToString();
                    string? EndDate = row["EndDate"].ToString();
                    string? Payment_Mode = row["Payment_Mode"].ToString();
                    string? Server_Key = row["Server_Key"].ToString();
                    string? Allotted_Date = row["Allotted_Date"].ToString();
                    htmlContent += "<tr style='border: 1px solid black;'>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + com_company_name + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + com_code + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + com_contact + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + com_email + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + com_staff_no + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + CountryId + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + package_name + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + final_Amount + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + StartDate + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + EndDate + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + Payment_Mode + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + Server_Key + "</td>";
                    htmlContent += "    <td style='border: 1px solid black; padding: 8px;'>" + Allotted_Date + "</td>";
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
