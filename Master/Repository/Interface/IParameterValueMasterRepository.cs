using Master.API.Entity;
using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.API.Repository.Interface
{ 
    public interface IParameterValueMasterRepository
    {
        public Task<IActionResult> ParameterValue(ParameterValueMaster model);
        public Task<IActionResult> Get(ParameterValueMaster model);
    }
}
