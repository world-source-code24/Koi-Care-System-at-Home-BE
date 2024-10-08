namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public class IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
