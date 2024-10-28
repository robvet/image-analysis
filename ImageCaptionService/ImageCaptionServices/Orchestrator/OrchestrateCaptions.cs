using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using ImageCaptionService.Contracts;

namespace ImageCaptionService.ImageCaptionServices.Orchestrator
{
    public class ImageCaptionOrchestrator : IImageCaptionOrchestrator
    {
        private readonly ILogger<ImageCaptionOrchestrator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IFetchImage _fetchImage;
        private readonly IInferCaption _inferCaption;

        public ImageCaptionOrchestrator(ILogger<ImageCaptionOrchestrator> logger,
                                        IConfiguration configuration,
                                        IFetchImage fetchImage,
                                        IInferCaption inferCaption)
        {
            _logger = logger;
            _configuration = configuration;
            _fetchImage = fetchImage;
            _inferCaption = inferCaption;
        }

        public async Task<string> OrchestrateAsync(string imageName)
        {
            try
            {
                // Fetch the image
                var imageBytes = await _fetchImage.FetchImageAsync(imageName);

                if (imageBytes == null)
                {
                    return string.Empty;
                }

                var imageCaption = await _inferCaption.InferImageCaptionAsync(imageBytes);

                if (string.IsNullOrEmpty(imageCaption))
                {
                    return string.Empty;
                }

                // Call the image caption service
                //var imageCaption = await _imageCaptionService.CaptionImageAsync(imageBase64);

                // Return the caption
                return imageCaption;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ImageCaptionOrchestrator");
                throw;
            }
        }
    }
}
