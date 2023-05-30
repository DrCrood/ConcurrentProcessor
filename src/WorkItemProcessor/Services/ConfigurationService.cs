using System.Text;
using System.Text.Json;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using WorkItemProcessor.Interfaces;
using WorkItemProcessor.Models;

namespace WorkItemProcessor.Services
{
    /// <summary>
    /// Provide configuration data service
    /// </summary>
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private int _maxConCurrentProcessingItems;

        public ConfigurationService(IConfiguration configuration, ILogger<ConfigurationService> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger?? throw new ArgumentNullException(nameof(logger));
            
            SetMaxConcurrentProcessingItems();
        }

        /// <summary>
        /// Return a configration value by key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public string GetConfigurationSectionValue(string key)
        {
            return _configuration.GetSection(key).Value ?? throw new Exception($"The configuration item {key} is missing.");
        }

        private void SetMaxConcurrentProcessingItems()
        {
            try
            {
                string max = GetConfigurationSectionValue("MaxConCurrentProcessingItems");
                _ = int.TryParse(max, out _maxConCurrentProcessingItems);

                if (_maxConCurrentProcessingItems < 1 || _maxConCurrentProcessingItems > 20)
                {
                    _maxConCurrentProcessingItems = 5;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("SetMaxConcurrentProcessingItems exception: {ex}", ex);
                _maxConCurrentProcessingItems = 5;
            }
        }

        public int GetMaxConCurrentItemProcessingThreads()
        {
            return _maxConCurrentProcessingItems;
        }
    }
}
