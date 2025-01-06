using Master.API.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.API.Repository.Interface
{
    public interface ICityMasterRepository
    {
        public Task<IActionResult> City(CityMaster model);
        public Task<IActionResult> Get(CityMaster model);
    }
}
