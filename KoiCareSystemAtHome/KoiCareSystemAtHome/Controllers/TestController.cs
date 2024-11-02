using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoiCareSystemAtHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        public TestController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        [HttpPut]
        public async Task<IActionResult> BuyMembership(int id)
        {
            bool isSuccess = await _accountRepository.BuyMembership(id);
            if (isSuccess)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
