using Microsoft.Extensions.Configuration;
using ReviewScrapper.Interfaces;
using ReviewScrapper.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewScrapper.Services
{
    public class WorkerService : IWorkerService
    {
        #region Injection Services
        private readonly IDataFetcherService _dataFetcherService;
        private readonly IEvaluationService _evaluationService;
        private readonly IConfigurationRoot _configuration;
        #endregion

        public WorkerService(IDataFetcherService dataFetcherService, IEvaluationService evaluationService, IConfigurationRoot configuration)
        {
            _dataFetcherService = dataFetcherService;
            _evaluationService = evaluationService;
            _configuration = configuration;
        }

        public List<Review> Run(int pages)
        {
            try
            {
                Console.WriteLine("Fetching data from DealerRater.com...");
                var reviews = _dataFetcherService.FetchData(pages);

                if (reviews != null)
                {
                    Console.WriteLine("Data fetched!");

                    Console.WriteLine("Searching for duplicates..");
                    reviews = RemoveDuplicates(reviews);

                    Console.WriteLine("Evaluating Reviews...");

                    foreach (var review in reviews)
                    {
                        _evaluationService.EvaluateUser(review, reviews);
                        _evaluationService.EvaluateReviewBody(review);
                    }

                    return reviews = TieBreak(reviews);
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        private List<Review> RemoveDuplicates(List<Review> reviews)
        {
            List<Review> singleReviews = new List<Review>();

            foreach (Review review1 in reviews)
            {
                bool duplicatefound = false;
                foreach (Review review2 in singleReviews)
                    if (review1.Author == review2.Author && review1.ReviewText.Trim() == review2.ReviewText.Trim())
                        duplicatefound = true;

                if (!duplicatefound)
                    singleReviews.Add(review1);
            }
            return singleReviews;
        }

        private List<Review> TieBreak(List<Review> reviews)
        {
            return reviews.OrderByDescending(t => t.TotalScore).
                ThenByDescending(t => t.SentenceScore).
                ThenByDescending(t => t.StarScore).
                ThenByDescending(t => t.WordScore).
                ThenBy(t => t.Author).
                Take(Convert.ToInt32(_configuration["ResultCount"])).
                ToList();
        }
    }
}
