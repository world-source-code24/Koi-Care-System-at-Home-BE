using KoiCareSystemAtHome.Models;

namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
