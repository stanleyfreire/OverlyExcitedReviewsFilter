using ReviewScrapper.Interfaces;
using ReviewScrapper.Models;
using ReviewScrapper.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReviewScrapper
{
    public class Worker
    {
        private readonly IDataFetcherService _dataFetcherService;

        public Worker(IDataFetcherService dataFetcherService)
        {
            _dataFetcherService = dataFetcherService;
        }

        public async Task<List<Review>> RunEvaluation(string endpoint, int pages)
        {
            // fetch data from API
            var reviews = await _dataFetcherService.FetchData(endpoint, pages);


            return reviews;
            // call ExcitmentService to run tests on each review
        }

    }
}
