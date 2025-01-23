using Shared.Models;

namespace Shared.Contracts
{
    public interface IInferCaption
    {
        Task<ImageCaptionResult> InferImageCaptionAsync(byte[] imageBytes);
        //Task<ImageCaptionResult> InferImageCaptionAsync(byte[] imageBytes);
    }
}