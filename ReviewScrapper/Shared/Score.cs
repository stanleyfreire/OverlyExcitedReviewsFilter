using System.Collections.Generic;
using Newtonsoft.Json;

namespace ReviewScrapper.Shared
{
    public partial class Score
    {
        [JsonProperty("types")]
        public List<TypeElement> Types { get; set; }

        [JsonProperty("special_character")]
        public SpecialCharacter SpecialCharacter { get; set; }

        [JsonProperty("type_ratings")]
        public List<RatingElement> Ratings { get; set; }
    }

    public partial class SpecialCharacter
    {
        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("character")]
        public string Character { get; set; }
    }

    public partial class TypeElement
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }
    }

    public partial class RatingElement
    {
        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("rating")]
        public string Rating { get; set; }
    }
}
