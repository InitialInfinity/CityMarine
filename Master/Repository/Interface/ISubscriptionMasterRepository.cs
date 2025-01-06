using Master.API.Entity;
using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{
    public interface ISubscriptionMasterRepository
    {
        public Task<IActionResult> Subscription(Subscription model);
        public Task<IActionResult> Get(Subscription model);
    }
}
