using Master.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Master.Repository.Interface
{
    public interface ISentClientRepository
    {
        public Task<IActionResult> SentClient(SentClientModel model);
        public Task<IActionResult> Get(SentClientModel model);
    }
}
