using WorkItemProcessor.Models;

namespace ConcurrentProcessor.Models
{
    /// <summary>
    /// Work item class. 
    /// </summary>
    public class WorkItem
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public DateTime QueueTime { get; set; }
        public DateTime ProcessStart { get; set; }
        public DateTime ProcessEnd { get; set; }
        public string? ProcessSummary { get; set; }
        public WorkItemStatus Status { get; set; }
        public ILogger<WorkItem>? Logger { get; set; }
        public WorkItem(string name, int size)
        {
            Name = name;
            Size = size;
            Status = WorkItemStatus.Unknown;
        }

        public async Task<bool> Process()
        {
            try
            {
                ProcessStart = DateTime.Now;
                Status = WorkItemStatus.Processing;
                await WaitWorkItemSizeTime();

                Status = WorkItemStatus.Completed;
                ProcessSummary = "WorkItem processed successfully.";
                ProcessEnd = DateTime.Now;
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught when procesing work item {Name}, exception: {ex}");
                Status = WorkItemStatus.ProcessException;
                ProcessSummary = "Exception: " + ex.Message;
                ProcessEnd = DateTime.Now;
                return false;
            }
        }

        public async Task WaitWorkItemSizeTime()
        {
            await Task.Delay(Size * 1000);
        }
    }
}
