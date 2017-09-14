using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TextClassification.Models
{
    public class Document
    {
        [Key, JsonProperty(Order = 1)]
        public int Id { get; set; }
        [JsonProperty(Order = 2)]
        public string Name { get; set; }
        [JsonProperty(Order = 3)]
        public int ClassificationId { get; set; }
        [JsonProperty(Order = 4)]
        public string Content { get; set; }

        [JsonProperty(Order = 5)]
        public virtual Classification Classification { get; set; }
    }
}