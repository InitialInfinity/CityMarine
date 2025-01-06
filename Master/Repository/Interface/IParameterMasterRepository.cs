using Master.API.Entity;
using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.API.Repository.Interface
{ 
    public interface IParameterMasterRepository
    {
        public Task<IActionResult> Parameter(ParameterMaster model);
        public Task<IActionResult> Get(ParameterMaster model);

    }
}
