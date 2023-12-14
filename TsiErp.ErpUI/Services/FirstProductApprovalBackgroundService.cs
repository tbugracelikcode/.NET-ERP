namespace TsiErp.ErpUI.Services
{
    public class FirstProductApprovalBackgroundService : BackgroundService
    {
        public static event Func<DateTime, Task> UpdateEvent;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(10000);
                await UpdateEvent?.Invoke(DateTime.Now);
            }
        }
    }
}
