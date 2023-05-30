using WorkItemProcessor.Models;

namespace WorkItemProcessor.Interfaces
{
    public interface IWorkItemMonitoringService
    {
        Task<List<string>> QueueWorkItems();
        public void LogWorkingWorkItems();
    }
}