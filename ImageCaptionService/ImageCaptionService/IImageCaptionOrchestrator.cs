using Azure.Storage.Blobs;
using ImageCaptionServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using ImageCaptionService;

namespace ImageCaptionServices
{
    public class ImageCaptionOrchestrator : IImageCaptionOrchestrator
    {
        private readonly ILogger<ImageCaptionOrchestrator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IFetchImage _fetchImage;

        public ImageCaptionOrchestrator(ILogger<ImageCaptionOrchestrator> logger, 
                                        IConfiguration configuration,
                                        IFetchImage fetchImage)
        {
            _logger = logger;
            _configuration = configuration;
            _fetchImage = fetchImage;
        }

        public async Task<string> OrchestrateAsync(string imageName)
        {
            try
            {
                // Fetch the image
                var imageBase64 = await _fetchImage.FetchImageAsync(imageName);

                // Call the image caption service
                //var imageCaption = await _imageCaptionService.CaptionImageAsync(imageBase64);

                // Return the caption
                //return imageCaption;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ImageCaptionOrchestrator");
                throw;
            }

            return "Image Caption";
        }
    }
}
