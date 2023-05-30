using ConcurrentProcessor.Models;
using WorkItemProcessor.Interfaces;
using WorkItemProcessor.Services;

namespace WorkItemProcessor
{
    /// <summary>
    /// Class for handling item processing.
    /// </summary>
    public class QueuedWorkItemProcessor : IQueuedWorkItemProcessor
    {
        private readonly CancellationToken _cancellationToken;
        private readonly IWorkItemCollection _workitemCollection;
        private readonly IConfigurationService _configService;
        private readonly ILogger<QueuedWorkItemProcessor> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private int MaxConCurrentItemProcessingThreads;
        public int CycleTimeInSeconds { get; set; }

        public QueuedWorkItemProcessor(ILoggerFactory loggerFactory, IWorkItemCollection itemCollection, IHostApplicationLifetime applicationLifetime,
                             IConfigurationService configService,
                             IWorkItemMonitoringService monitoringService)
        {
            _workitemCollection = itemCollection ?? throw new ArgumentNullException(nameof(itemCollection));
            _cancellationToken = applicationLifetime.ApplicationStopping;
            _configService = configService ?? throw new ArgumentNullException(nameof(configService));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _logger = loggerFactory.CreateLogger<QueuedWorkItemProcessor>();
        }

        public void Start()
        {
            _logger.LogInformation("Queued WorkItem Processor started.");
            MaxConCurrentItemProcessingThreads = _configService.GetMaxConCurrentItemProcessingThreads();
            CycleTimeInSeconds = 1;
            Task.Run(WorkItemMonitorLoop);
        }

        public async Task WorkItemMonitorLoop()
        {
            _logger.LogInformation("Queued item monitoring started. MaxConCurrentItemProcessingThreads = {max}", MaxConCurrentItemProcessingThreads);

            SemaphoreSlim semaphore = new SemaphoreSlim(MaxConCurrentItemProcessingThreads);

            while (!_cancellationToken.IsCancellationRequested)
            {
                while (_workitemCollection.ContainsWorkItem())
                {
                    await semaphore.WaitAsync(_cancellationToken);

                    WorkItem item = _workitemCollection.GetNext();
                    item.Logger = _loggerFactory.CreateLogger<WorkItem>();
                    _workitemCollection.AddWorkingItems(item);
                    _ = Task.Run( async () =>
                    {
                        bool success = await item.Process();
                        await Task.Delay(6000);
                        _workitemCollection.MarkItemAsProcessed(item.Name, success);
                        semaphore.Release();
                    });
                }
                await Task.Delay(CycleTimeInSeconds * 1000);
            }
        }
    }
}