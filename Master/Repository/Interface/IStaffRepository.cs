using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface

{
    public interface IStaffRepository
    {
        public Task<IActionResult> Staff(Staff model);
        public Task<IActionResult> Get(Staff model);
    }
}


