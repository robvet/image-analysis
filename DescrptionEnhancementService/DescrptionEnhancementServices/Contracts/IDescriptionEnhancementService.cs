namespace DescrptionEnhancementService.DescrptionEnhancementServices.Contracts
{
    public interface IDescriptionEnhancementService
    {
        Task<string> ChatCompletionAsync(string vendorDescription);
    }
}