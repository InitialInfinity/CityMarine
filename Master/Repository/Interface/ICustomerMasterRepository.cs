
using Microsoft.AspNetCore.Mvc;
using Master.Entity;

namespace Master.Repository.Interface
{
    public interface ICustomerMasterRepository
    {
        public Task<IActionResult> Customer(CustomerMaster model);
        public Task<IActionResult> Get(CustomerMaster model);
    }
}
