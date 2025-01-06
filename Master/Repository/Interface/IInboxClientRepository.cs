using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{
    public interface IInboxClientRepository
    {
        public Task<IActionResult> InboxClient(InboxClientModel model);
        public Task<IActionResult> Get(InboxClientModel model);
    }
}
