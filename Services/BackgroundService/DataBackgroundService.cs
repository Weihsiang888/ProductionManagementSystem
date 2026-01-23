using DxBlazorApplication7.Data;

namespace DxBlazorApplication7.Services
{
    public class DataBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _ServiceProvider;
        //private readonly DataService _DataService;

        public DataBackgroundService(IServiceProvider serviceProvider, ILogger<DataBackgroundService> logger)
        {
            _ServiceProvider = serviceProvider;
            //_DataService = _ServiceProvider.GetRequiredService<DataService>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //bool isFirstRun = true;

            using var scope = _ServiceProvider.CreateScope();
            var dataService = scope.ServiceProvider.GetRequiredService<DataService>();

            while (!stoppingToken.IsCancellationRequested)
            {
                //if (!isFirstRun)
                //{
                try
                {
                    await dataService.GetESOPDirectoryStatusAsync(@"\\172.25.210.8\01.product$\411 Standard Operation Procedure");
                }
                catch
                {

                }
                //}
                //else
                //{
                //    isFirstRun = false;
                //}

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
