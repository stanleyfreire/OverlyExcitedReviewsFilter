using ReviewScrapper.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReviewScrapper.Interfaces
{
    public interface IDataFetcherService
    {
        public Task<List<Review>> FetchData(string endpoint, int pages);

    }
}
