using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReviewScrapper.Models
{
    [Serializable]
    public class Review
    {
        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("reviewText")]
        public string ReviewText { get; set; }

        //TODO: VAI FICAR TUDO NO MESMO OBJECT QUE O RESPONSE? N SEI.. MAYBE

        [JsonIgnore]
        public List<string> MatchedSentences = new List<string>();

        [JsonIgnore]
        public List<string> MatchedWords = new List<string>();

        [JsonIgnore]
        public double UserScore { get; set; }

        [JsonIgnore]
        public double SentenceScore { get; set; }

        [JsonIgnore]
        public double WordScore { get; set; }

        [JsonIgnore]
        public double TotalScore
        {
            get { return WordScore + UserScore + SentenceScore; }
        }

    }
}

