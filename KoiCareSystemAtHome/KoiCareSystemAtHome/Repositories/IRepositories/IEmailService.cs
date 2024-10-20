namespace KoiCareSystemAtHome.Repositories.IRepositories
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string body);
        Task SendVerificationEmailAsync(string email, string verificationLink);
    }
}
