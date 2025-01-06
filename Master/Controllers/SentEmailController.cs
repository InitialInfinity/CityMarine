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
    public class SentEmailController : ControllerBase
    {
        private readonly ISentEmailRepository _sentemail;

        public SentEmailController(ISentEmailRepository sentemail)
        {
            _sentemail = sentemail;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid UserId, string type)
        {
            try
            {
                SentEmailModel user = new SentEmailModel();
                user.UserId = UserId;
                user.s_type = type;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetAll";
                var createduser = await _sentemail.SentEmail(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetEmail")]
        public async Task<IActionResult> GetEmail(Guid UserId,string? from, string? to, string?subject,string? hasthewords, string type)
        {
            try
            {
                SentEmailModel user = new SentEmailModel();
                user.UserId = UserId;
                user.s_from = from;
                user.s_to = to;
                user.s_subject = subject;
                user.s_body= hasthewords;
                user.s_type = type;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetEmail";
                var createduser = await _sentemail.SentEmail(user);
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
                SentEmailModel user = new SentEmailModel();
                user.UserId = UserId;
                user.s_type = type;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "General";
                var createduser = await _sentemail.SentEmail(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Generalfilter")]
        public async Task<IActionResult> Generalfilter(Guid UserId, string? from, string? to, string? subject, string? hasthewords, string type)
        {
            try
            {
                SentEmailModel user = new SentEmailModel();
                user.UserId = UserId;
                user.s_from = from;
                user.s_to = to;
                user.s_subject = subject;
                user.s_body = hasthewords;
                user.s_type = type;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "Generalfilter";
                var createduser = await _sentemail.SentEmail(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("Get")]
        public async Task<IActionResult> Get(Guid UserId, string s_id)
        {
            try
            {
                SentEmailModel user = new SentEmailModel();
                user.UserId = UserId;
                user.s_id = s_id;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetClientFile";
                var createduser = await _sentemail.SentEmail(user);
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
