using ConcurrentProcessor.Models;

namespace WorkItemProcessor.Interfaces
{
    public interface IWorkItemCollection
    {
        bool Enqueue(WorkItem items);
        bool ContainsWorkItem();
        WorkItem GetNext();
        void LogWorkingItems();
        int GetQueueSize();
        void MarkItemAsProcessed(string name, bool success);
        void AddWorkingItems(WorkItem item);
    }
}