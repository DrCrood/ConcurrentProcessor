using System.Diagnostics.CodeAnalysis;
using WorkItemProcessor.Interfaces;

namespace WorkItemProcessor.Services
{
    public class UtilityService : IUtilityService
    {
        private readonly ILogger<UtilityService> _logger;

        /// <summary>
        /// Create a UtilityService instance.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public UtilityService(ILogger<UtilityService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [ExcludeFromCodeCoverage]
        public DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
    }
}
