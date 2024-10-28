namespace ImageCaptionService.Contracts
{
    public interface IImageCaptionOrchestrator
    {
        Task<string> OrchestrateAsync(string imageUrl);
    }
}