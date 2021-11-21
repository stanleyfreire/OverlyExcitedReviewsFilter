using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReviewScrapper.Shared
{
    public class Expression
    {
        [JsonProperty("definition")]
        public string Definition { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("rating")]
        public string Rating { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }

    }
}
