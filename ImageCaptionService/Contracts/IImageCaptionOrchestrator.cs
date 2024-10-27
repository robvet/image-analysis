namespace ImageCaptionServices
{
    public interface IImageCaptionOrchestrator
    {
        Task<string> OrchestrateAsync(string imageUrl);
    }
}