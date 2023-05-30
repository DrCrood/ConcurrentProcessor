namespace WorkItemProcessor.Models
{
    /// <summary>
    /// Status of WorkItem 
    /// </summary>
    public enum WorkItemStatus
    {
        Queued,
        Processing,
        Completed,
        ProcessException,
        Unknown
    }
}
