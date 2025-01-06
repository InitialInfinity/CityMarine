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
    public class SentClientController : ControllerBase
    {

        private readonly ISentClientRepository _sentclient;

        public SentClientController(ISentClientRepository sentclient)
        {
            _sentclient = sentclient;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid UserId, string sc_to,string sc_type)
        {
            try
            {
                SentClientModel user = new SentClientModel();
                user.UserId = UserId;
                user.sc_to = sc_to;
                user.sc_type = sc_type;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "Count";
                var createduser = await _sentclient.SentClient(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetDetails")]
        public async Task<IActionResult> GetDetails(Guid UserId, string sc_year,string sc_to,string sc_type)
        {
            try
            {
                SentClientModel user = new SentClientModel();
                user.UserId = UserId;
                user.sc_year = sc_year;
                user.sc_to = sc_to;
                user.sc_type=sc_type;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetDetails";
                var createduser = await _sentclient.SentClient(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get(Guid UserId, string sc_id)
        {
            try
            {
                SentClientModel user = new SentClientModel();
                user.UserId = UserId;
                user.sc_id = sc_id;
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
        public async Task<IActionResult> Clientchange(Guid UserId, string clientid, string sc_type)
        {
            try
            {
                SentClientModel user = new SentClientModel();
                user.UserId = UserId;
                user.clientid = clientid;
                user.sc_type = sc_type;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "Clientchange";
                var createduser = await _sentclient.SentClient(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Clientchange1")]
        public async Task<IActionResult> Clientchange1(Guid UserId, string clientid, string sc_type)
        {
            try
            {
                SentClientModel user = new SentClientModel();
                user.UserId = UserId;
                user.clientid = clientid;
                user.sc_type = sc_type;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "Clientchange1";
                var createduser = await _sentclient.SentClient(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetEmail")]
        public async Task<IActionResult> GetEmail(Guid UserId, string? from, string? to, string? subject, string? hasthewords, string sc_year,string type)
        {
            try
            {
                SentClientModel user = new SentClientModel();
                user.UserId = UserId;
                user.sc_from = from;
                user.sc_to = to;
                user.sc_subject = subject;
                user.sc_body = hasthewords;
                user.sc_year=sc_year;
                user.sc_type = type;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "GetEmailClient";
                var createduser = await _sentclient.SentClient(user);
                var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetGeneral")]
        public async Task<IActionResult> GetGeneral(Guid UserId, string sc_type)
        {
            try
            {
                SentClientModel user = new SentClientModel();
                user.UserId = UserId;
                user.sc_type = sc_type;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.OperationType = "Generalcount";
                var createduser = await _sentclient.SentClient(user);
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
