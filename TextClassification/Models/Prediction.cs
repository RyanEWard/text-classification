using Newtonsoft.Json;

namespace TextClassification.Models
{
    public class Prediction
    {
        [JsonProperty(Order = 1)]
        public string Content;
        [JsonProperty(Order = 2)]
        public int ClassificationId;
        [JsonProperty(Order = 3)]
        public string ClassificationName;
        [JsonProperty(Order = 4)]
        public double ClassificationSimilarityMeasure;
    }
}