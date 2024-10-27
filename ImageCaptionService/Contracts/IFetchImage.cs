namespace ImageCaptionService
{
    public interface IFetchImage
    {
        Task<string> FetchImageAsync(string imageName);
    }
}