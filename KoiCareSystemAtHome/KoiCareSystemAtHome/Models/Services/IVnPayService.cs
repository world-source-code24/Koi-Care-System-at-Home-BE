namespace KoiCareSystemAtHome.Models.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model, int accId);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
