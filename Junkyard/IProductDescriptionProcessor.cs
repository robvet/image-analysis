namespace DescrptionEnhancementService.DescrptionEnhancementServices.Contracts
{
    public interface IProductDescriptionProcessor
    {
        Task<string> GeneratePromptAsync(string description);
        Task<string> IdentifyCategory(string description);
    }
}