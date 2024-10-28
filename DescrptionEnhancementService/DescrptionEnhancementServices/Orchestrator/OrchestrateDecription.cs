using DescrptionEnhancementService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DescrptionEnhancementService
{
    public class DescriptionOrchestrator : IDescriptionOrchestrator
    {
        private readonly ILogger<DescriptionOrchestrator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IProductDescriptionProcessor _productDescriptionProcessor;

        public DescriptionOrchestrator(ILogger<DescriptionOrchestrator> logger,
                                       IConfiguration configuration,
                                       IProductDescriptionProcessor productDescriptionProcessor)
        {
            _logger = logger;
            _configuration = configuration;
            _productDescriptionProcessor = productDescriptionProcessor;
        }

        public async Task<string> OrchestrateAsync(string productDescription)
        {
            try
            {
                // Fetch the image
                var productCategory = await _productDescriptionProcessor.IdentifyCategory(productDescription);

                if (productCategory == null)
                {
                    return string.Empty;
                }

                //var imageCaption = await _inferCaption.InferImageCaptionAsync(imageBytes);

                //if (string.IsNullOrEmpty(imageCaption))
                //{
                //    return string.Empty;
                //}

                // Call the image caption service
                //var imageCaption = await _imageCaptionService.CaptionImageAsync(imageBase64);

                // Return the caption
                return productCategory;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ImageCaptionOrchestrator");
                throw;
            }

            return null;

        }
    }
}
