using WorkItemProcessor.Interfaces;
using WorkItemProcessor.Models;

namespace WorkItemProcessor
{
    /// <summary>
    /// Worker service for managing work items.
    /// </summary>
    public class WorkItemProcessManager : BackgroundService
    {
        private readonly ILogger<WorkItemProcessManager> _logger;
        private readonly IQueuedWorkItemProcessor _workitemProcessor;
        private readonly IWorkItemMonitoringService _workitemMonitoringService;
        public WorkItemProcessManager(ILogger<WorkItemProcessManager> logger,
                                  IQueuedWorkItemProcessor workitemProcessor,
                                  IWorkItemMonitoringService workitemMonitoringService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _workitemProcessor = workitemProcessor ?? throw new ArgumentNullException(nameof(workitemProcessor));
            _workitemMonitoringService = workitemMonitoringService ?? throw new ArgumentNullException(nameof(workitemMonitoringService));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("WorkItem manager started at: {day} {time}.",
                                    DateTime.Now.DayOfWeek, DateTime.Now);

            _workitemProcessor.Start();

            while (!stoppingToken.IsCancellationRequested)
            {                           
                List<string> queuedItems = await _workitemMonitoringService.QueueWorkItems();

                _workitemMonitoringService.LogWorkingWorkItems();
                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}