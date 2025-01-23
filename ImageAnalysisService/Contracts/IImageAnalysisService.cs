using Azure.AI.Vision.ImageAnalysis;

namespace ImageAnalysisServices.Contracts
{
    public interface IImageAnalysisService
    {
        ImageAnalysisResult AnalyzeImage(string imageName);
    }
}