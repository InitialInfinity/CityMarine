using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{
    public interface IInboxEmailRepository
    {
        public Task<IActionResult> InboxEmail(InboxEmailModel model);
        public Task<IActionResult> Get(InboxEmailModel model);
    }
}
