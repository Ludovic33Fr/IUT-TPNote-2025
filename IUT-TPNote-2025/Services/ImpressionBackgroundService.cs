namespace IUT_TPNote_2025.Services
{
    public class ImpressionBackgroundService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await PrintJobManager.StartPooling(stoppingToken);
        }
    }

}
