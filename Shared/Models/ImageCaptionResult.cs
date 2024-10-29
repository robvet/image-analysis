using Newtonsoft.Json;

namespace ValidateImageCaptionAPI.Models
{
    public class ImageCaptionResult
    {
        public ImageCaptionResult(string targetObject, double probability)
        {
            TargetObject = targetObject;
            Probability = probability;
        }

        [JsonProperty("object")]
        public string TargetObject { get; set; }

        [JsonProperty("probability")]
        public double Probability { get; set; }
    }
}
