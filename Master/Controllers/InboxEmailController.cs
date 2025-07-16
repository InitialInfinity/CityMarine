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
    public class InboxEmailController : ControllerBase
    {
        private readonly IInboxEmailRepository _sentemail;

        public InboxEmailController(IInboxEmailRepository sentemail)
        {
            _sentemail = sentemail;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid UserId, string type)
        {
            try
            {
                InboxEmailModel user = new InboxEmailModel();
                user.UserId = UserId;
                user.i_type = type;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                var createduser = await _sentemail.InboxEmail(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet("EnquiryClaim")]
        public async Task<IActionResult> EnquiryClaim(Guid UserId, string type)
        {
            try
            {
                InboxEmailModel user = new InboxEmailModel();
                user.UserId = UserId;
                user.i_type = type;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetEnquiryClaim";
                var createduser = await _sentemail.InboxEmail(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Showdetails")]
        public async Task<IActionResult> Showdetails(Guid UserId, string i_id)
        {
            try
            {
                InboxEmailModel user = new InboxEmailModel();
                user.UserId = UserId;
                user.i_id = i_id;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "Showdetails";
                var createduser = await _sentemail.Get(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("inboxdates")]
        public async Task<IActionResult> inboxdates(Guid UserId, string id)
        {
            try
            {
                InboxEmailModel user = new InboxEmailModel();
                user.UserId = UserId;
                user.i_id = id;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "inboxdates";
                var createduser = await _sentemail.InboxEmail(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("EnquiryClaimfilter")]
        public async Task<IActionResult> EnquiryClaimfilter(Guid? UserId, string? from, string? to, string? subject, string? hasthewords, string? type)
        {
            try
            {
                InboxEmailModel user = new InboxEmailModel();
                user.UserId = UserId;
                user.i_from = from;
                user.i_to = to;
                user.i_subject = subject;
                user.i_body = hasthewords;
                user.i_type = type;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "EnquiryClaimfilter";
                var createduser = await _sentemail.InboxEmail(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("Generalfilter")]
        public async Task<IActionResult> Generalfilter(Guid? UserId, string? from, string? to, string? subject, string? hasthewords, string? type)
        {
            try
            {
                InboxEmailModel user = new InboxEmailModel();
                user.UserId = UserId;
                user.i_from = from;
                user.i_to = to;
                user.i_subject = subject;
                user.i_body = hasthewords;
                user.i_type = type;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "Generalfilter";
                var createduser = await _sentemail.InboxEmail(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }















        [HttpGet("General")]
        public async Task<IActionResult> General(Guid UserId, string type)
        {
            try
            {
                InboxEmailModel user = new InboxEmailModel();
                user.UserId = UserId;
                user.i_type = type;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "General";
                var createduser = await _sentemail.InboxEmail(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("Generaltype")]
        public async Task<IActionResult> Generaltype(Guid UserId, string i_generaltype)
        {
            try
            {

                InboxEmailModel user = new InboxEmailModel();
                user.UserId = UserId;

                user.i_generaltype = i_generaltype;
                user.i_type = "General";
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "Generaltypes";
                var createduser = await _sentemail.InboxEmail(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        

       

        [HttpGet("dropdownclaimno")]
        public async Task<IActionResult> dropdownclaimno(Guid UserId, string i_type)
        {
            try
            {
                InboxEmailModel user = new InboxEmailModel();
                user.UserId = UserId;
                user.i_type = i_type;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "dropdownClaimoneNo";
                var createduser = await _sentemail.InboxEmail(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("dropdownenquiryno")]
        public async Task<IActionResult> dropdownenquiryno(Guid UserId, string i_type)
        {
            try
            {
                InboxEmailModel user = new InboxEmailModel();
                user.UserId = UserId;
                user.i_type = i_type;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "dropdownenquiryno";
                var createduser = await _sentemail.InboxEmail(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("ClaimoneNo")]
        public async Task<IActionResult> ClaimoneNo(Guid UserId, string i_type,  string i_claimno)
        {
            try
            {
                InboxEmailModel user = new InboxEmailModel();
                user.UserId = UserId;                
                user.i_type = i_type;
                if (i_claimno == "--Select Claim No--")
                {
                    i_claimno = "";
                }
                if (i_claimno == "--Select Enquiry No--")
                {
                    i_claimno = "";
                }
                user.i_claimno = i_claimno;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "ClaimoneNo";
                var createduser = await _sentemail.InboxEmail(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
