using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{
    public interface ISentEmailRepository
    {
        public Task<IActionResult> SentEmail(SentEmailModel model);
        public Task<IActionResult> Get(SentEmailModel model);
    }
}
