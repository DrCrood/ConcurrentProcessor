using WorkItemProcessor.Interfaces;
using WorkItemProcessor.Models;
using ConcurrentProcessor.Models;

namespace WorkItemProcessor.Services
{
    /// <summary>
    /// Service for monitoring items and queue items for processing.
    /// </summary>
    public class WorkItemMonitoringService : IWorkItemMonitoringService
    {
        private readonly ILogger<WorkItemMonitoringService> _logger;
        private readonly IWorkItemCollection _workitemCollection;
        private readonly IUtilityService _utilityService;

        /// <summary>
        /// Payment item parsing service
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="itemCollection"></param>
        /// <param name="utilityService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public WorkItemMonitoringService(ILogger<WorkItemMonitoringService> logger, IWorkItemCollection itemCollection, IUtilityService utilityService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _workitemCollection = itemCollection ?? throw new ArgumentNullException(nameof(itemCollection));
            _utilityService = utilityService ?? throw new ArgumentNullException(nameof(utilityService));
        }

        /// <summary>
        /// Queue payment items for processing.
        /// </summary>
        /// <param name="fileConfigs"></param>
        /// <returns>List of queued items</returns>
        public async Task<List<string>> QueueWorkItems()
        {
            List<string> queuedItems = new();

            try
            {
                List<WorkItem> items = await GetWorkItems();
                foreach (var item in items)
                {
                    item.QueueTime = _utilityService.GetCurrentDateTime();
                    item.Status = WorkItemStatus.Queued;
                    _workitemCollection.Enqueue(item);
                    queuedItems.Add(item.Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception caught in QueueWorkItems: {ex}", ex);
            }

            return queuedItems;
        }

        public async Task<List<WorkItem>> GetWorkItems()
        {
            Random random = new Random();
            int count = random.Next(0, 3);
            List<WorkItem> items = new List<WorkItem>();

            for (int i = 0; i < count; i++)
            {
                string name = "";
                for (int n = 0; n < 6; n++)
                {
                    name += Convert.ToChar(random.Next(65, 91));
                }

                name += "_" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".dat";
                int size = random.Next(1, 11);
                size = size > 85 ? random.Next(1, 100) : size;
                size = size > 95 ? random.Next(1, 2000) : size;
                WorkItem item = new WorkItem(name, size);
                await Task.Delay(1000);
                items.Add(item);
            }
            return items;
        }

        public void LogWorkingWorkItems()
        {
            _workitemCollection.LogWorkingItems();
        }
    }
}