using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace iCare.Models
{
    public class Drug
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("sideEffects")]
        public List<string> SideEffects { get; set; }

        [JsonPropertyName("dosage")]
        public string Dosage { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }
    }
}
