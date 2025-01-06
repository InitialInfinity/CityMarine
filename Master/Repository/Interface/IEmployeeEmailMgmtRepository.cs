using Master.API.Entity;
using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{
    public interface IEmployeeEmailMgmtRepository
    {
        public Task<IActionResult> EmployeeEmailMgmt(EmployeeEmailMgmtModel model);
        public Task<IActionResult> Get(EmployeeEmailMgmtModel model);
    }
}
