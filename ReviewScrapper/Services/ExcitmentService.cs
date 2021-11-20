using ReviewScrapper.Interfaces;
using ReviewScrapper.Models;
using ReviewScrapper.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ReviewScrapper.Services
{
    public class ExcitmentService : IExcitmentService
    {
        public void EvaluateSentences(Review review)
        {
            try
            {
                string sentences = String.Join('|', Sentences.SentencesToMatch());
                string regex = @$"(?:^|(?<= ))({sentences})(?:(?= )|$)";

                MatchCollection matches = Regex.Matches(review.ReviewText, regex, RegexOptions.IgnoreCase);

                foreach (Match match in matches)
                {
                    review.MatchedSentences.Add(match.Value);
                    review.SentenceScore += 2; // 2 points for each sentence match
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void EvaluateUser(Review review, List<Review> reviews)
        {
            var userOccurences = reviews.Where(t => t.Author == review.Author);
            review.UserScore = userOccurences.Count(); // 1 point for each occurence on the list
        }

        public void EvaluateWords(Review review)
        {

            try
            {
                string words = String.Join('|', Words.WordsToMatch());
                string regex = @$"(?:^|(?<= ))({words})(?:(?= )|$)";

                MatchCollection matches = Regex.Matches(review.ReviewText, regex, RegexOptions.IgnoreCase);
                double score = 0;

                foreach (Match match in matches)
                {

                    if (review.MatchedWords.Contains(match.Value))
                        continue;


                    review.MatchedWords.Add(match.Value);
                    score = review.MatchedSentences.Any(t => t.Contains(match.Value)) ? 0.5 : 1;
                    review.WordScore += score; // 1 point for each word match that has not been found inside the sentences. half point if already found inside a sentence.
                }
            }
            catch (Exception e)
            {
                throw e;
            }


        }
    }
}
