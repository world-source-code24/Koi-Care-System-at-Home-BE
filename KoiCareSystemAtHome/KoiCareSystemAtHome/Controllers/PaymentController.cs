using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Models.Services;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoiCareSystemAtHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly KoicareathomeContext _context;
        private readonly IVnPayService _vpnPayService;
        private readonly IAccountRepository _accountRepository;
        public PaymentController(KoicareathomeContext context, IVnPayService vnPayService, IAccountRepository accountRepository)
        {
            _context = context;
            _vpnPayService = vnPayService;
            _accountRepository = accountRepository;
        }
        [HttpPost("checkout")]
        public IActionResult CheckoutByVnPay(int accId)
        {
            var account = _context.AccountTbls.FirstOrDefault(acc => acc.AccId.Equals(accId));
            string name = account.Name;
            if(name == null)
            {
                name = "Customer";
            }
            var vnPayModel = new VnPaymentRequestModel
            {
                OrderId = new Random().Next(1000, 10000),
                Amount = 99000,
                CreatedDate = DateTime.Now,
                Description = "Thank you for purchasing our Membership",
                FullName = name
            };
            var paymentUrl = _vpnPayService.CreatePaymentUrl(HttpContext, vnPayModel, accId);
            return Ok(new { paymentUrl });
        }

        [HttpGet("callback")]
        public async Task<IActionResult> PaymentCallBack()
        {
            var respone = _vpnPayService.PaymentExecute(Request.Query);

            if(respone != null)
            {
                if(respone.Success && respone.VnPayResponseCode == "00")
                {
                    //luu vao database
                    if (int.TryParse(Request.Query["accId"], out int accId))
                    {
                        bool isSuccess = await _accountRepository.BuyMembership(accId);
                        if (isSuccess)
                        {
                            return Redirect("https://koicareathome.azurewebsites.net/payment");
                        }
                    }
                }
            }
            return Redirect("https://koicareathome.azurewebsites.net/paymentFail");
        }
    }
}
