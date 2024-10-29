using DescrptionEnhancementService.DescrptionEnhancementServices.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DescrptionEnhancementService.DescrptionEnhancementServices.Orchestrator
{
    public class DescriptionOrchestrator : IDescriptionOrchestrator
    {
        private readonly ILogger<DescriptionOrchestrator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDescriptionEnhancementService _descriptionEnhancementService;

        public DescriptionOrchestrator(ILogger<DescriptionOrchestrator> logger,
                                       IConfiguration configuration,
                                       IDescriptionEnhancementService descriptionEnhancementService)
        {
            _logger = logger;
            _configuration = configuration;
            _descriptionEnhancementService = descriptionEnhancementService;
        }

        public async Task<string> OrchestrateAsync(string productDescription)
        {
            try
            {
                // Enhance vendor description
                var customerDescription = await _descriptionEnhancementService.ChatCompletionAsync(productDescription);

                if (customerDescription == null)
                {
                    return string.Empty;
                }

                return customerDescription;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ImageCaptionOrchestrator");
                throw;
            }
        }
    }
}
