using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{
    public interface IEmailRuleConfgRepository
    {
        public Task<IActionResult> EmailRuleConfg(EmailRuleConfg model);
        public Task<IActionResult> Get(EmailRuleConfg model);

    }
}
