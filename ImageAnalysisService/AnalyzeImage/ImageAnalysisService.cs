using Azure;
using Azure.AI.Vision.ImageAnalysis;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ImageAnalysisServices.Contracts;

namespace ImageAnalysisServices.AnalyzeImage
{
    public class ImageAnalysisService : IImageAnalysisService
    {
        private readonly ILogger<ImageAnalysisService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _endpoint;
        private readonly string _key;

        public ImageAnalysisService(IConfiguration configuration,
                                    ILogger<ImageAnalysisService> logger)
        {
            _logger = logger;

            _configuration = configuration;

            // Get the Azure OpenAI Service configuration values
            _endpoint = _configuration["VISION_ENDPOINT"]
                ?? throw new ArgumentException("VISION_ENDPOINT is Missing");

            _key = _configuration["VISION_KEY"]
                ?? throw new ArgumentException("VISION_KEY is Missing");
        }

        public ImageAnalysisResult AnalyzeImage(string imageName)
        {

            ImageAnalysisClient client = new ImageAnalysisClient(
                new Uri(_endpoint),
                new AzureKeyCredential(_key));

            ImageAnalysisResult result = client.Analyze(
                new Uri("https://learn.microsoft.com/azure/ai-services/computer-vision/media/quickstarts/presentation.png"),
                VisualFeatures.Caption | VisualFeatures.Read,
                new ImageAnalysisOptions { GenderNeutralCaption = true });

            //Console.WriteLine("Image analysis results:");
            //Console.WriteLine(" Caption:");
            //Console.WriteLine($"   '{result.Caption.Text}', Confidence {result.Caption.Confidence:F4}");

            //Console.WriteLine(" Read:");
            //foreach (DetectedTextBlock block in result.Read.Blocks)
            //    foreach (DetectedTextLine line in block.Lines)
            //    {
            //        Console.WriteLine($"   Line: '{line.Text}', Bounding Polygon: [{string.Join(" ", line.BoundingPolygon)}]");
            //        foreach (DetectedTextWord word in line.Words)
            //        {
            //            Console.WriteLine($"     Word: '{word.Text}', Confidence {word.Confidence.ToString("#.####")}, Bounding Polygon: [{string.Join(" ", word.BoundingPolygon)}]");
            //        }
            //    }
            //}

            //return new ImageAnalysisResult();
            return null;
        }
    }
}
