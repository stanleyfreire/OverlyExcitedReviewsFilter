using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ReviewScrapper.Interfaces;
using ReviewScrapper.Models;
using ReviewScrapper.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ReviewScrapper.Services
{
    public class EvaluationService : IEvaluationService
    {
        #region Injection Services
        private readonly IFileService _fileService;
        private readonly IConfigurationRoot _configuration;
        #endregion

        #region Properties
        private string splittedExpressions;
        private string pattern;
        private List<Expression> expressions = new List<Expression>();
        private Score score = new Score();
        #endregion

        public EvaluationService(IFileService fileService, IConfigurationRoot configuration)
        {
            _fileService = fileService;
            _configuration = configuration;

            try
            {
                string expressionsFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    _configuration["ExpressionsFile"]);

                expressions = JsonConvert.DeserializeObject<List<Expression>>(_fileService.GetStringFromFile(expressionsFilePath));

                string scoresFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    _configuration["ScoresFile"]);

                score = JsonConvert.DeserializeObject<Score>(_fileService.GetStringFromFile(scoresFilePath));

                splittedExpressions = String.Join('|', expressions.Select(t => t.Definition));
                pattern = @$"({splittedExpressions}){_configuration["RegexPattern"]}";

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// Evaluates the body of the review
        /// </summary>
        /// <param name="review"></param>
        public void EvaluateReviewBody(Review review)
        {
            try
            {
                MatchCollection matches = Regex.Matches(review.ReviewText, pattern, RegexOptions.IgnoreCase);

                foreach (Match match in matches)
                {
                    // scores type + characters
                    var expression = GetExpressionWithScore(match);

                    // scores expressions and check if words are contained inside sentences.
                    CheckIfWordIsContainedInAnySentence(review, expression);

                    review.Matches.Add(new ReviewMatch() { Definition = expression.Definition, Type = expression.Type });

                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Evaluate if user is contained more than once on the list
        /// </summary>
        /// <param name="review"></param>
        /// <param name="reviews"></param>
        public void EvaluateUser(Review review, List<Review> reviews)
        {
            try
            {
                var userOccurences = reviews.Where(t => t.Author == review.Author);
                review.UserScore = userOccurences != null ? userOccurences.Count() : 1;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// Checks if word is contained inside a sentence. If so, their score is cut by half.
        /// </summary>
        /// <param name="review"></param>
        /// <param name="expression"></param>
        private void CheckIfWordIsContainedInAnySentence(Review review, Expression expression)
        {
            try
            {
                if (expression.Type == "Word")
                {
                    var sentencesMatch = review.Matches.Where(t => t.Type == "Sentence").ToList().Where(t => t.Definition.Contains(expression.Definition));

                    review.WordScore += sentencesMatch.Count() > 1 ? (expression.Score / 2) : expression.Score;
                }
                else
                    review.SentenceScore += expression.Score;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// Returns Expression object with score and rating after evaluating if specials characters are contained on given Match
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private Expression GetExpressionWithScore(Match match)
        {
            try
            {
                Expression expression;
                string definition;

                string ch = score.SpecialCharacter.Character;

                //how many times the special occured on given string
                int occurences = match.Value.Trim().Where(x => (x.ToString() == ch)).Count();

                //remove special char from string
                definition = match.Value.Trim().Replace(ch, string.Empty).Trim();

                //create a new expression with new definifion value (after special chars have been removed)
                expression = expressions.FirstOrDefault(t => t.Definition.Equals(definition, StringComparison.OrdinalIgnoreCase));

                //assign expression rating and scores based on how many ponctuations were found on the string
                var returnExpression = RateExpression(expression, occurences);

                return returnExpression;
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

        /// <summary>
        /// Calculates score for Expression using the Score field and special characters occurences
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="occurences"></param>
        /// <returns></returns>
        private Expression RateExpression(Expression expression, int occurences)
        {
            try
            {
                //deepcopy (avoid assign value by reference)
                Expression returnExpression = new Expression()
                {
                    Definition = expression.Definition,
                    Type = expression.Type
                };

                // assign a score for the type of expression. example: if expression is a word, the score is 1 (depends os json config file)
                returnExpression.Score = score.Types.FirstOrDefault(t => t.Type.Equals(returnExpression.Type, StringComparison.OrdinalIgnoreCase)).Score;

                // how many special characters in string?
                if (occurences == 0)
                    returnExpression.Rating = score.Ratings.FirstOrDefault().Rating;
                else if (occurences > 0)
                {
                    // multiplies it by times it occured, adding with the existent type score
                    returnExpression.Score += score.SpecialCharacter.Score * occurences;

                    if (occurences > score.Ratings.Last().Score)
                        returnExpression.Rating = score.Ratings.Last().Rating;
                    else
                    {
                        if (score.Ratings.FirstOrDefault(t => t.Score == returnExpression.Score) != null)
                            returnExpression.Rating = score.Ratings.FirstOrDefault(t => t.Score == returnExpression.Score).Rating;
                        else
                            returnExpression.Rating = score.Ratings.Last().Rating;
                    }

                }

                return returnExpression;
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }
    }
}
