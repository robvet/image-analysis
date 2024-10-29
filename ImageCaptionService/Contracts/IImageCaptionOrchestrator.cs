using ValidateImageCaptionAPI.Models;

namespace ImageCaptionService.Contracts
{
    public interface IImageCaptionOrchestrator
    {
        Task<ImageCaptionResult> OrchestrateAsync(string imageName);
        //Task<string> OrchestrateAsync(string imageUrl);
    }
}