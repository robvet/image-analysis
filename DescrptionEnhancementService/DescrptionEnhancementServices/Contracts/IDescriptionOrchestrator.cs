using ValidateImageCaptionAPI.Models;

namespace DescrptionEnhancementService.DescrptionEnhancementServices.Contracts
{
    public interface IDescriptionOrchestrator
    {
        Task<string> OrchestrateAsync(string productDescription);
    }
}