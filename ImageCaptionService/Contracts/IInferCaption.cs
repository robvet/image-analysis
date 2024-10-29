using ValidateImageCaptionAPI.Models;

namespace ImageCaptionService.Contracts
{
    public interface IInferCaption
    {
        Task<ImageCaptionResult> InferImageCaptionAsync(byte[] imageBytes);
        //Task<ImageCaptionResult> InferImageCaptionAsync(byte[] imageBytes);
    }
}