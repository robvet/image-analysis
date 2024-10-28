namespace ImageCaptionService.Contracts
{
    public interface IFetchImage
    {
        //Task<string> FetchImageAsync(string imageName);
        Task<byte[]> FetchImageAsync(string imageName);
    }
}