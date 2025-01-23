using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using ImageCaptionService.Contracts;
using ImageAnalysisServices.Models;
using ImageAnalysisServices.Contracts;


namespace ImageAnalysisServices.Orchestrator
{
    public class ImageAnalysisOrchestrator : Contracts.IImageAnalysisOrchestrator
    {
        private readonly ILogger<ImageAnalysisOrchestrator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IFetchImage _fetchImage;
        private readonly IImageAnalysisService _analysisService;

        public ImageAnalysisOrchestrator(ILogger<ImageAnalysisOrchestrator> logger,
                                        IConfiguration configuration,
                                        IFetchImage fetchImage,
                                        IImageAnalysisService analysisService)
        {
            _logger = logger;
            _configuration = configuration;
            _fetchImage = fetchImage;
            _analysisService = analysisService;
        }

        public async Task<ImageAnalysisResult> OrchestrateAsync(string imageName)
        {
            try
            {
                // Fetch the image
                var imageBytes = await _fetchImage.FetchImageAsync(imageName);

                if (imageBytes == null)
                {
                    return new ImageAnalysisResult("Not Found", 0);
                }

                var imageAnalysis = await _analysisService.AnalyzeImage(imageName);

                if (imageAnalysis == null)
                {
                    return new ImageAnalysisResult("Not Found", 0);
                }

                return imageAnalysis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ImageCaptionOrchestrator");
                throw;
            }
        }
    }
}
