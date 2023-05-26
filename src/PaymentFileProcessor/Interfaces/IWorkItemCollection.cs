using ConcurrentProcessor.Models;

namespace WorkItemProcessor.Interfaces
{
    public interface IWorkItemCollection
    {
        bool Enqueue(WorkItem files);
        bool ContainsFile();
        WorkItem GetNext();
        void LogWorkingItems();
        int GetQueueSize();
        void MarkItemAsProcessed(string name, bool success);
        void AddWorkingFile(WorkItem item);
    }
}