using common.Token;
using Context;
using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using MimeKit;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using Tokens;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginDetailsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILoginDetailsRepository _userRepo;
        private readonly DapperContext _context;
        //private readonly IUserRepository _Repo;
        public LoginDetailsController(
           ILoginDetailsRepository userRepo, //IUserRepository Repo,
           IConfiguration configuration,
           DapperContext context)
        {
            _configuration = configuration;
            _userRepo = userRepo;
            _context = context;
            // _Repo = Repo;
        }
        [HttpGet("Get")]
        public async Task<IActionResult> Get(string? ip_address, string? browserName, string? browserVersion)
        {
            try
            {
                LoginDetails user = new LoginDetails();
                user.browser_version = browserVersion;
                user.browser_name = browserName;
                user.ip_address = ip_address;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "Get";
                object result = new { };
                dynamic createduser = await _userRepo.ValidateServer(user);
                var outcomevalue = createduser.Value.Outcome.OutcomeDetail;
                var outcomeidvalue = createduser.Value.Outcome.OutcomeId;
                var outcomedata = createduser.Value.Data;
                if (outcomeidvalue == 1 && outcomevalue == "Session found" && outcomedata != null)
                {
                    var data = createduser.Value.Data;
                    string Server_Value = data.Server_Value;
                    string com_id = data.UserId;
                    string com_code = data.com_code;
                    LoginDetails login = new LoginDetails();
                    login.com_id = com_id;

                    if (login.BaseModel == null)
                    {
                        login.BaseModel = new BaseModel();
                    }
                    login.BaseModel.Server_Value = Server_Value;
                    login.BaseModel.OperationType = "GetCredintials";
                    dynamic logindetails = await _userRepo.ValidateEmail(login);
                    if (data.is_signIn == "1" && logindetails.Value.Data != null && logindetails.Value.Outcome.OutcomeDetail == "login credintials")
                    {
                        var modeldata = logindetails.Value.Data;
                        string Contact_no = modeldata.contact_no;
                        string? com_company_name = modeldata.com_company_name;

                        string com_password = modeldata.com_password;
                        string loginId = outcomedata.ss_id.ToString();
                        string RoleId = modeldata.urm_role_id.ToString();
                        string staffid = modeldata.staffid.ToString();
                        string co_country_code = modeldata.co_country_code;
                        string co_timezone = modeldata.co_timezone;
                        string cm_currency_format = modeldata.cm_currency_format;
                        string cm_currencysymbol = modeldata.cm_currencysymbol;
                        string rolename = modeldata.r_rolename;
                        result = new { Contact_no, com_code, com_password, com_id, Server_Value, loginId, com_company_name, staffid, RoleId, rolename, co_country_code, co_timezone, cm_currency_format, cm_currencysymbol };
                    }
                    else if (data.is_signIn == "0" && logindetails.Value.Data != null)
                    {
                        var modeldata = logindetails.Value.Data;
                        string Contact_no = modeldata.contact_no;
                        string loginId = outcomedata.ss_id.ToString();
                        string staffid = modeldata.staffid.ToString();
                        string RoleId = modeldata.urm_role_id.ToString();
                        string rolename = modeldata.r_rolename;
                        string co_country_code = modeldata.co_country_code;
                        string co_timezone = modeldata.co_timezone;
                        string cm_currency_format = modeldata.cm_currency_format;
                        string cm_currencysymbol = modeldata.cm_currencysymbol;
                        result = new { Contact_no, com_code, com_id, loginId, staffid, RoleId, rolename, co_country_code, co_timezone, cm_currency_format, cm_currencysymbol };
                    }
                    return Ok(result);
                }
                else
                {
                    return Ok(result);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDetails model)
        {
            if (model.BaseModel == null)
            {
                model.BaseModel = new BaseModel();
            }
            model.BaseModel.OperationType = "ValidateServer";
            jwtTokenCreate tk = new jwtTokenCreate(_configuration);
            try
            {
                dynamic createduser = await _userRepo.ValidateServer(model);
                var outcomes = createduser.Value.Outcome;
                var Model = createduser.Value.Data;
                Result result;

                if (Model != null)
                {
                    model.BaseModel.Server_Value = Model.Server_Key;
                    model.com_id = Model.com_id;
                    if (Model.com_id != null)
                    {
                        model.BaseModel.OperationType = "ValidateLogin";
                        dynamic loginuser = await _userRepo.ValidateEmail(model);
                        var loginoutcomes = loginuser.Value.Outcome;
                        var loginModel = loginuser.Value.Data;
                        string? RoleId = loginModel.urm_role_id;
                        Model.RoleId = RoleId;
                        string? Username = loginModel.um_user_name;
                        Model.Username = Username;
                        Model.com_company_name = loginModel.com_company_name;
                        Model.staffid = loginModel.staffid;
                        Model.rolename = loginModel.r_rolename;
                        Model.co_country_code = loginModel.co_country_code;
                        Model.co_timezone = loginModel.co_timezone;
                        Model.cm_currency_format = loginModel.cm_currency_format;
                        Model.cm_currencysymbol = loginModel.cm_currencysymbol;
                        Model.st_staff_name = loginModel.st_staff_name
;
                        if (loginModel != null)
                        {
                            var authClaims = new List<Claim>
                            {
                                 new Claim(ClaimTypes.MobilePhone,model.Contact_no),
                                 new Claim(ClaimTypes.NameIdentifier, loginModel.com_id.ToString()),
                                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                            };
                            var token = GetToken(authClaims);
                            string s = Model.com_id;
                            var Token = new JwtSecurityTokenHandler().WriteToken(token);
                            var expiration = token.ValidTo.ToString();
                            var OperationType = "InsertToken";
                            TokenRepo TR = new TokenRepo(_context);
                            var InsterToken = await TR.InsertToken(Token, expiration, s, OperationType, model.BaseModel.Server_Value);
                            if (loginoutcomes.OutcomeDetail == "login successfully")
                            {
                                LoginDetails loginDetails = new LoginDetails();
                                if (loginDetails.BaseModel == null)
                                {
                                    loginDetails.BaseModel = new BaseModel();
                                }
                                loginDetails.BaseModel.OperationType = "InsertSessionDetails";
                                loginDetails.com_id = Model.com_id;
                                loginDetails.ip_address = model.ip_address;
                                loginDetails.browser_name = model.browser_name;
                                loginDetails.browser_version = model.browser_version;
                                loginDetails.BaseModel.Server_Value = model.BaseModel.Server_Value;
                                loginDetails.CreatedDate = model.CreatedDate;
                                loginDetails.is_signIn = model.is_signIn;
                                loginDetails.com_code = model.com_code;
                                //loginDetails.is_signIn = "1";
                                dynamic insertsession = await _userRepo.ValidateServer(loginDetails);
                                var sessiondata = insertsession.Value.Data;
                                Model.LoginId = sessiondata.ss_id;

                                string encrypttext = tk.Encrypt(Token, "abcdefghijklmnop");

                                Outcome outcome = new Outcome
                                {
                                    OutcomeId = loginoutcomes.OutcomeId,
                                    OutcomeDetail = loginoutcomes.OutcomeDetail,
                                    Tokens = encrypttext,
                                    Expiration = expiration
                                };
                                 result = new Result
                                {
                                    Data = Model,
                                    Outcome = outcome,
                                    //UserId= createduser.Value.Data.Id
                                };

                                return Ok(new { result });
                            }
                            else
                            {
                                Outcome outcome = new Outcome
                                {

                                    OutcomeId = 0,
                                    OutcomeDetail = "Please enter valid credentials",
                                    Tokens = null,
                                    Expiration = null
                                };
                                 result = new Result
                                {
                                    Data = null,
                                    Outcome = outcome,
                                    //UserId= createduser.Value.Data.Id
                                };

                                return Ok(new { result });
                            }
                        }
                        else
                        {
                            Outcome outcome = new Outcome
                            {

                                OutcomeId = 0,
                                OutcomeDetail = "Please enter valid credentials",
                                Tokens = null,
                                Expiration = null
                            };
                             result = new Result
                            {
                                Data = null,
                                Outcome = outcome,
                                //UserId= createduser.Value.Data.Id
                            };

                            return Ok(new { result });
                        }

                    }
                    Outcome outcome2 = new Outcome
                    {

                        OutcomeId = 0,
                        OutcomeDetail = "Please enter valid credentials",
                        Tokens = null,
                        Expiration = null
                    };
                     result = new Result
                    {
                        Data = null,
                        Outcome = outcome2,
                        //UserId= createduser.Value.Data.Id
                    };

                    return Ok(new { result });
                }

                else
                {
                    // Authentication failed
                    if (outcomes.OutcomeId == 2)
                    {
                        Outcome outcome = new Outcome
                        {

                            OutcomeId = 0,
                            OutcomeDetail = "Company not found",
                            Tokens = null,
                            Expiration = null
                        };
                         result = new Result
                        {
                            Data = null,
                            Outcome = outcome,
                            //UserId= createduser.Value.Data.Id
                        };

                        return Ok(new { result });
                    }
                    else
                    {

                        Outcome outcome = new Outcome
                        {

                            OutcomeId = 0,
                            OutcomeDetail = "Please enter valid credentials",
                            Tokens = null,
                            Expiration = null
                        };
                         result = new Result
                        {
                            Data = null,
                            Outcome = outcome,
                            //UserId= createduser.Value.Data.Id
                        };

                        return Ok(new { result });
                    }
                    //Response.Headers.Add("WWW-Authenticate", "YourChallengeHere");
                    //    return StatusCode(StatusCodes.Status403Forbidden, "Invalid Credintial.");
                }
            }
            //throw excetption
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut([FromBody] LoginDetails model)
        {
            string OperationType = "DeleteToken";
            TokenRepo tr = new TokenRepo(_context);
            string connstring = model.server_Value;
            try
            {
                TokenInfo token = new TokenInfo();

                if (token.BaseModel == null)
                {
                    token.BaseModel = new BaseModel();
                }
                token.BaseModel.Server_Value = connstring;
                token.LoginId = model.LoginId;
                token.UserId = model.com_id;
                token.browser_name = model.browser_name;
                token.ip_address = model.ip_address;
                token.browser_version = model.browser_version;
                token.CreatedDate = model.CreatedDate;
                token.BaseModel.OperationType = OperationType;

                var InsterToken = await tr.DeleteToken(token);
                return InsterToken;
                //var createdUserList = createduser.ToList()
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost("ForgotPass")]
        public async Task<IActionResult> ForgotPass([FromBody] LoginDetails model)
        {
            if (model.BaseModel == null)
            {
                model.BaseModel = new BaseModel();
            }
            model.BaseModel.OperationType = "ValidateServer";
            jwtTokenCreate tk = new jwtTokenCreate(_configuration);
            try
            {
                dynamic createdusers = await _userRepo.ValidateServer(model);
                var outcomes = createdusers.Value.Outcome;
                var Model = createdusers.Value.Data;
                if (Model != null)
                {
                    model.BaseModel.Server_Value = Model.Server_Key;
                    model.com_id = Model.com_id;
                    if (Model.com_id != null)
                    {

                        model.BaseModel.OperationType = "ValidateEmail";
                        dynamic createduser = await _userRepo.ValidateEmail(model);
                        var outcomess = createduser.Value.Outcome;
                        var data = createduser.Value.Data;
                        if (outcomess.OutcomeId == 1)
                        {
                            //user.Password = password;
                            var EmailId = data.EmailId;
                            var Password = data.Password;
                            //const string CharSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

                            //// Define the desired password length
                            //const int PasswordLength = 8; // You can change this to your desired length

                            //// Create a Random object
                            //Random random = new Random();

                            //// Generate a random password
                            //StringBuilder passwordBuilder = new StringBuilder();
                            //for (int i = 0; i < PasswordLength; i++)
                            //{
                            //    int index = random.Next(0, CharSet.Length);
                            //    char randomChar = CharSet[index];
                            //    passwordBuilder.Append(randomChar);
                            //}

                            //string password = passwordBuilder.ToString();
                            ////user.BaseModel.OperationType = "InsertStaffPassword";
                            ////jwtTokenCreate tk = new jwtTokenCreate(_configuration);
                            //model.com_password = password;//tk.Encrypt(password, "abcdefghijklmnop");
                            //                              //user.Password = password;

                            //model.UserId = data.UserId;
                            //model.com_id = data.com_id;
                            //model.BaseModel.OperationType = "UpdatePass";
                            //dynamic pass = await _userRepo.ValidateEmail(model);
                            //var outcomes1 = createduser.Value.Outcome;

                            // var sendPass = await _userRepo.SavePass(user);
                            //if (outcomes1.OutcomeId == 1)
                            //{

                            string htmlContent = "<div style=\"font-family: Arial, sans-serif; background-color: #f9f9f9; margin: 0; padding: 0;\">\r\n" +
                                     "<div style=\"max-width: 600px; margin: 0 auto; padding: 20px; background-color: #ffffff; border-radius: 5px; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);\">" +
                                     "<div style=\"text-align: center; margin-bottom: 20px;\">" +
                                     "<a style=\"color: #00466a; text-decoration: none; font-size: 1.4em; font-weight: 600;\">Current Password</a>" +
                                     "</div>" +
                                     "<div style=\"font-size: 1.1em; margin-bottom: 20px;\">" +
                                     "<p>Hi,</p>" +
                                     "<p>Please use the following ContactNo and Password to login on Application. ContactNo is :</p>" +
                                     "<h2 style=\"background-color: #00466a; color: #ffffff; margin: 0 auto; width: max-content; padding: 0 10px; border-radius: 4px;\">" +
                                      model.Contact_no +
                                     "</h2>" +
                                     "<p>Password is :</ p > " +
                                     "<h2 style=\"background-color: #00466a; color: #ffffff; margin: 0 auto; width: max-content; padding: 0 10px; border-radius: 4px;\">" +
                                      Password +
                                     "</h2>" +
                                     "<p>Regards,<br>Infinity Staff Pro</p>" +
                                     "</div>" +

                                     "</div>" +
                                     "</div>";

                            // Create a new message and set the HTML content
                            var message = new MimeMessage();
                            message.From.Add(new MailboxAddress(_configuration["email:CompanyName"], _configuration["email:EmailId"])); // set your email
                            message.To.Add(new MailboxAddress("Hello User", EmailId)); // recipient email

                            message.Subject = "Password";
                            var bodyBuilder = new BodyBuilder();
                            bodyBuilder.HtmlBody = htmlContent;
                            message.Body = bodyBuilder.ToMessageBody();
                            int smtpPort = int.Parse(_configuration["email:SMTPPort"]);
                            // Configure the SMTP client and send the message
                            using (var client = new SmtpClient())
                            {
                                //client.Connect("relay-hosting.secureserver.net", 25, true); // SMTP server and port
                                //client.Connect(_configuration["email:SMTPServer"], smtpPort, true); // SMTP server and port
                                //                                                                    //client.Connect("smtpout.secureserver.net", 587, false); // SMTP server and port
                                //client.Authenticate(_configuration["email:EmailId"], _configuration["email:Password"]); // Your email address and password
                                //client.Send(message);
                                //client.Disconnect(true);
                                client.Connect("smtpout.secureserver.net", 465, true); // SMTP server and port
                                client.Authenticate("info@initialInfinity.com", "Feb#20244"); // Your email address and password
                                client.Send(message);
                                client.Disconnect(true);
                            }
                            // return pass;
                            // }

                            //else
                            //{

                            //    Outcome outcome = new Outcome
                            //    {

                            //        OutcomeId = 0,
                            //        OutcomeDetail = "Error Occured!",
                            //        Tokens = null,
                            //        Expiration = null
                            //    };
                            //    return Ok(outcome);
                            //}
                        }
                        else
                        {
                            // Authentication failed
                            if (outcomes.OutcomeId == 2)
                            {
                                Outcome outcome = new Outcome
                                {

                                    OutcomeId = 0,
                                    OutcomeDetail = "Company not found",
                                    Tokens = null,
                                    Expiration = null
                                };
                                return Ok(outcome);
                            }
                            else
                            {

                                Outcome outcome = new Outcome
                                {

                                    OutcomeId = 0,
                                    OutcomeDetail = "Please enter valid credentials",
                                    Tokens = null,
                                    Expiration = null
                                };
                                return Ok(outcome);

                            }

                        }
                    }
                    //return outcomes.OutcomeDetail;
                }
                //return outcomes.OutcomeDetail;
                return Ok(outcomes);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] LoginDetails user)
        {
            try
            {


                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }

                user.BaseModel.OperationType = "ResetPassword";


                var createduser = await _userRepo.ValidateEmail(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetIsfirst")]
        public async Task<IActionResult> GetIsfirst1(string? com_id,string Server_Value)
        {
            try
            {
                LoginDetails user = new LoginDetails();
                user.com_id = com_id;

                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();

                }

                user.BaseModel.Server_Value = "SqlConnection";
                user.BaseModel.OperationType = "GetLoginCount";

                var createduser = await _userRepo.ValidateEmail(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }


        }



        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(5),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

    }
}
