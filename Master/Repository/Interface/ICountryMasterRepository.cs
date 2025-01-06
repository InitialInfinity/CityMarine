using Master.API.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.API.Repository.Interface
{
    public interface ICountryMasterRepository
    {
        public Task<IActionResult> Country(CountryMaster model);
        public Task<IActionResult> Get(CountryMaster model);
    }
}
