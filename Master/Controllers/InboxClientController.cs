using Common;
using Master.API.Entity;
using Master.API.Repository.Interface;
using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection.Metadata;
using System.Security.AccessControl;
using System.Text;
using Tokens;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InboxClientController : ControllerBase
    {
        private readonly IInboxClientRepository _sentclient;

        public InboxClientController(IInboxClientRepository sentclient)
        {
            _sentclient = sentclient;
        }


        [HttpGet("GetEnquiryClaimMail")]
        public async Task<IActionResult> GetEnquiryClaimMail(Guid UserId, string ic_year, string ic_from, string ic_type)
        {
            try
            {
                InboxClientModel user = new InboxClientModel();
                user.UserId = UserId;
                user.ic_year = ic_year;
                user.ic_from = ic_from;
                user.ic_type = ic_type;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetEnquiryClaimMail";
                var createduser = await _sentclient.InboxClient(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetAllAttchment")]
        public async Task<IActionResult> GetAllAttchment(Guid UserId, string ic_type, string ic_year, string ic_from)
        {
            try
            {
                InboxClientModel user = new InboxClientModel();
                user.UserId = UserId;
                user.ic_year = ic_year;
                user.ic_from = ic_from;
                user.ic_type = ic_type;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAllAttachment";
                var createduser = await _sentclient.InboxClient(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Clientchange1")]
        public async Task<IActionResult> Clientchange1(Guid UserId, string clientid, string ic_type, string ic_year)
        {
            try
            {
                InboxClientModel user = new InboxClientModel();
                user.UserId = UserId;
                user.clientid = clientid;
                user.ic_type = ic_type;
                user.ic_year = ic_year;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "Clientchange1";
                var createduser = await _sentclient.InboxClient(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("ClaimNo")]
        public async Task<IActionResult> ClaimNo(Guid UserId, string clientid, string ic_type, string ic_year, string ic_claimno)
        {
            try
            {
                InboxClientModel user = new InboxClientModel();
                user.UserId = UserId;
                user.clientid = clientid;
                user.ic_type = ic_type;
                user.ic_year = ic_year;
                if (ic_claimno == "--Select Claim No--")
                {
                    ic_claimno = "";
                }
                user.ic_claimno = ic_claimno;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "ClaimNo";
                var createduser = await _sentclient.InboxClient(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> EnquiryNo(Guid UserId, string clientid, string ic_type, string ic_year, string ic_enquiryno)
        {
            try
            {
                InboxClientModel user = new InboxClientModel();
                user.UserId = UserId;
                user.clientid = clientid;
                user.ic_type = ic_type;
                user.ic_year = ic_year;
                if (ic_enquiryno == "--Select Enquiry No--")
                {
                    ic_enquiryno = "";
                }
                user.ic_enquiryno = ic_enquiryno;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "EnquiryNo";
                var createduser = await _sentclient.InboxClient(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("dropdownclaimno")]
        public async Task<IActionResult> dropdownclaimno(Guid UserId, string ic_from, string ic_type)
        {
            try
            {
                InboxClientModel user = new InboxClientModel();
                user.UserId = UserId;
                user.ic_from = ic_from;
                user.ic_type = ic_type;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "dropdownClaimNo";
                var createduser = await _sentclient.InboxClient(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("dropdownenquiryno")]
        public async Task<IActionResult> dropdownenquiryno(Guid UserId, string ic_from, string ic_type)
        {
            try
            {
                InboxClientModel user = new InboxClientModel();
                user.UserId = UserId;
                user.ic_from = ic_from;
                user.ic_type = ic_type;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "dropdownenquiryno";
                var createduser = await _sentclient.InboxClient(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }















        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid UserId, string ic_from, string ic_type)
        {
            try
            {
                InboxClientModel user = new InboxClientModel();
                user.UserId = UserId;
                user.ic_from = ic_from;
                user.ic_type = ic_type;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "Count";
                var createduser = await _sentclient.InboxClient(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        


        

        [HttpGet("Get")]
        public async Task<IActionResult> Get(Guid UserId, string ic_id)
        {
            try
            {
                InboxClientModel user = new InboxClientModel();
                user.UserId = UserId;
                user.ic_id = ic_id;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "Get";
                var createduser = await _sentclient.Get(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Clientchange")]
        public async Task<IActionResult> Clientchange(Guid UserId, string clientid, string ic_type)
        {
            try
            {
                InboxClientModel user = new InboxClientModel();
                user.UserId = UserId;
                user.clientid = clientid;
                user.ic_type = ic_type;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "Clientchange";
                var createduser = await _sentclient.InboxClient(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        

        [HttpGet("filter")]
        public async Task<IActionResult> filter(Guid? UserId, string? from,string? to, string? subject, string? hasthewords, string? ic_year, string? type)
        {
            try
            {
                InboxClientModel user = new InboxClientModel();
                user.UserId = UserId;
                user.ic_from = from;
                user.ic_to = to;
               user.ic_subject = subject;
               user.ic_body = hasthewords;
                user.ic_year = ic_year;
                user.ic_type = type;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "filter";
                var createduser = await _sentclient.InboxClient(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet("GetGeneral")]
        public async Task<IActionResult> GetGeneral(Guid UserId, string ic_type)
        {
            try
            {
                InboxClientModel user = new InboxClientModel();
                user.UserId = UserId;
                user.ic_type = ic_type;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "Generalcount";
                var createduser = await _sentclient.InboxClient(user);
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
