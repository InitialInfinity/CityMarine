using Master.Entity;
using Microsoft.AspNetCore.Mvc;
namespace Master.Repository.Interface
{
    public interface IUnitConfigurationRepository
    {
        public Task<IActionResult> UnitConfiguration(UnitConfiguration model);
        public Task<IActionResult> Get(UnitConfiguration model);
    }
}


