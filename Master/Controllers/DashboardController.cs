using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tokens;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        public readonly IDashboardRepository _dashboardRepo;

        public DashboardController(IDashboardRepository dashRepo)
        {
            _dashboardRepo = dashRepo;
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(string RoleId, string Server_Value, string com_id)
        {
            try
            {
                Dashboard user = new Dashboard();
                
                user.com_id = com_id;

                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }
                user.BaseModel.Server_Value = Server_Value;
                user.BaseModel.OperationType = "GetDetails";

                var createduser = await _dashboardRepo.Get(user);
                return createduser;
            }
            catch (Exception)
            {
                throw;
            }
        }



        
		[HttpGet("GetSub")]
		public async Task<IActionResult> GetSub(string RoleId, string Server_Value, string com_id)
		{
			try
			{
				Dashboard user = new Dashboard();

				user.com_id = com_id;

				if (user.BaseModel == null)
				{
					user.BaseModel = new BaseModel();
				}
				user.BaseModel.Server_Value = Server_Value;
				user.BaseModel.OperationType = "GetSub";

				var createduser = await _dashboardRepo.Get(user);
				return createduser;
			}
			catch (Exception)
			{
				throw;
			}
		}

		[HttpGet("GetSaleCash")]
		public async Task<IActionResult> GetSaleCash(string RoleId, string Server_Value, string com_id)
		{
			try
			{
				Dashboard user = new Dashboard();

				user.com_id = com_id;

				if (user.BaseModel == null)
				{
					user.BaseModel = new BaseModel();
				}
				user.BaseModel.Server_Value = Server_Value;
				user.BaseModel.OperationType = "GetSaleCash";

				var createduser = await _dashboardRepo.GetList(user);
				return createduser;
			}
			catch (Exception)
			{
				throw;
			}
		}
		[HttpGet("GetExpenseValue")]
		public async Task<IActionResult> GetExpenseValue(string RoleId, string Server_Value, string com_id)
		{
			try
			{
				Dashboard user = new Dashboard();

				user.com_id = com_id;

				if (user.BaseModel == null)
				{
					user.BaseModel = new BaseModel();
				}
				user.BaseModel.Server_Value = Server_Value;
				user.BaseModel.OperationType = "GetExpenseValue";

				var createduser = await _dashboardRepo.GetList(user);
				return createduser;
			}
			catch (Exception)
			{
				throw;
			}
		}
		[HttpGet("Getprofitvalue")]
		public async Task<IActionResult> Getprofitvalue(string RoleId, string Server_Value, string com_id)
		{
			try
			{
				Dashboard user = new Dashboard();

				user.com_id = com_id;

				if (user.BaseModel == null)
				{
					user.BaseModel = new BaseModel();
				}
				user.BaseModel.Server_Value = Server_Value;
				user.BaseModel.OperationType = "Getprofitvalue";

				var createduser = await _dashboardRepo.GetList(user);
				return createduser;
			}
			catch (Exception)
			{
				throw;
			}
		}
		[HttpGet("Getprofitval")]
		public async Task<IActionResult> Getprofitval(string RoleId, string Server_Value, string com_id)
		{
			try
			{
				Dashboard user = new Dashboard();

				user.com_id = com_id;

				if (user.BaseModel == null)
				{
					user.BaseModel = new BaseModel();
				}
				user.BaseModel.Server_Value = Server_Value;
				user.BaseModel.OperationType = "Getprofitval";

				var createduser = await _dashboardRepo.GetList(user);
				return createduser;
			}
			catch (Exception)
			{
				throw;
			}
		}
		
	}
}
