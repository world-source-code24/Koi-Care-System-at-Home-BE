
using KoiCareSystemAtHome.Repositories.IRepositories;

namespace KoiCareSystemAtHome.Repositories
{
    public class MembershipCheckRepository : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public MembershipCheckRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckAndUpdateMembershipAsync();
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
        private async Task CheckAndUpdateMembershipAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IAccountRepository>();
                await userService.UpdateAndCheckAllUserRole();
            }
        }
    }
}
