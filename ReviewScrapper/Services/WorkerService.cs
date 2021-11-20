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
        private readonly IDataFetcherService _dataFetcherService;
        private readonly IExcitmentService _excitmentService;

        public WorkerService(IDataFetcherService dataFetcherService, IExcitmentService excitmentService)
        {
            _dataFetcherService = dataFetcherService;
            _excitmentService = excitmentService;
        }

        public async Task<List<Review>> RunEvaluation(string endpoint, int pages)
        {
            try
            {
                var reviews = await _dataFetcherService.FetchData(endpoint, pages);

                foreach (var review in reviews)
                {
                    _excitmentService.EvaluateSentences(review);
                    _excitmentService.EvaluateWords(review);
                    _excitmentService.EvaluateUser(review, reviews);
                }

                var filteredReviews = reviews.Where(t => t.TotalScore >= 5).ToList();
                return filteredReviews;
            }
            catch (Exception e)
            {
                throw e;
            }
            

        }
    }
}
