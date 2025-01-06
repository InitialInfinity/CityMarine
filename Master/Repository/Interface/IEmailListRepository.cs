using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{
    public interface IEmailListRepository
    {
        public Task<IActionResult> EmailList(EmailList model);
        public Task<IActionResult> Get(EmailList model);

    }
}
