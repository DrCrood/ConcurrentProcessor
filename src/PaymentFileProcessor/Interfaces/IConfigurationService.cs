namespace WorkItemProcessor.Interfaces
{
    /// <summary>
    /// Configuration service interface 
    /// </summary>
    public interface IConfigurationService
    {
        public int GetMaxConCurrentFileProcessingThreads();
    }
}