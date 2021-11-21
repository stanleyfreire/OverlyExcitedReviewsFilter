using Microsoft.Extensions.Configuration;
using ReviewScrapper.Interfaces;
using ReviewScrapper.Models;
using System;
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

        public async Task<List<Review>> Run(string endpoint, int pages)
        {
            try
            {
                Console.WriteLine("Fetching data from API..");
                var reviews = await _dataFetcherService.FetchData(endpoint, pages);
                Console.WriteLine("Data fetched!");

                foreach (var review in reviews)
                {
                    _evaluationService.EvaluateUser(review, reviews);
                    _evaluationService.EvaluateReviewBody(review);
                }

                return reviews.Where(t => t.TotalScore >= Convert.ToDouble(_configuration["ExcitmentThreshold"])).OrderByDescending(t => t.TotalScore).
                    Take(Convert.ToInt32(_configuration["ResultCount"])).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
