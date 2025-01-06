using Master.Repository.Interface;
using Master.Entity;
using Microsoft.AspNetCore.Mvc;
using Tokens;
using Common;
using System.Data;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffRepository _staffRepo;
        public StaffController(IStaffRepository staffRepo)
        {
            _staffRepo = staffRepo;

        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetStaff(Guid? UserId,string Server_Value,Guid? st_com_id)
        {
            try
            {
                Staff user = new Staff();
                user.UserId = UserId;
                user.st_com_id = st_com_id;
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.Server_Value = Server_Value;
                user.BaseModel.OperationType = "GetAll";
                var createduser = await _staffRepo.Staff(user);
                //var data = ((Microsoft.AspNetCore.Mvc.ObjectResult)createduser).Value;
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetStaff(Guid UserId, Guid? st_id,string Server_Value,Guid? st_com_id)
        {
            try
            {
                Staff user = new Staff();
                user.UserId = UserId;
                user.st_id = st_id;
                user.st_com_id = st_com_id;

                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.Server_Value = Server_Value;
                user.BaseModel.OperationType = "Get";
                var createduser = await _staffRepo.Get(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertStaff([FromBody] Staff user)
        {
            try
            {
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.Server_Value = user.Server_Value;
                if (user.st_id == null)
                {
                    user.BaseModel.OperationType = "Insert";
                }
                else
                {
                    user.BaseModel.OperationType = "Update";
                }
                if (user.st_contact==null)
                {
                    user.BaseModel.OperationType = "UpdateStatus";
                }
                var createduser = await _staffRepo.Get(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteStaff([FromBody] Staff user)
        {
            if (user.BaseModel == null)
            {
                user.BaseModel = new BaseModel();
            }
            user.BaseModel.Server_Value = user.Server_Value;
            user.BaseModel.OperationType = "Delete";
            var productDetails = await _staffRepo.Get(user);
            return productDetails;
        }
        [HttpGet("GetExcel")]
        public async Task<IActionResult> GetExcel(Guid UserId, string? Server_Value, string? st_com_id)
        {
            try
            {
                Staff user = new Staff();
                user.UserId = UserId;
                user.st_com_id = new Guid(st_com_id);
                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.Server_Value = Server_Value;
                user.BaseModel.OperationType = "GetAll";
                dynamic createduser = await _staffRepo.Staff(user);
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
                ExportRepository exportRepository = new ExportRepository();
                var result = new Result
                {
                    Data = exportRepository.DataTableToJsonObj(data)
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

    }
}
