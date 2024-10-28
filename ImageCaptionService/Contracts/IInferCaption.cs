namespace ImageCaptionService.Contracts
{
    public interface IInferCaption
    {
        //Task<string> InferImageCaptionAsync(string imageBase64);
        Task<string> InferImageCaptionAsync(byte[] imageBytes);
    }
}