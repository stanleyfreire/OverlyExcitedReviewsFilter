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
    public class ExcitmentService : IExcitmentService
    {
        #region Injection Services
        private readonly IFileService _fileService;
        private readonly IConfigurationRoot _configuration;
        #endregion

        #region Properties
        private string filePath;
        private string splittedExpressions;
        private string pattern;
        private List<Expression> expresions = new List<Expression>();
        #endregion

        public ExcitmentService(IFileService fileService, IConfigurationRoot configuration)
        {
            _fileService = fileService;
            _configuration = configuration;

            try
            {
                filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    _configuration["ExpressionsFile"].ToString());
                expresions = JsonConvert.DeserializeObject<List<Expression>>(_fileService.GetStringFromFile(filePath));

                splittedExpressions = String.Join('|', expresions.Select(t => t.Definition));
                pattern = @$"(?:^|)({splittedExpressions})(?:$|)\b";
                
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void EvaluateReviewBody(Review review)
        {
            try
            {
                MatchCollection matches = Regex.Matches(review.ReviewText, pattern, RegexOptions.IgnoreCase);

                foreach (Match match in matches)
                {
                    //TODO: Replace?.. Don't know..
                    var expression = expresions.FirstOrDefault(t => t.Definition.Equals(match.Value.ToUpper().Replace(".", string.Empty), StringComparison.OrdinalIgnoreCase));
                    review.Matches.Add(new ReviewMatch() { Definition = match.Value, Type = expression.Type });

                    ScoreExpression(review, expression);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        
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

        private void ScoreExpression(Review review, Expression expression)
        {

            if (expression.Type.ToUpper() == "WORD")
            {
                var sentencesMatch = review.Matches.Where(t => t.Type == "SENTECE").ToList().Where(t => t.Definition.Contains(expression.Definition));

                review.WordScore += sentencesMatch.Count() > 1 ? (expression.Score / 2) : expression.Score;
            }
            else
                review.SentenceScore += expression.Score;

        }

    }
}
