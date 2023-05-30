using System;
using System.Collections.Concurrent;
using ConcurrentProcessor.Models;
using WorkItemProcessor.Interfaces;

namespace WorkItemProcessor
{
    /// <summary>
    /// Class representing a collection of work items.
    /// </summary>
    public class WorkItemCollection : IWorkItemCollection
    {
        private readonly ILogger<WorkItemCollection> _logger;
        private readonly BlockingCollection<WorkItem> workitemQueue = new();
        private readonly ConcurrentDictionary<string, WorkItem> _workingWorkItems = new();

        public WorkItemCollection(ILogger<WorkItemCollection> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Add a item to the collection
        /// </summary>
        /// <param name="workItem"></param>
        /// <returns></returns>
        public bool Enqueue(WorkItem workItem)
        {
            try
            {
                workitemQueue.Add(workItem);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception caught when adding a work item to the queue: {msg}", ex);
                return false;
            }
        }

        /// <summary>
        /// Get&remove a item from the collection queue.
        /// </summary>
        /// <returns></returns>
        public WorkItem GetNext()
        {
            return workitemQueue.Take();
        }

        public int GetQueueSize()
        {
            return workitemQueue.Count;
        }

        /// <summary>
        /// Check if the queue contains any item.
        /// </summary>
        /// <returns></returns>
        public bool ContainsWorkItem()
        {
            return workitemQueue.Any();
        }

        /// <summary>
        /// Add the item to the in-processing item list.
        /// </summary>
        /// <param name="item"></param>
        public void AddWorkingItems(WorkItem item)
        {
            _workingWorkItems.TryAdd(item.Name, item);
        }

        /// <summary>
        /// Remove the item from the in-processing item list.
        /// </summary>
        /// <param name="name"></param>
        public void MarkItemAsProcessed(string name, bool success = false)
        {
            bool removed = _workingWorkItems.Remove(name, out WorkItem? _);
            if(!removed)
            {
                _logger.LogError("MarkItemAsProcessed: The name {name} is not found in the working item list.", name);
                return;
            }
        }

        /// <summary>
        /// Log the info of items which are in processing.
        /// </summary>
        public void LogWorkingItems()
        {
            if(_workingWorkItems.Count + workitemQueue.Count < 1)
            {
                return;
            }

            List<string> logs = new();
            int index = 1;
            logs.Add("");
            foreach (var item in workitemQueue)
            {
                logs.Add(index + ": " + item.Name + " " + item.Size.ToString() + " " + item.Status.ToString());
                index++;
            }

            foreach (var item in _workingWorkItems)
            {
                logs.Add(index + ": " + item.Key + " " + item.Value.Size + " " + item.Value.Status.ToString());
                index++;
            }
            logs.Add("");
            _logger.LogInformation("{items}", string.Join(Environment.NewLine, logs));
        }

    }
}
