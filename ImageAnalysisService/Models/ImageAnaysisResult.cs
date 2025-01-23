using Newtonsoft.Json;

namespace ImageAnalysisServices.Models
{
    public class ImageAnalysisResult

    {
        public ImageAnalysisResult(string targetObject, double probability)
        {
            TargetObject = targetObject;
            Probability = probability;
        }

        [JsonProperty("object")]
        public string TargetObject { get; set; }

        [JsonProperty("probability")]
        public double Probability { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("volume")]
        public string volume { get; set; }

        [JsonProperty("isfood")]
        public bool isFood { get; set; }

        [JsonProperty("isdrink")]
        public bool isDrink { get; set; }

        [JsonProperty("isalcoholic")]
        public bool isAlcoholic { get; set; }

        [JsonProperty("upc")]
        public string UPC { get; set; }

        [JsonProperty("manufacturer")]
        public string Manufacturer { get; set; }
    }
}
