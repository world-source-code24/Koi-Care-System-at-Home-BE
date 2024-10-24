using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoiCareSystemAtHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberRevenueController : ControllerBase
    {
        private readonly IMembershipRepository _membershipRepository;
        public MemberRevenueController(IMembershipRepository membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetMembershipRevenue()
        {
            var membershipRevenue = await _membershipRepository.GetRenevueMembership();
            return Ok(membershipRevenue);
        }
    }
}
