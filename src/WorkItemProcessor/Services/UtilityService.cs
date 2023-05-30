using System.Diagnostics.CodeAnalysis;
using WorkItemProcessor.Interfaces;

namespace WorkItemProcessor.Services
{
    public class UtilityService : IUtilityService
    {
        private readonly ILogger<UtilityService> _logger;

        public UtilityService(ILogger<UtilityService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
    }
}
