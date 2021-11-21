using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReviewScrapper.Shared
{
    public class ReviewMatch
    {
        [JsonProperty("definition")]
        public string Definition { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
