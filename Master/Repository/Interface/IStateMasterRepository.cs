using Master.API.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.API.Repository.Interface
{
    public interface IStateMasterRepository
    {
        public Task<IActionResult> State(StateMaster model);

        public Task<IActionResult> Get(StateMaster model);

    }
}
