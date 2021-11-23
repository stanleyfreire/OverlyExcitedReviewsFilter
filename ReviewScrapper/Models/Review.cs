using Newtonsoft.Json;
using ReviewScrapper.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReviewScrapper.Models
{
    [Serializable]
    public class Review
    {
        public string Author { get; set; }

        public DateTime Date { get; set; }

        public double StarScore { get; set; }

        public string Title { get; set; }

        public string ReviewText { get; set; }


        public List<Expression> Matches = new List<Expression>();

        public double UserScore { get; set; }

        public double SentenceScore { get; set; }

        public double WordScore { get; set; }

        public double TotalScore
        {
            get { return WordScore + UserScore + SentenceScore; }
        }

    }
}

