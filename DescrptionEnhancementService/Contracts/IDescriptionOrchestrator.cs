namespace DescrptionEnhancementService
{
    public interface IDescriptionOrchestrator
    {
        Task<string> OrchestrateAsync(string productDescription);
    }
}