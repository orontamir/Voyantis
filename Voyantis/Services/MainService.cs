using Voyantis.Interfaces;

namespace Voyantis.Services
{
    public class MainService : IHostedService
    {
        private readonly IQueueService _queueService;

        public MainService(IQueueService queueService)
        {
            this._queueService = queueService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
           
            return Task.CompletedTask;
        }
    }
}
