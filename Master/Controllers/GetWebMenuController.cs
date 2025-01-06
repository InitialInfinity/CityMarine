using Master.API.Entity;
using Master.API.Repository.Interface;
using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Tokens;

namespace Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetWebMenuController : Controller
    {
        private readonly IGetWebMenuRepository _getwebmenuRepo;
        public GetWebMenuController(IGetWebMenuRepository getwebmenuRepo)
        {
            _getwebmenuRepo = getwebmenuRepo;

        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(Guid? RoleId, string? server)
        {
            try
            {
                GetWebMenu user = new GetWebMenu();
                user.RoleId = RoleId;

                if (user.BaseModel == null)
                {
                    user.BaseModel = new BaseModel();
                }

                user.BaseModel.OperationType = "GetWebMenu";
                user.BaseModel.Server_Value = server;
				var createduser = await _getwebmenuRepo.GetWebMenuR(user);
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
