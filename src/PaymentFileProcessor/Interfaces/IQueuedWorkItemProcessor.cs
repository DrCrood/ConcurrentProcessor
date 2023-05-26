using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorkItemProcessor.Interfaces
{
    public interface IQueuedWorkItemProcessor
    {
        void Start();
    }
}