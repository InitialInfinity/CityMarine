using Master.API.Entity;
using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{
    public interface IServerAllocation
    {
        public Task<IActionResult> GetAll(CompanyDetails user);
        public Task<IActionResult> Allocation(CompanyDetails user);
        public Task<IActionResult> AllocationCompany(LoginDetails user);
    }
}
